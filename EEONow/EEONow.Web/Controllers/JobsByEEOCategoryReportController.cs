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
using System.IO;
using HiQPdf;
using System.Configuration;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("JobsByEEOCategoryReport")]
   // [Authorize()]

    public class JobsByEEOCategoryReportController : Controller
    {
        // GET: Country
        IJobsByEEOCategoryReportService _JobsByEEOCategoryReportService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        public JobsByEEOCategoryReportController()
        {
            _JobsByEEOCategoryReportService = new JobsByEEOCategoryReportService();
            _OrganizationsService = new OrganizationService();
            _EmployeeService = new EmployeeService();
        }
        [CustomAuthorizeFilter]
        public ActionResult Index()
        { 
            return View();
        }
        public async Task<ActionResult> GetOrganizationsList()
        {
            try
            {
                var model = await _OrganizationsService.BindOrganizationDropDown();
                return Json(model.Select(p => new { OrganizationId = p.Value, OrganizationName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetFileViaOrganisation(int? organization)
        {
            try
            {
                var model = _EmployeeService.GetFileSubmissionViaOrganisation(organization);
                return Json(model.Select(p => new { FileSubmissionId = p.Value, FileName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEEOJobCategory(int? organization)
        {
            try
            {
                var model = _JobsByEEOCategoryReportService.BindEEOJobCategoryDropDown(organization);
                return Json(model.Select(p => new { EEOJobCategoryId = p.Value, EEOJobCategoryName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEEOProgramOffice(int? organization, int? filesubmission)
        {
            try
            {
                var model = _JobsByEEOCategoryReportService.BindEEOProgramOfficeDropDown(organization.Value, filesubmission.Value);
                return Json(model.Select(p => new { EEOProgramOfficeId = p.Value, EEOProgramOfficeName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        public ActionResult GetJobsByEEOCategoryReport(int? organization, int? filesubmission, int? eeojobcategory, string eeoprogramoffice, string region)
        {
            try
            {
                var modelJobsByEEOCategoryReport = _JobsByEEOCategoryReportService.GetJobsByEEOCategoryReportService(organization, filesubmission, eeojobcategory, eeoprogramoffice, region);
                return PartialView("~/Views/JobsByEEOCategoryReport/Partials/JobsByEEOCategoryReport.cshtml", modelJobsByEEOCategoryReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportJobsByEEOCategoryReport(int? organization, int? filesubmission, int? eeojobcategory, string eeoprogramoffice, string region)
        {
            try
            {
                var modelJobsByEEOCategoryReport = _JobsByEEOCategoryReportService.GetJobsByEEOCategoryExportService(organization, filesubmission, eeojobcategory, eeoprogramoffice, region);
                return View(modelJobsByEEOCategoryReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        [AllowAnonymous()]
        public ActionResult ExportToPDF(int? organization, int? filesubmission, int? eeojobcategory, string eeoprogramoffice, string region)
        {
            eeoprogramoffice = HttpUtility.UrlEncode(eeoprogramoffice);
            
            region = HttpUtility.UrlEncode(region);
            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            htmlToPdfConverter.BrowserWidth = 1800;
            htmlToPdfConverter.SerialNumber = ConfigurationManager.AppSettings["HiQPDFKey"].ToString();
            // set HTML Load timeout
            htmlToPdfConverter.HtmlLoadedTimeout = 120;
            // set PDF page size and orientation
            htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;
            htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Landscape;
            // set the PDF standard used by the document
            htmlToPdfConverter.Document.PdfStandard = PdfStandard.Pdf;
            // set PDF page margins
            htmlToPdfConverter.Document.Margins = new PdfMargins(0);
            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            //htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.WaitTime;
            htmlToPdfConverter.WaitBeforeConvert = 5;
            // convert URL to a PDF memory buffer

            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/JobsByEEOCategoryReport/ExportJobsByEEOCategoryReport?organization={0}&filesubmission={1}&eeojobcategory={2}&eeoprogramoffice={3}&region={4}", organization, filesubmission, eeojobcategory, eeoprogramoffice, region);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }
        private string RenderViewAsString(string viewName, JobsByEEOCategoryReportModel model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext,
                      viewName, null);
            // create a context to render a view based on a model
            ViewContext viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                new ViewDataDictionary(model),
                new TempDataDictionary(),
                stringWriter
            );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }


    }
}
