using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wempe.Controllers
{
    public class ReportSummary
    {
        public Int32 NumberofRepairs { get; set; }
        public Int64 repairNumber { get; set; }
        public string repairNumberComplete { get; set; }
        public Int64 customerNumber { get; set; }
        public string Status { get; set; }
        public DateTime entryDate { get; set; }


        public long StatusId { get; set; }
    }
    public class PendingReportSummary
    {
        public string dueDate { get; set; }
        public Int64 repairNumber { get; set; }
        public string repairNumberComplete { get; set; }
        public Int64 customerNumber { get; set; }
        public string Status { get; set; }
    }
    public class GetToalAndPendingRepairList
    {
        public List<ReportSummary> TotalRepairs { get; set; }
        public List<PendingReportSummary> PendingRepairs { get; set; }
    }
    public class AdminController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCompleteRepairsByYear(string Year)
        {
            var x = db.Database.SqlQuery<ReportSummary>("GetCompletedRepairsByYear @p0", Convert.ToInt32(Year));
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRepairsByStatusId(int StatusId)
        {
            var x = db.Database.SqlQuery<ReportSummary>("GetRepairsByStatusId @p0", StatusId);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard()
        {
            try
            {
                var _items = new GetToalAndPendingRepairList
                {
                    TotalRepairs = db.Database.SqlQuery<ReportSummary>("GetNoOfRepairs").ToList(),
                    PendingRepairs = db.Database.SqlQuery<PendingReportSummary>("GetNoOfPendingRepairs").ToList()
                };
                ViewBag.SystemOverViewNumerofTotalRepairs = _items.TotalRepairs.Sum(s => s.NumberofRepairs);
                ViewBag.SystemOverViewNumerofPendingRepairs = _items.PendingRepairs.Count();
                ViewBag.Years = Enumerable.Range(2003,(DateTime.Now.AddYears(1).Year-2003)).Reverse();
                return View(_items);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    }
}
