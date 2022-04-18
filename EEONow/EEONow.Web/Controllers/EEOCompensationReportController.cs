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
    [RoutePrefix("EEOCompensationReport")]
    [Authorize()]

    public class EEOCompensationReportController : Controller
    {
        // GET: Country
        IEEOCompensationReportService _EEOCompensationReportService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        public EEOCompensationReportController()
        {
            _EEOCompensationReportService = new EEOCompensationReportService();
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
                var model = _EEOCompensationReportService.BindEEOJobCategoryDropDown(organization);
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
                var model = _EEOCompensationReportService.BindEEOProgramOfficeDropDown(organization.Value, filesubmission.Value);
                return Json(model.Select(p => new { EEOProgramOfficeId = p.Value, EEOProgramOfficeName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEEOPositionTitle(int? organization, int? filesubmission, int? eeojobcategory, string eeoprogramoffice,string region)
        {
            try
            {
                if (eeojobcategory == null)
                {
                    eeojobcategory = 0;
                }
                var model = _EEOCompensationReportService.BindEEOPositionTitleDropDown(organization.Value, filesubmission.Value, eeojobcategory.Value, eeoprogramoffice, region);
                return Json(model.Select(p => new { EEOPositionTitleId = p.Value, EEOPositionTitleName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }              
        public ActionResult GetEEOCompensationReport(int? organization, int? filesubmission, int? eeojobcategory, int jobtitlesortorder, string eeoprogramoffice, string eeopositiontitle, string region)
        {
            try
            {
                // int organisastionId = AccountService.GetDashboardOrganisationID();
                // int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                var modelEEOCompensationReport = _EEOCompensationReportService.GetEEOCompensationReportService(organization, filesubmission, eeojobcategory, jobtitlesortorder, eeoprogramoffice, eeopositiontitle, region);
                return PartialView("~/Views/EEOCompensationReport/Partials/EEOCompensationReport.cshtml", modelEEOCompensationReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportEEOReport(int? organization, int? filesubmission, int? eeojobcategory, int jobtitlesortorder, string eeoprogramoffice, string eeopositiontitle, string region)
        {
            try
            {
                var modelEEOCompensationReport = _EEOCompensationReportService.GetEEOCompensationExportService(organization, filesubmission, eeojobcategory, jobtitlesortorder, eeoprogramoffice, eeopositiontitle, region);
                return View(modelEEOCompensationReport); 
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
       
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int? organization, int? filesubmission, int? eeojobcategory, int jobtitlesortorder, string eeoprogramoffice, string eeopositiontitle, string region)
        {
            eeoprogramoffice = HttpUtility.UrlEncode(eeoprogramoffice);
            eeopositiontitle = HttpUtility.UrlEncode(eeopositiontitle);
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
 
            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/EEOCompensationReport/ExportEEOReport?organization={0}&filesubmission={1}&eeojobcategory={2}&jobtitlesortorder={3}&eeoprogramoffice={4}&eeopositiontitle={5}&region={6}", organization, filesubmission, eeojobcategory, jobtitlesortorder, eeoprogramoffice, eeopositiontitle, region);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }
        private string RenderViewAsString(string viewName, EEOCompensationReportModel model)
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


//public ActionResult ExportEEOReport(int? organization, int? filesubmission, int? eeojobcategory, int jobtitlesortorder, string eeoprogramoffice, string eeopositiontitle)
//{
//    try
//    {
//        //int organisastionId = AccountService.GetDashboardOrganisationID();
//        //int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
//        var modelEEOCompensationReport = _EEOCompensationReportService.GetEEOCompensationReportService(organization, filesubmission, eeojobcategory, jobtitlesortorder, eeoprogramoffice, eeopositiontitle);

//        return View(modelEEOCompensationReport);
//        //string htmlString = RenderViewAsString("~/Views/EEOCompensationReport/Partials/EEOCompensationReportPDF.cshtml", modelEEOCompensationReport);
//        //htmlString = htmlString.Replace("Export to PDF", "");
//        //string pdf_page_size = "A4";
//        //string baseUrl = "";
//        //PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
//        //    pdf_page_size, true);
//        //string pdf_orientation = "Landscape";
//        //PdfPageOrientation pdfOrientation =
//        //    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
//        //    pdf_orientation, true);
//        //int webPageWidth = 1400;
//        //int webPageHeight = 900;
//        //// instantiate a html to pdf converter object
//        //HtmlToPdf converter = new HtmlToPdf();
//        //// set converter options
//        //converter.Options.PdfPageSize = pageSize;
//        //converter.Options.PdfPageOrientation = pdfOrientation;
//        //converter.Options.WebPageWidth = webPageWidth;
//        //converter.Options.WebPageHeight = webPageHeight;
//        //converter.Options.MarginTop = 10;
//        //converter.Options.MarginLeft = 10;
//        //converter.Options.MarginRight = 10;
//        //// create a new pdf document converting an url
//        //PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);
//        //var Response = System.Web.HttpContext.Current.Response;
//        //// save pdf document
//        //doc.Save(Response, false, "EEOReport.pdf");
//        //// close pdf document
//        //doc.Close();
//        //return Json("success", JsonRequestBehavior.AllowGet);
//    }
//    catch
//    {
//        return RedirectToAction("Errorwindow", "Home");
//    }
//}