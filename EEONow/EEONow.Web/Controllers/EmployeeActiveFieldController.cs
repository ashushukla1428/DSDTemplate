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
    [RoutePrefix("EmployeeActiveField")]
    [Authorize()]
    public class EmployeeActiveFieldController : Controller
    {
        IEmployeeActiveFieldService _EmployeeActiveFieldService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public EmployeeActiveFieldController()
        {

            _EmployeeActiveFieldService = new EmployeeActiveFieldService();
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
        public async Task<ActionResult> BindEmployeeActiveFieldModel([DataSourceRequest] DataSourceRequest request, int? organization, int? roleid)
        {
            try
            {
                List<EmployeeActiveFieldModel> empltyModel = new List<EmployeeActiveFieldModel>();
                if(roleid==null && roleid==0)
                {
                    return Json(empltyModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                var model = await _EmployeeActiveFieldService.GetEmployeeActiveFieldModel(organization, roleid);
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
        public async Task<ActionResult> UpdateEmployeeActiveField([DataSourceRequest] DataSourceRequest request, EmployeeActiveFieldModel _EmployeeActiveFieldModel)
        {
            try
            {
                if (_EmployeeActiveFieldModel != null && ModelState.IsValid)
                {
                    var model = await _EmployeeActiveFieldService.UpdateEmployeeActiveField(_EmployeeActiveFieldModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _EmployeeActiveFieldModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}