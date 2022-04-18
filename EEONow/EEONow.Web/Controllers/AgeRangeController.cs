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
    [RoutePrefix("AgeRange")]
    [Authorize()]
    public class AgeRangeController : Controller
    {

        IAgeRange _AgeRangeService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public AgeRangeController()
        {

            _AgeRangeService = new AgeRangeService();
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
        public async Task<ActionResult> BindAgeRangeModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _AgeRangeService.GetAgeRangeModel();
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
        public async Task<ActionResult> CreateAgeRange([DataSourceRequest] DataSourceRequest request, AgeRangeModel _AgeRangeModel)
        {
            try
            {
                if (_AgeRangeModel != null && ModelState.IsValid)
                {
                    var model = await _AgeRangeService.CreateAgeRange(_AgeRangeModel);
                    _AgeRangeModel.AgeRangeId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _AgeRangeModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateAgeRange([DataSourceRequest] DataSourceRequest request, AgeRangeModel _AgeRangeModel)
        {
            try
            {
                if (_AgeRangeModel != null && ModelState.IsValid)
                {
                    var model = await _AgeRangeService.UpdateAgeRange(_AgeRangeModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _AgeRangeModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}