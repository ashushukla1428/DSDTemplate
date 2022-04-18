using OpenPop.Mime;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;

namespace Wempe.Controllers
{
    public class ReadMailController : Controller
    {
        //
        // GET: /ReadMail/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            try
            {
                readMailBox();
                return View();
            }
            catch (Exception ex)
            {
                return View();
                //please opne this link https://www.google.com/settings/security/lesssecureapps and "Access for less secure apps" set "On" 
               //
            }
        }

        [HttpPost]
        public JsonResult readMailBox()
        {
            try
            {
               // var _items = db.Database.SqlQuery<AppraiserModel>("USP_GetAppraiser @p0, @p1, @p2, @p3, @p4,@p5", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                Pop3Client pop3Client;
                if (Session["Pop3Client"] == null)
                {
                    pop3Client = new Pop3Client();
                    pop3Client.Connect("pop.gmail.com", 995, true);
                    pop3Client.Authenticate("impingetest1@gmail.com", "");
                    Session["Pop3Client"] = pop3Client;
                }
                else
                {
                    pop3Client = (Pop3Client)Session["Pop3Client"];
                }
                
                int count = pop3Client.GetMessageCount();
                // Fetch all the current uids seen
              List<string> uids = pop3Client.GetMessageUids();
                int counter = 0;
                for (int i = count; i >= 1; i--)
                {
                    Message message = pop3Client.GetMessage(i);
                    
                    //message.Headers.MessageId
                    wmpMailBox messageModel = new wmpMailBox();
                    MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                    string body;
                    if (plainTextPart != null)
                    {
                        // The message had a text/plain version - show that one
                        body = plainTextPart.GetBodyAsText();
                    }
                    else
                    {
                        // Try to find a body to show in some of the other text versions
                        List<MessagePart> textVersions = message.FindAllTextVersions();
                        if (textVersions.Count >= 1)
                            body = textVersions[0].GetBodyAsText();
                        else
                            body = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                    }
                    
                    messageModel.MessageBody = body;
                    messageModel.LastUpdate = DateTime.Now;
                    messageModel.MessageId = message.Headers.MessageId;
                    messageModel.Subject = message.Headers.Subject;
                    messageModel.FromAddress = message.Headers.From.Address;
                    messageModel.DisplayName = message.Headers.From.DisplayName;
                    messageModel.DateSent = message.Headers.DateSent;
                    messageModel.OwnerID = SessionMaster.Current.OwnerID;
                    messageModel.UserID = SessionMaster.Current.LoginId;
                    // Build up the attachment list
                    List<MessagePart> attachments = message.FindAllAttachments();
                    foreach (MessagePart attachment in attachments)
                    {
                        messageModel.HaveAttachment = true;
                    }

                    if (!db.wmpMailBoxes.Any(c => c.MessageId == messageModel.MessageId))
                    {
                        db.wmpMailBoxes.Add(messageModel);
                        db.SaveChanges();
                        foreach (var item in message.Headers.Bcc)
                        {
                            wmpMailBoxEmailAddress emailAddress = new wmpMailBoxEmailAddress();
                            emailAddress.AddressType = "BCC";
                            emailAddress.DisplayName = item.DisplayName;
                            emailAddress.HasValidMailAddress = item.HasValidMailAddress;
                            emailAddress.MailAddress = item.MailAddress.ToString();
                            emailAddress.MailRelId = messageModel.MessageNumber;
                            db.wmpMailBoxEmailAddresses.Add(emailAddress);
                            db.SaveChanges();
                        }

                        foreach (var item in message.Headers.Cc)
                        {
                            wmpMailBoxEmailAddress emailAddress = new wmpMailBoxEmailAddress();
                            emailAddress.AddressType = "BCC";
                            emailAddress.DisplayName = item.DisplayName;
                            emailAddress.HasValidMailAddress = item.HasValidMailAddress;
                            emailAddress.MailAddress = item.MailAddress.ToString();
                            emailAddress.MailRelId = messageModel.MessageNumber;
                            //messageModel.BCC = messageModel.BCC + item.Address;
                            db.wmpMailBoxEmailAddresses.Add(emailAddress);
                            db.SaveChanges();
                        }

                       
                    }
                    counter++;
                    if (counter > 5)
                    {
                        break;
                    }
                }
                pop3Client.Dispose();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (InvalidLoginException)
            {
                //MessageBox.Show(this, "The server did not accept the user credentials!", "POP3 Server Authentication");
                return Json("The server did not accept the user credentials!", "POP3 Server Authentication");
            }
            catch (PopServerNotFoundException)
            {
                //MessageBox.Show(this, "The server could not be found", "POP3 Retrieval");
                return Json("The server could not be found", "POP3 Retrieval");
            }
            catch (PopServerLockedException)
            {
                //MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked");
                return Json("The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked");
            }
            catch (LoginDelayException)
            {
                //MessageBox.Show(this, "Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay");
                return Json("Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay");
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }

    }
}
