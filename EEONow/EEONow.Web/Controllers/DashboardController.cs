using EEONow.Services;
using System;
using System.Web.Mvc;
using EEONow.Interfaces;
using EEONow.Utilities;
using System.IO;
using EEONow.Models;
using HiQPdf;
using System.Configuration;
using System.Web;
using System.Threading.Tasks;
using System.Linq;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("Dashboard")]
    [Authorize()]
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]

    public class DashboardController : Controller
    {
        IDashboard _Dashboard;
        IALMViaRacesEeoService _ALMViaRacesEeoService;
        IGraphOrganizationViewService _GraphOrganizationViewService;
        IOrganizationsService _OrganizationsService;
        public DashboardController()
        {
            _Dashboard = new DashboardService();
            _ALMViaRacesEeoService = new ALMViaRacesEeoService();
            //_EEORating = new EEORatingService();
            _GraphOrganizationViewService = new GraphOrganizationViewService();
            _OrganizationsService = new OrganizationService();
        }
        //GET: Dashboard
        [CustomAuthorizeFilter]
        public ActionResult Index()
        {
            try
            {
                bool _result = AccountService.CheckIsGraphRender();
                if (!_result)
                {
                    return RedirectToAction("NotAssignedGraphAndOrganization", "Home");
                }

                int _organization = 0;
                int _RoleId = 0;

                ViewBag.configuredOrganization = 1;

                if (AppUtility.GetOrgIdForAdminView().Length > 0 && AppUtility.GetRoleIdForAdminView().Length > 0)
                {
                    _organization = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _RoleId = Convert.ToInt32(AppUtility.GetRoleIdForAdminView());
                    var ListOfGraphOrganizationView = _GraphOrganizationViewService.GraphOrganizationViewList(_organization, _RoleId);
                    ViewBag.ListOfGraphOrganizationView = ListOfGraphOrganizationView;
                    ViewBag.organisastionId = AccountService.GetDashboardOrganisationID();

                    if (ListOfGraphOrganizationView.Length > 0)
                    {
                        ViewBag.configuredOrganization = 0;
                    }
                }
                else
                {
                    if (AppUtility.DecryptCookie().Roles != "DefinedSoftwareAdministrator")
                    { 
                        ViewBag.organisastionId = AccountService.GetDashboardOrganisationID();
                         
                        var ListOfGraphOrganizationView = _GraphOrganizationViewService.GraphOrganizationViewList(0,0);
                        ViewBag.ListOfGraphOrganizationView = ListOfGraphOrganizationView;
                        if (ListOfGraphOrganizationView.Length > 0)
                        {
                            ViewBag.configuredOrganization = 0;
                        }
                    }
                        
                }
                ViewBag.EEORating = 0.00;// _EEORating.GetBenchMarkValue(0);
                
                return View();
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
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
        public ActionResult GetCurrentUserFiles()
        {
            try
            {
                var result = _Dashboard.GetdbCurrentUserFile();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetCurrentOrganisationColorCode()
        {
            try
            {
                var result = _Dashboard.GetdbCurrentOrganisationColorCode();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public ActionResult GetDynamicColorChanger(String Selectedvalue)
        {
            try
            {
                var result = _Dashboard.GetdbDynamicColorChanger(Selectedvalue);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public ActionResult GetCsvFileList(string year)
        {
            try
            {
                if(Convert.ToInt32(year)==0)
                {
                    year= _Dashboard.GetdbCurrentSelectedYear();
                }
                var result = _Dashboard.GetdbCsvFileList(year);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetSelectedYearList()
        {
            try
            {
                var result = _Dashboard.GetdbSelectedYearList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetCurrentSelectedYear()
        {
            try
            {
                var result = _Dashboard.GetdbCurrentSelectedYear();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch             {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult getLegendCollections()
        {
            try
            {
                string filename = _Dashboard.GetdbCurrentUserFile();
                var result = _Dashboard.GetLegendCollections(filename);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        //[Route("Dashboard/GetLegendList/{Type}")]
        public ActionResult GetLegendList(string Type, string filename)
        {
            try
            {
                if (filename.Trim().Length == 0)
                {
                    filename = _Dashboard.GetdbCurrentUserFile();
                }
                var result = _Dashboard.GetDBLegendList(Type, filename);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        public ActionResult GetUserLabel(int organisastionId)
        {
            try
            {
                var _GetUserLabel = _Dashboard.GetUserLabel(organisastionId);
                return Json(_GetUserLabel, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }
        [AllowAnonymous()]
        public ActionResult AvailableLaborMarketPDF(int organisastionId, string filename,string empPosition="")
        {
            try
            {
                int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                //var modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketService(organisastionId, FileSubmissionID);
                ALMViaRacesEeoModel modelALM = new ALMViaRacesEeoModel();
                if (string.IsNullOrEmpty(empPosition))
                {
                    modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketService(organisastionId, FileSubmissionID);
                }
                else
                {
                    modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketReportByEmpId(organisastionId, FileSubmissionID, empPosition);
                }
                return View(modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [AllowAnonymous()]
        public ActionResult ExportToPDF(int organization, string filename,string empPosition="")
        {
            filename = HttpUtility.UrlEncode(filename);
            empPosition = HttpUtility.UrlEncode(empPosition);
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

            string url = string.Format(ConfigurationManager.AppSettings["AppUrl"] + "/Dashboard/AvailableLaborMarketPDF?organisastionId={0}&filename={1}&empPosition={2}", organization.ToString(), filename, empPosition);

            byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(url);

            // send the PDF document to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "EEOReport_" + DateTime.Now.Date.ToString("MM_dd_yyyy") + ".pdf";

            return fileResult;
        }
        public ActionResult GetAvailableLaborMarket(int organization,string filename,string empPosition)
        {
            try
            {
                //int organisastionId = AccountService.GetDashboardOrganisationID();
                int FileSubmissionID = AccountService.GetFileSubmissionID(organization, filename);
                ALMViaRacesEeoModel modelALM = new ALMViaRacesEeoModel();
                if (string.IsNullOrEmpty(empPosition))
                {
                    modelALM= _ALMViaRacesEeoService.GetAvailableLaborMarketService(organization, FileSubmissionID);
                }
                else {
                    modelALM = _ALMViaRacesEeoService.GetAvailableLaborMarketReportByEmpId(organization, FileSubmissionID, empPosition);
                }
                
                
                return PartialView("~/Views/Dashboard/Partials/AvailableLaborMarket.cshtml", modelALM);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetIndexReport(string filename, string position)
        {
            try
            {
                int organisastionId = AccountService.GetDashboardOrganisationID();
                int FileSubmissionID = AccountService.GetFileSubmissionID(organisastionId, filename);
                var modelIndexReport = _ALMViaRacesEeoService.GetIndexReport(organisastionId, FileSubmissionID, position);
                return PartialView("~/Views/Dashboard/Partials/ViewIndexReport.cshtml", modelIndexReport);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        private string RenderViewAsString(string viewName, ALMViaRacesEeoModel model)
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
        public ActionResult Sample()
        {
            try
            {
                var model = _Dashboard.BindOrhChartDashborad();
                return View(model);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult LiveOrgChart()
        {
            try
            {
                ViewBag.configuredOrganization = 1;
                var model = _Dashboard.BindOrhChartDashborad();
                if(model.OrganizationId==0)
                {
                  ViewBag.configuredOrganization = 0;                     
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetFileSubmissionDetail(int FileSubmissionId)
        {
            try
            {
                var model = _Dashboard.GetFileSubmissionDetail(FileSubmissionId);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public JsonResult BindDashboardForAdmin(int orgId)
        {
            try
            {
                AppUtility.SetOrgIdForAdminView(orgId);
                return Json("Sucess", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BindDashboardRoleForAdmin(int roleId)
        {
            try
            {
                AppUtility.SetRoleIdForAdminView(roleId);
                return Json("Sucess", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}