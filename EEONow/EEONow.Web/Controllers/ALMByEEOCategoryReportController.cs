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
    [RoutePrefix("ALMByEEOCategoryReport")]
    [Authorize()]

    public class ALMByEEOCategoryReportController : Controller
    {
        // GET: Country
        IALMByEEOCategoryReportService _ALMByEEOCategoryReportService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        public ALMByEEOCategoryReportController()
        {
            _ALMByEEOCategoryReportService = new ALMByEEOCategoryReportService();
            _OrganizationsService = new OrganizationService();
            _EmployeeService = new EmployeeService();
        }
        [CustomAuthorizeFilter]
        public async Task<ActionResult> Index()
        {
            var _model = await _OrganizationsService.GetDefaultALMValue();
            return View(_model);
        }
        public ActionResult GetUSCensusDataVersionDropDown()
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetUSCensusDataVersionDropDown();
                return Json(model.Select(p => new { USCensusDataVersionId = p.Value, USCensusDataVersionName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetStateDropDown()
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetStateDropDown();
                return Json(model.Select(p => new { StateId = p.Value, StateName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public ActionResult GetUSCensusOccupationsDropDown()
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetUSCensusOccupationsDropDown();
                return Json(model.Select(p => new { USCensusOccupationsId = p.Value, USCensusOccupationsName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public ActionResult GetUSCensusGeographyTypesDropDown()
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetUSCensusGeographyTypesDropDown();
                return Json(model.Select(p => new { USCensusGeographyTypesId = p.Value, USCensusGeographyTypesName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public ActionResult GetPUMACodesDropDown(int? stateid, int? uscensusgeographytypesid)
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetPUMACodesDropDown(stateid, uscensusgeographytypesid);
                return Json(model.Select(p => new { PUMACodesId = p.Value, PUMACodesName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEmploymentStatusDropDown(int? uscensusgeographytypesid)
        {
            try
            {
                var model = _ALMByEEOCategoryReportService.GetEmploymentStatusDropDown(uscensusgeographytypesid);
                return Json(model.Select(p => new { EmploymentStatusId = p.Value, EmploymentStatusName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        public ActionResult GetALMByEEOCategoryReport(int? uscensusversionid, int? searchbyworksite, int? occupationcodeid, int? stateid, string puma_codes, string employmentstatus)
        {
            try
            {
                var modelALMByEEOCategoryReport = _ALMByEEOCategoryReportService.GetALMByEEOCategoryReportService(uscensusversionid, searchbyworksite == 1 ? true : false, occupationcodeid, stateid, puma_codes, employmentstatus);
                return PartialView("~/Views/ALMByEEOCategoryReport/Partials/ALMByEEOCategoryReport.cshtml", modelALMByEEOCategoryReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportALMByEEOCategoryReport(int? uscensusversionid, int? searchbyworksite, int? occupationcodeid, int? stateid, string puma_codes, string employmentstatus)
        {
            try
            {
                var modelALMByEEOCategoryReport = _ALMByEEOCategoryReportService.GetALMByEEOCategoryExportService(uscensusversionid, searchbyworksite == 1 ? true : false, occupationcodeid, stateid, puma_codes, employmentstatus);
                return View(modelALMByEEOCategoryReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int? uscensusversionid, int? searchbyworksite, int? occupationcodeid, int stateID, string puma_codes, string employmentstatus)
        {
            puma_codes = HttpUtility.UrlEncode(puma_codes);
            employmentstatus = HttpUtility.UrlEncode(employmentstatus);
           
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
            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/ALMByEEOCategoryReport/ExportALMByEEOCategoryReport?uscensusversionid={0}&searchbyworksite={1}&occupationcodeid={2}&stateID={3}&puma_codes={4}&employmentstatus={5}", uscensusversionid, searchbyworksite, occupationcodeid, stateID, puma_codes, employmentstatus);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }
        private string RenderViewAsString(string viewName, ALMByEEOCategoryReportModel model)
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
