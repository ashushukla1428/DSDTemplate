using EEONow.Utilities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EEONow.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Errorwindow()
        {
            AppUtility.ClearDataFromCookie("TempOrgId");
            AppUtility.ClearDataFromCookie("TempRoleId");
            return RedirectToAction("NotAssignedOrganization", "Home");

        }
        public ActionResult NotAssignedOrganization()
        {
            return View();
        }
        public ActionResult NotAssignedGraphAndOrganization()
        {
            return View();
        }
        public ActionResult ValidateLoginSession()
        {
            try
            {
                var result = AppUtility.ValidateLoginSession();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LevelUserSession()
        {
            try
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }

    }


}