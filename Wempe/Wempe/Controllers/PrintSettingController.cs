using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
   // [CustomAuthorize()]
    public class PrintSettingController : Controller
    {
        //
        // GET: /PrintSetting/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            wmpPrintSetting model = db.wmpPrintSettings.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).FirstOrDefault();
            if (model == null) {
                model = new wmpPrintSetting();
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Add(wmpPrintSetting model)
        {
            try
            {
                wmpPrintSetting _model = db.wmpPrintSettings.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).FirstOrDefault();
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.IsActive = true;
                if (_model != null)
                {
                    model.SettingID = _model.SettingID;
                    db.Dispose();
                    db = new dbWempeEntities();
                }
                if (ModelState.IsValid)
                {
                    if (model.SettingID == 0)
                    {
                       
                        model.OwnerID = SessionMaster.Current.OwnerID;
                        db.wmpPrintSettings.Add(model);
                    }
                    else
                    {
                      
                        model.OwnerID = SessionMaster.Current.OwnerID;
                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
