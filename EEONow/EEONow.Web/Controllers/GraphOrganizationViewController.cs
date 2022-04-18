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
    [RoutePrefix("GraphOrganizationView")]
    [Authorize()]
    public class GraphOrganizationViewController : Controller
    {
        IGraphOrganizationViewService _GraphOrganizationViewService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        IUserRolesService _UserRolesService;
        public GraphOrganizationViewController()
        {

            _GraphOrganizationViewService = new GraphOrganizationViewService();
            _OrganizationsService = new OrganizationService();
            _AccountService = new AccountService();
            _UserRolesService = new UserRoleService();
        } 
        [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            try
            {
               
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindGraphOrganizationViewModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _GraphOrganizationViewService.GetGraphOrganizationViewModel();
                var OrganisationId = _AccountService.GetOrganisationID();
                if (OrganisationId > 0)
                {
                    model = model.Where(e => e.OrganizationId == OrganisationId).ToList();
                }
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        } 
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateGraphOrganizationView([DataSourceRequest] DataSourceRequest request, GraphOrganizationViewModel _GraphOrganizationViewModel)
        {
            try
            {
                if (_GraphOrganizationViewModel != null && ModelState.IsValid)
                {
                    var model = await _GraphOrganizationViewService.UpdateGraphOrganizationView(_GraphOrganizationViewModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _GraphOrganizationViewModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        
        // Assign Color selector to levels
        public async Task<ActionResult> AssignGraphOrganizationView()
        {
            try
            {
                ViewBag.GraphOrganizationViewList = await _GraphOrganizationViewService.BindGraphOrganizationViewDropDown();
              
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindColorAssignmentModel([DataSourceRequest] DataSourceRequest request, int? organization, int? roleid)
        {
            try
            {
           //     if (organization != null)
                {
                    var model = await _GraphOrganizationViewService.GetAssignedGraphOrganizationViewModel(organization, roleid);
                    //if (organization == 0)
                    //{
                    //    var OrganisationId = _AccountService.GetOrganisationID();
                    //    if (OrganisationId > 0)
                    //    {
                    //        model = model.Where(e => e.OrganizationId == OrganisationId).ToList();
                    //    }
                    //}
                    //else
                    //{
                    //    model = model.Where(e => e.OrganizationId == organization).ToList();
                    //}
                    return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }

                //return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateColorAssignmentSelector([DataSourceRequest] DataSourceRequest request, AssigGraphOrganizationViewModel _AssignedGraphOrganizationView)
        {
            try
            {
                if (_AssignedGraphOrganizationView != null && ModelState.IsValid)
                {
                    var model = _GraphOrganizationViewService.UpdateAssignedGraphOrganizationView(_AssignedGraphOrganizationView);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _AssignedGraphOrganizationView }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetGraphOrganizationViewViaOrganisation(int organization, int roleid)
        {
            try
            {
                var model = _GraphOrganizationViewService.BindGraphOrganizationViewViaOrganisationIdDropDown(organization);

                return Json(model.Select(p => new { Value = p.Value, Text = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<ActionResult> GetOrganizationsList()
        {
            try
            {
                var model = await _OrganizationsService.BindOrganizationDropDown();
                return Json(model.Select(p => new { OrganizationId = p.Value, OrganizationName = p.Text }), JsonRequestBehavior.AllowGet);
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
                var model = await _UserRolesService.BindUserRoleDropDown(organization,true);
                return Json(model.Select(p => new { RoleId = p.Value, RoleName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}