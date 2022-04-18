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
    [RoutePrefix("OrganizationLabelField")]
    [Authorize()]
    public class OrganizationLabelFieldController : Controller
    {
        IOrganizationLabelFieldService _OrganizationLabelFieldService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public OrganizationLabelFieldController()
        {

            _OrganizationLabelFieldService = new OrganizationLabelFieldService();
            _OrganizationsService = new OrganizationService();
            _AccountService = new AccountService();
        } 
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
        public async Task<ActionResult> BindOrganizationLabelFieldModel([DataSourceRequest] DataSourceRequest request, int? organization, int? roleid)
        {
            try
            {
                List<OrganizationLabelFieldModel> empltyModel = new List<OrganizationLabelFieldModel>();
                if(roleid==null && roleid==0)
                {
                    return Json(empltyModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                var model = await _OrganizationLabelFieldService.GetOrganizationLabelFieldModel(organization, roleid);
                var OrganizationId = _AccountService.GetOrganisationID();
                if (OrganizationId > 0)
                {
                    model = model.Where(e => e.OrganizationId == OrganizationId).ToList();
                }
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateOrganizationLabelField([DataSourceRequest] DataSourceRequest request, OrganizationLabelFieldModel _OrganizationLabelFieldModel)
        {
            try
            {
                if (_OrganizationLabelFieldModel != null && ModelState.IsValid)
                {
                    var model = await _OrganizationLabelFieldService.UpdateOrganizationLabelField(_OrganizationLabelFieldModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _OrganizationLabelFieldModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}