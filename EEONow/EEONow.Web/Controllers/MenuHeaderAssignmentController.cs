using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web.Routing;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("MenuHeaderAssignment")]
   // [Authorize()]
    public class MenuHeaderAssignmentController : Controller
    {

        IMenuHeaderAssignmentService _MenuHeaderAssignmentService;
        IUserRolesService _userRoleService;
        public MenuHeaderAssignmentController()
        {

            _MenuHeaderAssignmentService = new MenuHeaderAssignmentService();
            _userRoleService = new UserRoleService();
        }
       [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.MenuList = await _MenuHeaderAssignmentService.BindMenuDropDown();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindMenuHeaderAssignmentModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _MenuHeaderAssignmentService.GetMenuHeaderAssignmentModel();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateMenuHeaderAssignment([DataSourceRequest] DataSourceRequest request, MenuHeaderAssignmentModel _MenuHeaderAssignment)
        {
            try
            {
                if (_MenuHeaderAssignment != null && ModelState.IsValid)
                {
                    var model = _MenuHeaderAssignmentService.UpdateMenuHeaderAssignment(_MenuHeaderAssignment);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _MenuHeaderAssignment }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}