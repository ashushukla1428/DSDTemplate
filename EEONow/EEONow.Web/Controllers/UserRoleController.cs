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

namespace EEONow.Web.Controllers
{
    [RoutePrefix("UserRole")]
    [Authorize()]
    // [Authorize(Roles = "admin,user")]
    public class UserRoleController : Controller
    {
        // GET: UserRole
        IUserRolesService _stateService;
        IOrganizationsService _OrganizationsService;
        public UserRoleController()
        {
            _stateService = new UserRoleService();
            _OrganizationsService = new OrganizationService();
        }
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindUserRoleModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _stateService.GetUserRoles();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateUserRole([DataSourceRequest] DataSourceRequest request, UserRoleModel _UserRole)
        {
            try
            {
                if (_UserRole != null && ModelState.IsValid)
                {
                    var model = await _stateService.CreateUserRole(_UserRole);
                    _UserRole.RoleId = model.Id;

                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _UserRole }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateUserRole([DataSourceRequest] DataSourceRequest request, UserRoleModel _UserRole)
        {
            try
            {
                if (_UserRole != null && ModelState.IsValid)
                {
                    var model = await _stateService.UpdateUserRole(_UserRole);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _UserRole }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, UserRoleModel _UserRole)
        //{
        //    if (_UserRole != null)
        //    {
        //        //productService.Destroy(product);
        //    }

        //    return Json(new[] { _UserRole }.ToDataSourceResult(request, ModelState));
        //}
    }
}