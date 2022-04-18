using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class ErrorHandleController : Controller
    {
        //
        // GET: /ErrorHandle/

        public ActionResult ErrorList()
        {
            return View();
        }


        [HttpPost]
        public JsonResult getErrorTrackList(SearchFilters model)
        {
            //try
            //{

                //int i = Convert.ToInt32("ds");


                dbWempeEntities db = new dbWempeEntities();

                var _items = db.Database.SqlQuery<ErrorTrackModel>("USP_GetErrorTrack @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID, model.UserType);

                return Json(_items, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(ex.Message);
            //}
        }

    }
}
