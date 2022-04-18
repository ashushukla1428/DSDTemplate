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
using Kendo.Mvc;
using EEONow.Utilities;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("EEOJobCategory")]
    [Authorize()]
    public class EEOJobCategoryController : Controller
    {

        IEEOJobCategoryService _EEOJobCategoryService;
        IOrganizationsService _OrganizationsService;
        IAccountService _AccountService;
        public EEOJobCategoryController()
        {

            _EEOJobCategoryService = new EEOJobCategoryService();
            _OrganizationsService = new OrganizationService();
            _AccountService = new AccountService();
        }

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
        public async Task<ActionResult> BindEEOJobCategoryModel([DataSourceRequest] DataSourceRequest request)
        {
            //request.Filters.Add(new FilterDescriptor()
            //{
            //    Member = "OrganizationId",
            //    MemberType = typeof(string),
            //    Operator = FilterOperator.Contains,
            //    Value = 1
            //});
            try
            {
                var model = await _EEOJobCategoryService.GetEEOJobCategoryModel();
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
        public async Task<ActionResult> CreateEEOJobCategory([DataSourceRequest] DataSourceRequest request, EEOJobCategoriesModel _EEOJobCategoriesModel)
        {
            try
            {
                if (_EEOJobCategoriesModel != null && ModelState.IsValid)
                {
                    var model = await _EEOJobCategoryService.CreateEEOJobCategory(_EEOJobCategoriesModel);
                    _EEOJobCategoriesModel.EEOJobCategoryId = model.Id;
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _EEOJobCategoriesModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateEEOJobCategory([DataSourceRequest] DataSourceRequest request, EEOJobCategoriesModel _EEOJobCategoriesModel)
        {
            try
            {
                if (_EEOJobCategoriesModel != null && ModelState.IsValid)
                {
                    var model = await _EEOJobCategoryService.UpdateEEOJobCategory(_EEOJobCategoriesModel);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);
                    }
                }
                return Json(new[] { _EEOJobCategoriesModel }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}
