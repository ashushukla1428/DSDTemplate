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
    [RoutePrefix("EEORating")]
    [Authorize()]
    public class EEORatingController : Controller
    {

        IEEORating _EEORatingService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public EEORatingController()
        {

            _EEORatingService = new EEORatingService();
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
                if (AccountService.CheckOrganisationAssigned() == 0)
                {
                    return RedirectToAction("ErrorScreen", "Account");
                }
                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();
                ViewBag.EEORatingTypeList = await _EEORatingService.BindEEORatingTypeDropDown();
                //ViewBag.RoleList = await _userRoleService.BindUserRoleDropDown();
                ViewBag.IsFilter = _AccountService.ActiveOrganisationFilter();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindEEORatingModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _EEORatingService.GetEEORatingModel();
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
        public async Task<ActionResult> CreateEEORating([DataSourceRequest] DataSourceRequest request, EEORatingModel _EEORatingModel)
        {
            try
            {
                if (_EEORatingModel != null && ModelState.IsValid)
                {
                    var model = await _EEORatingService.CreateEEORating(_EEORatingModel);
                    _EEORatingModel.EEORatingId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _EEORatingModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateEEORating([DataSourceRequest] DataSourceRequest request, EEORatingModel _EEORatingModel)
        {
            try
            {
                if (_EEORatingModel != null && ModelState.IsValid)
                {
                    var model = await _EEORatingService.UpdateEEORating(_EEORatingModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _EEORatingModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}