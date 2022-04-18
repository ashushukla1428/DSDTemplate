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
    [RoutePrefix("ALMByFederalJobCodesReport")]
    [Authorize()]

    public class ALMByFederalJobCodesReportController : Controller
    {
        // GET: Country
        IALMByFederalJobCodesReportService _ALMByFederalJobCodesReportService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        public ALMByFederalJobCodesReportController()
        {
            _ALMByFederalJobCodesReportService = new ALMByFederalJobCodesReportService();
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
                var model = _ALMByFederalJobCodesReportService.GetUSCensusDataVersionDropDown();
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
                var model = _ALMByFederalJobCodesReportService.GetStateDropDown();
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
                var model = _ALMByFederalJobCodesReportService.GetUSCensusOccupationsDropDown();
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
                var model = _ALMByFederalJobCodesReportService.GetUSCensusGeographyTypesDropDown();
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
                var model = _ALMByFederalJobCodesReportService.GetPUMACodesDropDown(stateid, uscensusgeographytypesid);
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
                var model = _ALMByFederalJobCodesReportService.GetEmploymentStatusDropDown(uscensusgeographytypesid);
                return Json(model.Select(p => new { EmploymentStatusId = p.Value, EmploymentStatusName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        public ActionResult GetALMByFederalJobCodesReport(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs)
        {
            try
            {
                var modelALMByFederalJobCodesReport = _ALMByFederalJobCodesReportService.GetALMByFederalJobCodesReportService(uSCensusVersionID, stateID, eEOOccupationCodeID, eEOCategoryCodeNbr, searchByWorkSite, eSRCodes, pUMA_CODES, majorOccupationGroup, minorOccupationGroupID, boardingOccupationGroupID, occupationIDs);
                return PartialView("~/Views/ALMByFederalJobCodesReport/Partials/ALMByFederalJobCodesReport.cshtml", modelALMByFederalJobCodesReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportALMByFederalJobCodesReport(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs)
        {
            try
            {
                var modelALMByFederalJobCodesReport = _ALMByFederalJobCodesReportService.GetALMByFederalJobCodesExportService(uSCensusVersionID, stateID, eEOOccupationCodeID, eEOCategoryCodeNbr, searchByWorkSite, eSRCodes, pUMA_CODES, majorOccupationGroup, minorOccupationGroupID, boardingOccupationGroupID, occupationIDs);
                return View(modelALMByFederalJobCodesReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs)
        {
            eSRCodes = HttpUtility.UrlEncode(eSRCodes);
            occupationIDs = HttpUtility.UrlEncode(occupationIDs);
            pUMA_CODES = HttpUtility.UrlEncode(pUMA_CODES);
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
            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/ALMByFederalJobCodesReport/ExportALMByFederalJobCodesReport?uSCensusVersionID={0}&stateID={1}&eEOOccupationCodeID={2}&eEOCategoryCodeNbr={3}&searchByWorkSite={4}&eSRCodes={5}&pUMA_CODES={6}&majorOccupationGroup={7}&minorOccupationGroupID={8}&boardingOccupationGroupID={9}&occupationIDs={10}", uSCensusVersionID, stateID, eEOOccupationCodeID, eEOCategoryCodeNbr, searchByWorkSite, eSRCodes, pUMA_CODES, majorOccupationGroup, minorOccupationGroupID, boardingOccupationGroupID, occupationIDs);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }
        private string RenderViewAsString(string viewName, ALMByFederalJobCodesReportModel model)
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




        public ActionResult GetOccupations(int BoardingId)
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetOccupations(BoardingId);
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetBoardingOccupationalGroup(int MinorId)
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetBoardingOccupationalGroup(MinorId);
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetMinorOccupationalGroup(int MajorId)
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetMinorOccupationalGroup(MajorId);
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetMajorOccupationalGroup()
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetMajorOccupationalGroup();
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetEEOCategories(int CensusOccupationsId)
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetEEOCategories(CensusOccupationsId);
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }


        public ActionResult GetOccupationswithEEO(int EEOCategoriesId)
        {
            try
            {
                var model = _ALMByFederalJobCodesReportService.GetOccupationswithEEo(EEOCategoriesId);
                return Json(model.Select(p => new { DisplayValue = p.Value, ValueName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}
