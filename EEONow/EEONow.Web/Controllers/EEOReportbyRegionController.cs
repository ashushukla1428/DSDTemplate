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
    [RoutePrefix("EEOReportbyRegion")]
    [Authorize()]

    public class EEOReportbyRegionController : Controller
    {
        // GET: Country
        IEEOReportbyRegionService _EEOReportbyRegionService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        public EEOReportbyRegionController()
        {
            _EEOReportbyRegionService = new EEOReportbyRegionService();
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
        public ActionResult GetEEORegion(int? organization, int? filesubmission)
        {
            try
            {
                var model = _EEOReportbyRegionService.BindEmployeeRegionDropDown(organization.Value, filesubmission.Value);
                return Json(model.Select(p => new { RegionId = p.Value, RegionName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEEOReportbyRegion(int? organization, int? filesubmission, string region)
        {
            try
            { 
                var modelEEOReportbyRegion = _EEOReportbyRegionService.GetEEOReportbyRegionService(organization, filesubmission, region);
                modelEEOReportbyRegion.RegionName = region.Length > 0 ? region.ToUpper() : "ALL";
                return PartialView("~/Views/EEOReportbyRegion/Partials/EEOReportbyRegion.cshtml", modelEEOReportbyRegion);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportEEOReport(int? organization, int? filesubmission, string region)
        {
            try
            { 
                var modelEEOReportbyRegion = _EEOReportbyRegionService.GetEEOExportbyRegionService(organization, filesubmission, region);
                modelEEOReportbyRegion.RegionName = region.Length > 0 ? region.ToUpper() : "ALL";
                return View(modelEEOReportbyRegion); 
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }       
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int? organization, int? filesubmission, string region)
        {
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
 
            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/EEOReportbyRegion/ExportEEOReport?organization={0}&filesubmission={1}&region={2}", organization, filesubmission, region);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReportByRegion_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }    
    }
}


