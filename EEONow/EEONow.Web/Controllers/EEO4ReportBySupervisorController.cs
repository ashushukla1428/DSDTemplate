using EEONow.Interface.Interfaces;
using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Models.Models.EEO4Report;
using EEONow.Services;
using EEONow.Services.Services;
using EEONow.Utilities;
using HiQPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("EEO4ReportBySupervisor")]
    [Authorize()]
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class EEO4ReportBySupervisorController : Controller
    {
        IEEO4ReportBySupervisorService _EEO4ReportBySupervisorService;
        IOrganizationsService _OrganizationsService;
        IEmployeeService _EmployeeService;
        // GET: ReportBySupervisor
        public EEO4ReportBySupervisorController()
        {
            _EEO4ReportBySupervisorService = new EEO4ReportBySupervisorService();
            _OrganizationsService = new OrganizationService();
            _EmployeeService = new EmployeeService();
        }
       
        public ActionResult Index()
        {
          //  ViewBag.OrganisationId = AccountService.GetDashboardOrganisationID(); 
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
        public async Task<ActionResult> GetSupervisors(int? organization, int? filesubmission)
        {
            try
            {
                var model =await  _EmployeeService.GetSupervisorsViaOrganisation(organization.Value, filesubmission.Value);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
            //return null;
        }
        public ActionResult GetReportBySupervisor(int organization, int filesubmission, string empPosition = "")
        {
            try
            {
                if (organization == 0)
                {
                    organization = AccountService.GetDashboardOrganisationID();
                }

                //  int FileSubmissionID = filesubmission;// AccountService.GetFileSubmissionID(organisastionId, filesubmission);
                EEO4ReportViewModel modelALM = new EEO4ReportViewModel();
                modelALM = _EEO4ReportBySupervisorService.GetEEO4ReportBySupervisor(organization, filesubmission, empPosition);

                ViewBag.SalaryRange = _EEO4ReportBySupervisorService.GetEEO4ReportSalaryRangeList();
                return PartialView("~/Views/EEO4ReportBySupervisor/Partials/EEO4Report.cshtml", modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult EEO4ReportPDF(int organization, int filesubmission, string empPosition = "")
        {
            try
            {
                if (organization == 0)
                {
                    organization = AccountService.GetDashboardOrganisationID();
                }

                //  int FileSubmissionID = filesubmission;// AccountService.GetFileSubmissionID(organisastionId, filesubmission);
                EEO4ReportViewModel modelALM = new EEO4ReportViewModel();
                modelALM = _EEO4ReportBySupervisorService.GetEEO4ReportBySupervisor(organization, filesubmission, empPosition);

                ViewBag.SalaryRange = _EEO4ReportBySupervisorService.GetEEO4ReportSalaryRangeList();
                return View(modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int organization, int filesubmission, string empPosition = "",string supervisorName="")
        {
            empPosition = HttpUtility.UrlEncode(empPosition);
            supervisorName = HttpUtility.UrlEncode(supervisorName);
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

            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/EEO4ReportBySupervisor/EEO4ReportPDF?organization={0}&filesubmission={1}&empPosition={2}", organization.ToString(), filesubmission, empPosition);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            if (supervisorName == "--Select All--")
            {
                fileResult.FileDownloadName = "EEO4ReportBySupervisor_" + DateTime.Now.Date.ToString("MM_dd_yyyy")  + ".pdf";
            }
            else
            {
                fileResult.FileDownloadName = "EEO4ReportBySupervisor_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + "-" + supervisorName.Replace(" ","_") + ".pdf";
            }
            

            return fileResult;
        }


    }
}