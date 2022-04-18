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
    [RoutePrefix("Organization")]
    [Authorize()]
    public class OrganizationController : Controller
    {
        IALMByEEOCategoryReportService _ALMByEEOCategoryReportService;
        IOrganizationsService _organizationService;
        IAccountService _AccountService;
        IStatesService _stateService;
        public OrganizationController()
        {
            _organizationService = new OrganizationService();
            _stateService = new StateService();
            _AccountService = new AccountService();
            _ALMByEEOCategoryReportService = new ALMByEEOCategoryReportService();
        }
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.StateList = await _stateService.BindStateDropDown();
                ViewBag.DefaultStateList = await _stateService.BindStateDropDown();
                ViewBag.ParentOrganizationList = await _organizationService.BindParentOrganizationDropDown();
                
                ViewBag.USCensusOccupationsList = _ALMByEEOCategoryReportService.GetUSCensusOccupationsDropDown();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindOrganizationModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _organizationService.GetOrganization();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateOrganization([DataSourceRequest] DataSourceRequest request, OrganizationModel _Organization)
        {
            try
            {
                if (_Organization != null && ModelState.IsValid)
                {
                    var model = await _organizationService.CreateOrganization(_Organization);
                    _Organization.OrganizationId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _Organization }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateOrganization([DataSourceRequest] DataSourceRequest request, OrganizationModel _Organization)
        {
            try
            {
                if (_Organization != null && ModelState.IsValid)
                {
                    var model = await _organizationService.UpdateOrganization(_Organization);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _Organization }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> VacancyPositionColor()
        {
            try
            {
                ViewBag.OrganisationList = await _organizationService.BindOrganizationDropDown();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindVacancyPositionColor([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _organizationService.GetVacancyPositionColor();
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
        public async Task<ActionResult> UpdateVacancyPositionColor([DataSourceRequest] DataSourceRequest request, VacancyPositionColorModel _VacancyPositionColor)
        {
            try
            {
                if (_VacancyPositionColor != null && ModelState.IsValid)
                {
                    var model = await _organizationService.UpdateVacancyPositionColor(_VacancyPositionColor);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _VacancyPositionColor }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

    }
}