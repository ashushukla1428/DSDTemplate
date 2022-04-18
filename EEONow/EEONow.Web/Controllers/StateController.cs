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
    [RoutePrefix("State")]
    [Authorize()]

    public class StateController : Controller
    {
        // GET: State
        IStatesService _stateService;
        public StateController()
        {
            _stateService = new StateService();
        }

        [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            //var model = await _stateService.GetStates(); 
            return View();
        }
        public async Task<ActionResult> BindStateModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _stateService.GetStates();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<ActionResult> BindStateModelDDL([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _stateService.GetStates();
                return Json(model.Select(e => e.Name).Distinct(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateState([DataSourceRequest] DataSourceRequest request, StateModel _State)
        {
            try
            {
                if (_State != null && ModelState.IsValid)
                {
                    var model = await _stateService.CreateState(_State);
                    _State.StateId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _State }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateState([DataSourceRequest] DataSourceRequest request, StateModel _State)
        {
            try
            {
                if (_State != null && ModelState.IsValid)
                {
                    var model = await _stateService.UpdateState(_State);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _State }.ToDataSourceResult(request, ModelState));
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