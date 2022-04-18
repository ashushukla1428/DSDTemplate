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
    [RoutePrefix("MenuHeader")]
    //  [Authorize()]

    public class MenuHeaderController : Controller
    {
        // GET: MenuHeader
        //IMenuHeaderAssignmentService _MenuHeaderAssignmentService;
        IOrganizationsService _organizationService;
        IMenuHeaderService _MenuHeaderService;
        IUserRolesService _UserRolesService;
        public MenuHeaderController()
        {
          //  _MenuHeaderAssignmentService = new MenuHeaderAssignmentService();
            _MenuHeaderService = new MenuHeaderService();
            _UserRolesService = new UserRoleService();
            _organizationService = new OrganizationService();
        }
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            ViewBag.MenuList = await _MenuHeaderService.BindMenuDropDown();
            ViewBag.RoleList = await _UserRolesService.BindUserRoleDropDown(null);
            ViewBag.OrganisationList = await _organizationService.BindOrganizationDropDown();
            return View();
        }
        public async Task<ActionResult> BindMenuHeaderModel([DataSourceRequest] DataSourceRequest request, HeaderMenuSearchModel _HeaderMenuSearchModel)
        {
            try
            {
                var model = await _MenuHeaderService.GetMenuHeader(_HeaderMenuSearchModel);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateMenuHeader([DataSourceRequest] DataSourceRequest request, MenuHeaderModel _MenuHeader)
        {
            try
            {
                if (_MenuHeader != null && ModelState.IsValid)
                {
                    if (_MenuHeader.IsHeader)
                    {
                        if (_MenuHeader.ListMenu == null || _MenuHeader.ListMenu.Count() == 0)
                        {
                            ModelState.AddModelError("Error Message", "Please select at least one Menu item");
                            return Json(new[] { _MenuHeader }.ToDataSourceResult(request, ModelState));
                        }
                    }

                    var model = await _MenuHeaderService.CreateMenuHeader(_MenuHeader);
                    _MenuHeader.MenuHeaderID_PK = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }

                return Json(new[] { _MenuHeader }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateMenuHeader([DataSourceRequest] DataSourceRequest request, MenuHeaderModel _MenuHeader)
        {
            try
            {
                if (_MenuHeader != null && ModelState.IsValid)
                {
                    if (_MenuHeader.IsHeader)
                    {
                        if (_MenuHeader.ListMenu == null || _MenuHeader.ListMenu.Count() == 0)
                        {
                            ModelState.AddModelError("Error Message", "Please select atleast one Menu item");
                            return Json(new[] { _MenuHeader }.ToDataSourceResult(request, ModelState));
                        }
                    }

                    var model = await _MenuHeaderService.UpdateMenuHeader(_MenuHeader);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _MenuHeader }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<ActionResult> GetRolesList(int? organization)
        {
            try
            {
                var model = await _UserRolesService.BindUserRoleDropDown(organization);
                return Json(model.Select(p => new { RoleId = p.Value, RoleName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}