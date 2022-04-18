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
    [RoutePrefix("PositionYearsOfService")]
    [Authorize()]
    public class PositionYearsOfServiceController : Controller
    {

        IPositionYearsOfService _PositionYearsOfServiceService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public PositionYearsOfServiceController()
        {

            _PositionYearsOfServiceService = new PositionYearsOfServiceService();
            _OrganizationsService = new OrganizationService();
            _AccountService = new AccountService();
        }
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    var actionName = requestContext.RouteData.Values["action"];
        //}
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                
                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();
                //ViewBag.RoleList = await _userRoleService.BindUserRoleDropDown();
                
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindPositionYearsOfServiceModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _PositionYearsOfServiceService.GetPositionYearsOfServiceModel();
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
        public async Task<ActionResult> CreatePositionYearsOfService([DataSourceRequest] DataSourceRequest request, PositionYearsOfServiceModel _PositionYearsModel)
        {
            try
            {
                if (_PositionYearsModel != null && ModelState.IsValid)
                {
                    var model = await _PositionYearsOfServiceService.CreatePositionYearsOfService(_PositionYearsModel);
                    _PositionYearsModel.PositionYearsOfServiceId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _PositionYearsModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdatePositionYearsOfService([DataSourceRequest] DataSourceRequest request, PositionYearsOfServiceModel _PositionYearsModel)
        {
            try
            {
                if (_PositionYearsModel != null && ModelState.IsValid)
                {
                    var model = await _PositionYearsOfServiceService.UpdatePositionYearsOfService(_PositionYearsModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _PositionYearsModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}