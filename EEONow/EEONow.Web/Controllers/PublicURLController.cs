using EEONow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EEONow.Interfaces;
using System.IO;
using EEONow.Models;
using HiQPdf;
using System.Configuration;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("PublicURL")]
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class PublicURLController : Controller
    {
        IPublicURL _PublicURL;
        IALMViaRacesEeoService _ALMViaRacesEeoService;
        //IEEORating _EEORating;
        IGraphOrganizationViewService _GraphOrganizationViewService;
       

        public PublicURLController()
        {
            _PublicURL = new PublicURLService();
            _ALMViaRacesEeoService = new ALMViaRacesEeoService();
            //_EEORating = new EEORatingService();
            _GraphOrganizationViewService = new GraphOrganizationViewService();
         
        }
        // GET: PublicURL
        public ActionResult Index(String token)
        {
            try
            {
                var result = _PublicURL.GetOrgainisationIdViaToken(token);
                if (result.FirstOrDefault().Key > 0)
                {
                    ViewBag.configuredOrganization = 1;
                    ViewBag.OrgainisationId = result.FirstOrDefault().Key;
                    ViewBag.RoleId = result.FirstOrDefault().Value;
                    ViewBag.EEORating = 0.00;// _EEORating.GetBenchMarkValue(result.FirstOrDefault().Key);
                    ViewBag.ListOfGraphOrganizationView = _GraphOrganizationViewService.GraphOrganizationViewList(result.FirstOrDefault().Key, result.FirstOrDefault().Value);
                    if (ViewBag.ListOfGraphOrganizationView.Length > 0)
                    {
                        ViewBag.configuredOrganization = 0;
                    }

                    if (result.FirstOrDefault().Value != 1)
                    {
                        int FileSubmissionID = AccountService.GetFileSubmissionID(result.FirstOrDefault().Key);
                        var modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketService(result.FirstOrDefault().Key, FileSubmissionID);
                        return View(modelALM);
                    }
                }
                else
                {
                    return RedirectToAction("ErrorScreen");
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
 
        public ActionResult GetCurrentUserFiles(Int32 OrgainisationId)
        {
            try
            {
                var result = _PublicURL.GetdbCurrentUserFile(OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetCurrentOrganisationColorCode(Int32 OrgainisationId, int RoleId)
        {
            try
            {
                
                var result = _PublicURL.GetdbCurrentOrganisationColorCode(OrgainisationId, RoleId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public ActionResult GetDynamicColorChanger(Int32 OrgainisationId, String Selectedvalue)
        {
            try
            {
                var result = _PublicURL.GetdbDynamicColorChanger(OrgainisationId, Selectedvalue);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public ActionResult GetCsvFileList(Int32 OrgainisationId, string year)
        {

            try
            {
                if (Convert.ToInt32(year) == 0)
                {
                    year = _PublicURL.GetdbCurrentSelectedYear(OrgainisationId);
                }
                var result = _PublicURL.GetdbCsvFileList(year, OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
             
        }

        //GetSelectedYearList
        public ActionResult GetSelectedYearList(Int32 OrgainisationId)
        {
            try
            {
                var result = _PublicURL.GetdbSelectedYearList(OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        //GetCurrentSelectedYear
        public ActionResult GetCurrentSelectedYear(Int32 OrgainisationId)
        {
            try
            {
                var result = _PublicURL.GetdbCurrentSelectedYear(OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult getLegendCollections(string OrgainisationId)
        {
            try
            {
                string filename = _PublicURL.GetdbCurrentUserFile(Convert.ToInt32(OrgainisationId));
                var result = _PublicURL.GetLegendCollections(Convert.ToInt32(OrgainisationId), filename);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetLegendList(string OrgainisationId, string Type, string filename)
        {
            try
            {
                if (filename.Trim().Length == 0)
                {
                    filename = _PublicURL.GetdbCurrentUserFile(Convert.ToInt32(OrgainisationId));
                }
                var result = _PublicURL.GetDBLegendList(Convert.ToInt32(OrgainisationId), Type, filename);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult ErrorScreen()
        {
            return View();
        }

        public ActionResult GetUserLabel(int organisastionId,int RoleId)
        {
            try
            {
                var _GetUserLabel = _PublicURL.GetUserLabel(organisastionId, RoleId);
                return Json(_GetUserLabel, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }

        //AvailableLaborMarketPDF
        [AllowAnonymous()]
        public ActionResult AvailableLaborMarketPDF(int organisastionId, string filename)
        {
            try
            {
                int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                var modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketService(organisastionId, FileSubmissionID);
                return View(modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        //ExportToPDF
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int organization, string filename)
        {
            filename = HttpUtility.UrlEncode(filename);
            
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
            htmlToPdfConverter.Document.Margins.Top = 20;
            // set whether to embed the true type font in PDF
            htmlToPdfConverter.Document.FontEmbedding = true;
            //htmlToPdfConverter.TriggerMode = ConversionTriggerMode.Auto;
            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.WaitTime;
            htmlToPdfConverter.WaitBeforeConvert = 5;
            // convert URL to a PDF memory buffer

            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/Dashboard/AvailableLaborMarketPDF?organisastionId={0}&filename={1}", organization.ToString(), filename);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }




        public ActionResult GetAvailableLaborMarket(Int32 organisastionId, string filename)
        {
            try
            { 
                int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                var modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketService(organisastionId, FileSubmissionID);
                return PartialView("~/Views/Dashboard/Partials/AvailableLaborMarket.cshtml", modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        } 
        public ActionResult GetIndexReport(int organisastionId,string filename, string position)
        {
            try
            { 
                int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                var modelIndexReport = _ALMViaRacesEeoService.GetIndexReport(organisastionId, FileSubmissionID, position);
                return PartialView("~/Views/Dashboard/Partials/ViewIndexReport.cshtml", modelIndexReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}