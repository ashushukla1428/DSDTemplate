using System;
using System.Linq;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class SuggestionController : Controller
    {
        public ActionResult SuggestionList()
        {
            return View();
        }
        public ActionResult MySuggestions()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getSuggestionList(SearchFilters model)
        {
            dbWempeEntities db = new dbWempeEntities();
            var _items = db.Database.SqlQuery<SuggestionModel>("USP_GetSuggestionList @p0, @p1, @p2, @p3, @p4,@p5", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
            return Json(_items, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getSuggestionListUserWise(SearchFilters model)
        {
            dbWempeEntities db = new dbWempeEntities();
            var _items = db.Database.SqlQuery<SuggestionModel>("USP_GetSuggestionListUserWise @p0, @p1, @p2, @p3, @p4,@p5,@p6",SessionMaster.Current.LoginId, model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
            return Json(_items, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSuggestion(SuggestionModel model)
        {
            dbWempeEntities db = new dbWempeEntities();
            if (model.SuggestionId == 0)
            {
                wmpSuggestion obj = new wmpSuggestion { Suggestion = model.Suggestion, TimeStamp = DateTime.Now, UserId = SessionMaster.Current.LoginId };
                db.wmpSuggestions.Add(obj);
                db.SaveChanges();
            }
            else
            {
                wmpSuggestion obj = db.wmpSuggestions.Where(S => S.SuggestionId == model.SuggestionId).FirstOrDefault();
                obj.Suggestion = model.Suggestion;
                db.SaveChanges();
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditSuggestion(SuggestionModel model)
        {
            dbWempeEntities db = new dbWempeEntities();
            wmpSuggestion obj = db.wmpSuggestions.Where(S => S.SuggestionId == model.SuggestionId).FirstOrDefault();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
