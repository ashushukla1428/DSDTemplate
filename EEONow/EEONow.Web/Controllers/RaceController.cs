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
    [RoutePrefix("Race")]
    [Authorize()]
    public class RaceController : Controller
    {
        IRaceService _RaceService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public RaceController()
        {

            _RaceService = new RaceService();
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
                
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindRaceModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _RaceService.GetRaceModel();
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
        public async Task<ActionResult> CreateRace([DataSourceRequest] DataSourceRequest request, RaceModel _RaceModel)
        {
            try
            {
                if (_RaceModel != null && ModelState.IsValid)
                {
                    var model = await _RaceService.CreateRace(_RaceModel);
                    _RaceModel.RaceId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _RaceModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateRace([DataSourceRequest] DataSourceRequest request, RaceModel _RaceModel)
        {
            try
            {
                if (_RaceModel != null && ModelState.IsValid)
                {
                    var model = await _RaceService.UpdateRace(_RaceModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _RaceModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}