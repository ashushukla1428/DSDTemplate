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
    [RoutePrefix("Country")]
    [Authorize()]

    public class CountyController : Controller
    {
        // GET: Country
        ICountiesService _countryService;
        public CountyController()
        {
            _countryService = new CountyService();
        }
        [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            //var model = await _stateService.GetStates(); 
            return View();
        }
        public async Task<ActionResult> BindCountyModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _countryService.GetCounties();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateCounty([DataSourceRequest] DataSourceRequest request, CountyModel _Country)
        {
            try
            {
                if (_Country != null && ModelState.IsValid)
                {
                    var model = await _countryService.CreateCounty(_Country);
                    _Country.CountyId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _Country }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateCounty([DataSourceRequest] DataSourceRequest request, CountyModel _Country)
        {
            try
            {
                if (_Country != null && ModelState.IsValid)
                {
                    var model = await _countryService.UpdateCounty(_Country);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _Country }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, StateModel _State)
        //{
        //    if (_State != null)
        //    {
        //        //productService.Destroy(product);
        //    }

        //    return Json(new[] { _State }.ToDataSourceResult(request, ModelState));
        //}
    }
}