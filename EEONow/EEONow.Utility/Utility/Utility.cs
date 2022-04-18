using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Xml;
using System.Threading;
using EEONow.Context.EntityContext;

namespace EEONow.Utilities
{
    public class AppUtility
    {
        private const CipherMode cipherMode = CipherMode.CBC;
        private const PaddingMode paddingMode = PaddingMode.ISO10126;
        private const string defaultVector = "fdsah123456789";
        private const int iterations = 2;
        public static string CreateSalt()
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[20];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }
        public static string Encrypt(string plainText, string passphrase)
        {
            byte[] clearData = Encoding.Unicode.GetBytes(plainText);
            byte[] encryptedData;
            var crypt = GetCrypto(passphrase);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearData, 0, clearData.Length);
                    //cs.FlushFinalBlock(); //Have tried this active and commented with no change.
                }
                encryptedData = ms.ToArray();
            }
            //Changed per Xint0's answer.
            return Convert.ToBase64String(encryptedData);
        }
        public static string Decrypt(string cipherText, string passphrase)
        {
            //Changed per Xint0's answer.
            if (!String.IsNullOrEmpty(cipherText))
            {
                byte[] encryptedData = Convert.FromBase64String(cipherText);
                byte[] clearData;
                var crypt = GetCrypto(passphrase);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, crypt.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, 0, encryptedData.Length);
                        //I have tried adding a cs.FlushFinalBlock(); here as well.
                    }
                    clearData = ms.ToArray();
                }
                return Encoding.Unicode.GetString(clearData);
            }
            else
            {
                return null;
            }
        }
        private static Rijndael GetCrypto(string passphrase)
        {
            var crypt = Rijndael.Create();
            crypt.Mode = cipherMode;
            crypt.Padding = paddingMode;
            crypt.BlockSize = 256;
            crypt.KeySize = 256;
            crypt.Key =
                new Rfc2898DeriveBytes(passphrase, Encoding.Unicode.GetBytes(defaultVector), iterations).GetBytes(32);
            crypt.IV = new Rfc2898DeriveBytes(passphrase, Encoding.Unicode.GetBytes(defaultVector), iterations).GetBytes(32);
            return crypt;
        }
        public static void SendEmail(string to, string body, string subject)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }
        public static void SendEmailToMultipeRecipientsBCC(string[] to, string body, string subject)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(ConfigurationManager.AppSettings["NotificationEmails"]);

            foreach (var item in to)
            {
                mail.Bcc.Add(item);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }
        public static void SendEmailToMultipeRecipients(string[] to, string body, string subject)
        {
            MailMessage mail = new MailMessage();
            foreach (var item in to)
            {
                mail.To.Add(item);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static int GetCompanyId()
        {
            return 0;
        }
        public static void SetCookiesData(LoginResponse model)
        {
            FormsAuthentication.SetAuthCookie(model.Email, false);
            var userData = JsonConvert.SerializeObject(model);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
           model.Email,
           DateTime.Now,
           DateTime.Now.AddMinutes(30),
           true,
           userData,
           FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("Role", model.Roles));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserRoleId", model.UserRoleId.ToString()));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("IsAdd", model.IsAdd.ToString()));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("IsEdit", model.IsEdit.ToString()));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("IsFilter", model.IsFilter.ToString()));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserName", model.UserName.ToString()));
        }
        public static void SetOrgIdForAdminView(int orgId)
        {
            try
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("TempOrgId", orgId.ToString()));
            }
            catch
            {
            }
        }
        public static String GetOrgIdForAdminView()
        {
            try
            {
                string result = "";
                var httpCookie = HttpContext.Current.Request.Cookies["TempOrgId"];
                if (httpCookie != null)
                {
                    result = httpCookie.Value;
                }
                return result;
            }
            catch
            {
                return "";

            }

        }
        public static void SetRoleIdForAdminView(int roleId)
        {
            try
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("TempRoleId", roleId.ToString()));
            }
            catch
            {
            }
        }
        public static String GetRoleIdForAdminView()
        {
            try
            {
                string result = "";
                var httpCookie = HttpContext.Current.Request.Cookies["TempRoleId"];
                if (httpCookie != null)
                {
                    result = httpCookie.Value;
                }
                return result;
            }
            catch
            {
                return "";

            }

        }
        public static void ClearDataFromCookie(String cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
            }
        }
        public static LoginResponse DecryptCookie()
        {
            var httpCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (httpCookie == null)
            {
                return null;
            }

            //Here throws error!
            var decryptedCookie = FormsAuthentication.Decrypt(httpCookie.Value);

            if (decryptedCookie == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<LoginResponse>(decryptedCookie.UserData);
        }
        public static string GetPercentage(decimal value)
        {
            var dataToReturn = value * 100;
            return Math.Round(dataToReturn, 0) + "%";
        }
        public static decimal GetDecimalPercentage(decimal value)
        {
            var dataToReturn = value * 100;
            return Math.Round(dataToReturn, 0);
        }
        public static DataTable GetDataTabletFromCSVFile(StreamReader _Stream, ExcelModel _Model)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(_Stream))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    if (!CheckCsvStructure(colFields, _Model))
                    {
                        return null;
                    }
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        int count = 0;
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            //count = 0;

                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                                count++;
                            }

                        }
                        if (fieldData.Length != count)
                        {
                            csvData.Rows.Add(fieldData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDataTabletFromCSVFile", "Utility.cs");
                throw;
            }
            LoginResponse _model = AppUtility.DecryptCookie();
            csvData.Columns.Add("Userid").Expression = _model.UserId.ToString();
            return csvData;
        }
        private static bool CheckCsvStructure(string[] ColumnHeaders, ExcelModel _Model)
        {
            try
            {
                //if (ColumnHeaders.Count() < 5)
                //{
                //    return false;
                //}

                Int32 valid = 0;
                Int32 Compare = 5;
                //foreach (var item in ColumnHeaders)
                //{ 
                //    if (_Model.GenerateDate.ToLower() == item.ToLower())
                //    { valid = valid + 1; }
                //    else if (_Model.Description.ToLower() == item.ToLower())
                //    { valid = valid + 1; }
                //    else if (_Model.Withdrawals.ToLower() == item.ToLower())
                //    { valid = valid + 1; }
                //    else if (_Model.Deposits.ToLower() == item.ToLower())
                //    { valid = valid + 1; }
                //    else if (_Model.ReserveBalance.ToLower() == item.ToLower())
                //    { valid = valid + 1; }
                //}
                //if(_Model.Deposits.ToLower() =="na")
                //{Compare = Compare-1; }
                //if (_Model.ReserveBalance.ToLower() == "na")
                //{Compare = Compare - 1; }

                if (Compare == valid)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CheckCsvStructure", "Utility.cs");
                throw;
            }
        }
        public static void LogMessage(Exception ex, String MethodName, String FileName)
        {
            try
            {
                string CurrentDate = DateTime.Now.ToString("yyyyMMdd");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"ErrorLog/" + CurrentDate + "_Error.txt";

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();

                }
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + Environment.NewLine +
                                     "Date :" + DateTime.Now.ToString() + Environment.NewLine +
                                     "Method Name :" + MethodName + Environment.NewLine +
                                     "File Name :" + FileName + Environment.NewLine
                       );
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    string OrganinationName = "Currently not implemented";
                    try
                    {
                        LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                        int _user = Convert.ToInt32(_Loginmodel.UserId);
                        EEONowEntity _context = new EEONowEntity();
                        var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                        if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                        {
                            int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                            OrganinationName = _context.Organizations.Where(e => e.OrganizationId == org).Select(e => e.Name).FirstOrDefault();
                        }
                        else
                        {
                            OrganinationName = user.UserRole.Organization.Name;
                        }
                    }
                    catch (Exception innerex)
                    {
                        OrganinationName = innerex.Message.ToString();
                    }
                    try
                    {
                        String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/ErrorMessageTemplate.html").ToString();
                        string ErrorHTML = String.Format(RawHtml, ex.Message, DateTime.Now.ToString(), MethodName, FileName, OrganinationName);
                        SendEmail(ConfigurationManager.AppSettings["ErrorMessageReceiver"], ErrorHTML, ConfigurationManager.AppSettings["ErrorMessageSubject"]);
                    }
                    catch
                    { }

                }
            }
            catch
            {

            }
        }
        public static void SendUploadEmployeeNotification(string FileName)
        {
            try
            {
                string UserEmail = "";
                string OrganinationName = "Currently not implemented";
                string UserName = "";
                string FileUploadDate = DateTime.Now.ToString("MM/dd/yyyy");
                try
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    EEONowEntity _context = new EEONowEntity();
                    var user = _context.Users.Where(e => e.UserId == _user).FirstOrDefault();
                    UserEmail = user.Email;

                    if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                    {
                        int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                        OrganinationName = _context.Organizations.Where(e => e.OrganizationId == org).Select(e => e.Name).FirstOrDefault();
                    }
                    else
                    {
                        OrganinationName = user.UserRole.Organization.Name;
                    }

                    UserName = user.FirstName + " " + user.LastName;
                }
                catch (Exception innerex)
                {
                    OrganinationName = innerex.Message.ToString();
                }
                String EmailList = ConfigurationManager.AppSettings["NotificationEmails"] + "," + UserEmail;
                String[] Emails = EmailList.Split(',');
                String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/UploadEmployeeNotificationTemplate.html").ToString();
                string HTML = String.Format(RawHtml, OrganinationName, UserName, FileName, FileUploadDate);
                SendEmailToMultipeRecipients(Emails, HTML, ConfigurationManager.AppSettings["UploadEmployeeNotificationSubject"]);
            }
            catch (Exception ex)
            {
                LogMessage(ex, "SendValidateEmployeeNotification", "Utility.cs");
            }
        }
        public static void SendValidateEmployeeNotification(string FileName)
        {
            try
            {
                string UserEmail = "";
                string OrganinationName = "Currently not implemented";
                string UserName = "";
                string FileValidatedDate = DateTime.Now.ToString("MM/dd/yyyy");
                try
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    EEONowEntity _context = new EEONowEntity();
                    var user = _context.Users.Where(e => e.UserId == _user).FirstOrDefault();
                    UserEmail = user.Email;
                    if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                    {
                        int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                        OrganinationName = _context.Organizations.Where(e => e.OrganizationId == org).Select(e => e.Name).FirstOrDefault();
                    }
                    else
                    {
                        OrganinationName = user.UserRole.Organization.Name;
                    }
                    UserName = user.FirstName + " " + user.LastName;
                }
                catch (Exception innerex)
                {
                    OrganinationName = innerex.Message.ToString();
                }
                String EmailList = ConfigurationManager.AppSettings["NotificationEmails"] + "," + UserEmail;
                String[] Emails = EmailList.Split(',');
                String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/ValidateEmployeeNotificationTemplate.html").ToString();
                string HTML = String.Format(RawHtml, OrganinationName, UserName, FileName, FileValidatedDate);
                SendEmailToMultipeRecipients(Emails, HTML, ConfigurationManager.AppSettings["ValidateEmployeeNotificationSubject"]);
            }
            catch (Exception ex)
            {
                LogMessage(ex, "SendValidateEmployeeNotification", "Utility.cs");
            }
        }
        public static void SendFileAvailableNotification(int OrganizationId, string FileSubmissionDate, Int32 UserID)
        {
            try
            {
                string[] OrganizationUser;
                try
                {
                    EEONowEntity _context = new EEONowEntity();

                    OrganizationUser = _context.Users.Where(e => e.UserId == UserID).Select(e => e.Email).ToArray();

                    string userEmailThatCreatedFile = string.Empty;

                    foreach (var item in OrganizationUser)
                    {
                        userEmailThatCreatedFile = item.ToString();
                    }

                    String EmailList = ConfigurationManager.AppSettings["NotificationEmails"] + "," + userEmailThatCreatedFile.ToString();
                    String[] Emails = EmailList.Split(',');

                    String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/FileAvailableNotificationTemplate.html").ToString();
                    string HTML = String.Format(RawHtml, FileSubmissionDate);
                    SendEmailToMultipeRecipients(Emails, HTML, String.Format(ConfigurationManager.AppSettings["FileAvailableNotificationSubject"], FileSubmissionDate));
                }
                catch (Exception innerex)
                {
                    LogMessage(innerex, "SendFileAvailableNotification", "Utility.cs");
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex, "SendFileAvailableNotification", "Utility.cs");
            }
        }
        public static bool ValidateLoginSession()
        {
            try
            {
                var httpCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (httpCookie == null)
                {
                    return false;
                }
                //Here throws error!
                var decryptedCookie = FormsAuthentication.Decrypt(httpCookie.Value);
                if (decryptedCookie == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {

                return false;
            }


        }
        public static string EncryptURl(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string DecryptUrl(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
