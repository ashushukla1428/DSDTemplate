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
    public class ManageRightsController : Controller
    {
        //
        // GET: /ManageRights/
        dbWempeEntities db = new dbWempeEntities();
        
        public ActionResult Index()
        {
            var _pageData = db.wmpRoleMasters.Where(c => c.IsActive == true && c.OwnerID==SessionMaster.Current.OwnerID).Select(c => new { c.RoleID, c.Role });
            SelectList list = new SelectList(_pageData, "RoleID", "Role");
            ViewBag.Roles = list.OrderBy(s => s.Text);
            return View();
        }
        [HttpPost]
        public JsonResult getList(ManageRightsFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<AuthenticationRightsModel>("USP_GetAuthenticationRights @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.Name == null ? "" : model.Name, model.RoleID, model.pageNo, 500, model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
      

    }
}
