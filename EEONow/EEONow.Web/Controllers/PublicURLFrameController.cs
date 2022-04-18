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
namespace EEONow.Web.Controllers
{
    [RoutePrefix("PublicURLFrame")]
    [Authorize()]
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class PublicURLFrameController : Controller
    {
        IPublicURL _PublicURL;
        public PublicURLFrameController()
        {
            _PublicURL = new PublicURLService();
        }


        [CustomAuthorizeFilter]
        // GET: PublicURLFrame
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BindPublicURLFrameModel([DataSourceRequest] DataSourceRequest request)
        {

            try
            {
                var model = _PublicURL.GetPublicURL();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        //public ActionResult GeneratePublicURLFrame()
        //{
        //    return View();
        //}

        public JsonResult ReGenerateKey(int OrgainisationId)
        {
            try
            {
                var result = _PublicURL.ReGenerateKey(OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ActivateUrl(int OrgainisationId)
        {
            try
            {
                var result = _PublicURL.ActivateUrl(OrgainisationId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}