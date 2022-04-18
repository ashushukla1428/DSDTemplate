using EEONow.Interface;
using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Services;
using EEONow.Utilities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("EmployeeSubmission")]
    [Authorize()]
    public class EmployeeSubmissionController : Controller
    {
        IEmployeeService _EmployeeService;
        IOrganizationsService _OrganizationsService;
        ICountiesService _CountiesService;
        IEEOJobCategoryService _EEOJobCategoryService;
        IRaceService _RaceService;
        IAgencyYearsOfService _AgencyYearsOfService;
        IStatesService _StateService;
        IUploadService _UploadService;

        public EmployeeSubmissionController()
        {
            _EmployeeService = new EmployeeService();
            _OrganizationsService = new OrganizationService();
            _CountiesService = new CountyService();
            _EEOJobCategoryService = new EEOJobCategoryService();
            _RaceService = new RaceService();
            _AgencyYearsOfService = new AgencyYearsOfServiceService();
            _StateService = new StateService();
            _UploadService = new UploadService();
        }
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            try
            { 
                
                ViewBag.OrganisationList = await _OrganizationsService.BindOrganizationDropDown();
                ViewBag.GenderList = new List<SelectListItem>() {
                new SelectListItem(){ Text="Male", Value="1"},
                new SelectListItem(){ Text="Female", Value="2"}
            };

                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
                int? organizationID = _Loginmodel.OrgId;

                ViewBag.RaceList = await _RaceService.BindRaceDropDown(organizationID);
                ViewBag.EEOCodeList = await _EEOJobCategoryService.BindEEOJobCategoryDropDown(organizationID);
                ViewBag.AgencyYearsOfServiceList = await _AgencyYearsOfService.BindAgencyYearsOfServiceDropDown(organizationID);
                ViewBag.StateList = await _StateService.BindStateDropDown();
                ViewBag.CountyList = await _CountiesService.BindCountyDropDown();
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult SubmitEmployeeCSV(IEnumerable<HttpPostedFileBase> files,DateTime SubmissionDateTime)
        {
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fileName = _UploadService.GetFileName();
                        var physicalPath = Path.Combine(Server.MapPath("~/EmployeeCSV"), fileName);
                        file.SaveAs(physicalPath);
                        var result = _UploadService.CsvUploading(physicalPath, file.FileName, fileName, SubmissionDateTime);

                        if (result.Id > 0)
                        {
                            AppUtility.SendUploadEmployeeNotification(file.FileName);
                            // Return an empty string to signify success
                            return Content("");
                        }
                        else
                        {
                            return Content(result.Message);
                        }
                    }
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult BindFileSubmissionRecordModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = _UploadService.GetFileSubmissionRecords();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> DeleteFileSubmissionRecords()
        {
            try
            {
                var model = await _UploadService.DeleteFileSubmissionRecords();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult BindNotValidateFileSubmissionRecordModel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = _UploadService.GetNotValidateFileSubmissionRecords();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult ValidateFileSubmissionRecords()
        {
            try
            {
                var model = _UploadService.ValidateEmployeeRecords();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> UpdateEmployeeSubmission([DataSourceRequest] DataSourceRequest request, ValidateEmployeeRecords _EmployeeRecords)
        {
            try
            {
                if (_EmployeeRecords != null && ModelState.IsValid)
                {
                    var model = await _UploadService.UpdateFileSubmission(_EmployeeRecords);
                    if (!model.Succeeded)
                    {
                        ModelState.AddModelError("Error Message", model.Message);

                    }
                }
                return Json(new[] { _EmployeeRecords }.ToDataSourceResult(request, ModelState));
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult BindToolbarData()
        {
            try
            {
                var _model = _UploadService.BindFileSubmissionToolbar();
                return Json(_model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult BindEmployeeSubmissionErrorList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = _UploadService.BindEmployeeSubmissionErrorList();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            try
            {
                var fileContents = Convert.FromBase64String(base64);

                return File(fileContents, contentType, fileName);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}