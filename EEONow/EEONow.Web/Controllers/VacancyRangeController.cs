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
    [RoutePrefix("VacancyRange")]
   // [Authorize()]
    public class VacancyRangeController : Controller
    {

        IVacancyRange _VacancyRangeService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public VacancyRangeController()
        {

            _VacancyRangeService = new VacancyRangeService();
            _OrganizationsService = new OrganizationService();
            _AccountService = new AccountService();
        }
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    var actionName = requestContext.RouteData.Values["action"];
        //}
       // [CustomAuthorizeFilter]
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
        public async Task<ActionResult> BindVacancyRangeModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _VacancyRangeService.GetVacancyRangeModel();
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
        public async Task<ActionResult> CreateVacancyRange([DataSourceRequest] DataSourceRequest request, VacancyRangeModel _VacancyRangeModel)
        {
            try
            {
                if (_VacancyRangeModel != null && ModelState.IsValid)
                {
                    var model = await _VacancyRangeService.CreateVacancyRange(_VacancyRangeModel);
                    _VacancyRangeModel.VacancyRangeId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _VacancyRangeModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateVacancyRange([DataSourceRequest] DataSourceRequest request, VacancyRangeModel _VacancyRangeModel)
        {
            try
            {
                if (_VacancyRangeModel != null && ModelState.IsValid)
                {
                    var model = await _VacancyRangeService.UpdateVacancyRange(_VacancyRangeModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _VacancyRangeModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}