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
    [RoutePrefix("Gender")]
    [Authorize()]

    public class GenderController : Controller
    {
        // GET: Gender
        IGendersService _GenderService;
        IAccountService _AccountService;
        public GenderController()
        {
            _GenderService = new GenderService();
            _AccountService = new AccountService();
        }

        [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            try
            {
                 
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> BindGenderModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _GenderService.GetGenders();
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
        public async Task<ActionResult> UpdateGender([DataSourceRequest] DataSourceRequest request, GenderModel _Gender)
        {
            try
            {
                if (_Gender != null && ModelState.IsValid)
                {
                    var model = await _GenderService.UpdateGender(_Gender);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _Gender }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


    }
}