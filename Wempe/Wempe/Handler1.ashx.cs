using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Wempe.CommonClasses;

namespace Wempe
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler, IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            dbWempeEntities _db = new dbWempeEntities();
            try
            {
                context.Response.ContentType = "text/plain";
                HttpPostedFile uploadFiles = context.Request.Files["Filedata"];
                int repairNumber = Convert.ToInt32(context.Request["repairNumber"]);
               
                var TempFetchRecord = _db.wmpRepairs.Where(x => x.repairNumber == repairNumber);
                string FileNameFull = "C" + TempFetchRecord.FirstOrDefault().customerNumber + "_R" + TempFetchRecord.FirstOrDefault().repairNumber + "_" + DateTime.Now.Ticks + "_" + uploadFiles.FileName;
                string FilePathToStore = +TempFetchRecord.FirstOrDefault().OwnerID + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore));
                if (!exists)
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore));

                string pathToSave = HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore + "/") + FileNameFull;
                uploadFiles.SaveAs(pathToSave);

                wmpFileAttachment obj = new wmpFileAttachment
                {
                    repairNumber = repairNumber,
                    dateStamp = DateTime.Now.ToString(),
                    fileName = FileNameFull,
                    fullPath = FilePathToStore + "/" + FileNameFull,
                    OwnerID = SessionMaster.Current.OwnerID,
                    UpdateBy = SessionMaster.Current.LoginId
                };
                _db.wmpFileAttachments.Add(obj);
                _db.SaveChanges();
                wmpRepairLog log = new wmpRepairLog();
                log.changeComment = "Attached file (" + obj.fileName + ") to repair.";
                log.repairNumber = Convert.ToInt32(repairNumber);
                log.dateStamp = log.LastUpdate = DateTime.Now;

                log.OwnerID = SessionMaster.Current.OwnerID;
                log.UpdateBy = SessionMaster.Current.LoginId;

                _db.wmpRepairLogs.Add(log);
                _db.SaveChanges();
            }

            catch (Exception ex)
            {

                wmpRepairLog log = new wmpRepairLog();
                log.changeComment = ex.Message;
                log.repairNumber = 0;
                log.dateStamp = log.LastUpdate = DateTime.Now;

                log.OwnerID = SessionMaster.Current.OwnerID;
                log.UpdateBy = SessionMaster.Current.LoginId;

                _db.wmpRepairLogs.Add(log);
                _db.SaveChanges();
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}