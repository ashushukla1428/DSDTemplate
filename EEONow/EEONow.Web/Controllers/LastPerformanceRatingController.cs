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
    [RoutePrefix("LastPerformanceRating")]
    [Authorize()]
    public class LastPerformanceRatingController : Controller
    {

        ILastPerformanceRating _LastPerformanceRatingService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public LastPerformanceRatingController()
        {

            _LastPerformanceRatingService = new LastPerformanceRatingService();
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
        public async Task<ActionResult> BindLastPerformanceRatingModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _LastPerformanceRatingService.GetLastPerformanceRatingModel();
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
        public async Task<ActionResult> CreateLastPerformanceRating([DataSourceRequest] DataSourceRequest request, LastPerformanceRatingModel _LastPerformanceRatingModel)
        {
            try
            {
                if (_LastPerformanceRatingModel != null && ModelState.IsValid)
                {
                    var model = await _LastPerformanceRatingService.CreateLastPerformanceRating(_LastPerformanceRatingModel);
                    _LastPerformanceRatingModel.LastPerformanceRatingId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _LastPerformanceRatingModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateLastPerformanceRating([DataSourceRequest] DataSourceRequest request, LastPerformanceRatingModel _LastPerformanceRatingModel)
        {
            try
            {
                if (_LastPerformanceRatingModel != null && ModelState.IsValid)
                {
                    var model = await _LastPerformanceRatingService.UpdateLastPerformanceRating(_LastPerformanceRatingModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _LastPerformanceRatingModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}