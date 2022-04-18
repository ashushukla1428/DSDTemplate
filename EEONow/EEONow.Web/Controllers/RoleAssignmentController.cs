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
    [RoutePrefix("RoleAssignment")]
    [Authorize()]
    public class RoleAssignmentController : Controller
    {

        IAssignRoleService _AssignRoleService;
        IUserRolesService _userRoleService;
        IOrganizationsService _OrganizationsService;
        public RoleAssignmentController()
        {

            _AssignRoleService = new AssignRoleService();
            _userRoleService = new UserRoleService();
            _OrganizationsService = new OrganizationService();
        }
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    var actionName = requestContext.RouteData.Values["action"];
        //}
        // GET: RoleAssignment
        //[CustomAuthorizeFilter]
        //public async Task<ActionResult> Index()
        //{
        //    try
        //    {

        //        ViewBag.MenuList = await _AssignRoleService.BindMenuDropDown();
        //        // ViewBag.RoleList = await _userRoleService.BindUserRoleDropDown();
        //        return View();
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Errorwindow", "Home");
        //    }
        //}
        //public async Task<ActionResult> BindRoleAssignmentModel([DataSourceRequest] DataSourceRequest request)
        //{
        //    try
        //    {
        //        var model = await _AssignRoleService.GetAssignRoleModel();
        //        return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Errorwindow", "Home");
        //    }
        //}
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateAssignRole([DataSourceRequest] DataSourceRequest request, AssignRoleModel _AssignRole)
        //{
        //    try
        //    {
        //        if (_AssignRole != null && ModelState.IsValid)
        //        {
        //            var model = _AssignRoleService.UpdateAssignRole(_AssignRole);
        //            if (!model.Succeeded)
        //            {
        //                ModelState.AddModelError("Error Message", model.Message);
        //            }
        //        }
        //        return Json(new[] { _AssignRole }.ToDataSourceResult(request, ModelState));
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Errorwindow", "Home");
        //    }
        //}

        //  [CustomAuthorizeFilter]
        public async Task<ActionResult> ManageUserRole()
        {
            try
            {
                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();
                ViewBag.RoleList = await _userRoleService.BindUserRoleDropDown(null);
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindManageUserRoleModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _AssignRoleService.GetManageUserModel();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateManageUserRole([DataSourceRequest] DataSourceRequest request, ManageUserRoleModel _ManageUserRole)
        {
            try
            {
                if (_ManageUserRole != null && ModelState.IsValid)
                {
                    var model = await _AssignRoleService.CreateUserAccount(_ManageUserRole);
                    _ManageUserRole.UserId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _ManageUserRole }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateManageUserRole([DataSourceRequest] DataSourceRequest request, ManageUserRoleModel _ManageUserRole)
        {
            try
            {
                if (_ManageUserRole != null && ModelState.IsValid)
                {
                    var model = await _AssignRoleService.UpdateUserRole(_ManageUserRole);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _ManageUserRole }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<JsonResult> ReSendEmail(string Email)
        {
            try
            {
                var result = await _AssignRoleService.ReSendPasswordEmail(Email);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}