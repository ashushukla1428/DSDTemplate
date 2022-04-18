using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class Get_Repair_StatusController : Controller
    {
        public ActionResult GetRepairStatusLog()
        {
            return View();
        }

     


        [HttpPost]
        public JsonResult getRepairStatusLogs(SearchFilters model)
        {
            dbWempeEntities db = new dbWempeEntities();
            var _items = db.Database.SqlQuery<GetRepairStatusLogModel>("USP_GetRepairStatusLog @p0, @p1, @p2, @p3, @p4,@p5", model.Name == null ? "" : model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
            return Json(_items, JsonRequestBehavior.AllowGet);
        }


      




    }
}
