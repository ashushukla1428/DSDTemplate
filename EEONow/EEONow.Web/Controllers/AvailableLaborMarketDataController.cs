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
using System.Web.UI;
using SelectPdf;
using EEONow.Utilities;

namespace EEONow.Web.Controllers
{
    [RoutePrefix("AvailableLaborMarketData")]
    [Authorize()]
    public class AvailableLaborMarketDataController : Controller
    {
        IAvailableLaborMarketService _availableLaborMarketService;
        public AvailableLaborMarketDataController()
        {
            _availableLaborMarketService = new AvailableLaborMarketService();
        }
        [CustomAuthorizeFilter]

        public ActionResult Index(int? AvailableLaborMarketFileVersionId)
        {
            try
            {
              
                ViewBag.CompleteStatus = -1;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    if (AvailableLaborMarketFileVersionId != null && AvailableLaborMarketFileVersionId > 0)
                    {
                        var model = _availableLaborMarketService.GetAvailableLaborMarketDataViaFileVersion(AvailableLaborMarketFileVersionId.Value);
                        return View(model);
                    }
                    else
                    {
                        var model = _availableLaborMarketService.GetAvailableLaborMarketData();
                        return View(model);
                    }
                }
                else
                {
                    if (AppUtility.DecryptCookie().Roles != "DefinedSoftwareAdministrator")
                    {
                        if (AvailableLaborMarketFileVersionId != null && AvailableLaborMarketFileVersionId > 0)
                        {
                            var model = _availableLaborMarketService.GetAvailableLaborMarketDataViaFileVersion(AvailableLaborMarketFileVersionId.Value);
                            return View(model);
                        }
                        else
                        {
                            var model = _availableLaborMarketService.GetAvailableLaborMarketData();
                            return View(model);
                        }
                    }

                }
                AvailableLaborMarketFileVersionModel _Emptymodel = new AvailableLaborMarketFileVersionModel();
                return View(_Emptymodel); 
                 
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public ActionResult Index(AvailableLaborMarketFileVersionModel _AvailableLaborMarketFileVersion)
        {
            try
            {
                var Result = _availableLaborMarketService.SaveAvailableLaborMarketData(_AvailableLaborMarketFileVersion);
                ViewBag.CompleteStatus = Result.Id;
                var model = _availableLaborMarketService.GetAvailableLaborMarketData();
                return View(model);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult AddNewLaborMarketDataAction()
        {
            try
            {
                var model = _availableLaborMarketService.AddNewLaborMarketData();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult ExportPdfLaborMarketData(int AvailableLaborMarketFileVersionId)
        {
            try
            {
                var _LaborMarketData = _availableLaborMarketService.GetAvailableLaborMarketDataViaFileVersion(AvailableLaborMarketFileVersionId);
                var ByteData = _availableLaborMarketService.PDFAvailableLaborMarketData(_LaborMarketData);

                string htmlString = ByteData;
                string baseUrl = "";

                string pdf_page_size = "A4";
                PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                    pdf_page_size, true);

                string pdf_orientation = "Portrait";
                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    pdf_orientation, true);

                int webPageWidth = 1024;
                int webPageHeight = 0;
                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = webPageHeight;
                converter.Options.MarginBottom = 30;
                converter.Options.MarginTop = 20;
                converter.Options.MarginLeft = 10;

                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);
                var Response = System.Web.HttpContext.Current.Response;
                // save pdf document
                doc.Save(Response, false, "LaborMarketData.pdf");
                // close pdf document
                doc.Close();


                return Json("Suceess", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult MarkasCurrentLaborMarketData(int AvailableLaborMarketFileVersionId)
        {
            try
            {
                _availableLaborMarketService.MarkasCurrentLaborMarketData(AvailableLaborMarketFileVersionId);
                return RedirectToAction("Index", "AvailableLaborMarketData");
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetFileVersionMarketData()
        {
            try
            {
                var model = _availableLaborMarketService.GetFileVersionMarketData();
                return Json(model.Select(p => new { AvailableLaborMarketFileVersionId = p.Value, FileVersionName = p.Text }), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

    }
}