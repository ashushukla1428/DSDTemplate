using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    [CustomAuthorize()]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search()
        {
            SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "--Select---" };
            //
            ViewBag.NameTitles = new SelectList(db.wmpTitles.Where(c => c.IsActive == true ).Select(c => new { c.ID, c.title }), "Id", "title");
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true ).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            return View();
        }

        public JsonResult searchCustomer(SearchCustomerFilters model)
        {
            try
            {
                model.sortColumn = "firstName";
                model.sortOrder = "desc";
                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomerOnAllFields @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.SearchFields,model.SearchValues, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

    }
}
