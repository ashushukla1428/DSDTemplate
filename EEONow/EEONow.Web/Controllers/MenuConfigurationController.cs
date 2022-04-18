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
    [RoutePrefix("MenuConfiguration")]
   // [Authorize()]

    public class MenuConfigurationController : Controller
    {
        // GET: MenuConfiguration
        IMenuConfigurationService _MenuConfigurationService;
        public MenuConfigurationController()
        {
            _MenuConfigurationService = new MenuConfigurationService();
        }
       [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> BindMenuConfigurationModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _MenuConfigurationService.GetMenuConfiguration();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> CreateMenuConfiguration([DataSourceRequest] DataSourceRequest request, MenuConfigurationModel _MenuConfiguration)
        {
            try
            {
                if (_MenuConfiguration != null && ModelState.IsValid)
                {
                    var model = await _MenuConfigurationService.CreateMenuConfiguration(_MenuConfiguration);
                    _MenuConfiguration.MenuID_PK = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }

                return Json(new[] { _MenuConfiguration }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateMenuConfiguration([DataSourceRequest] DataSourceRequest request, MenuConfigurationModel _MenuConfiguration)
        {
            try
            {
                if (_MenuConfiguration != null && ModelState.IsValid)
                {
                    var model = await _MenuConfigurationService.UpdateMenuConfiguration(_MenuConfiguration);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _MenuConfiguration }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}