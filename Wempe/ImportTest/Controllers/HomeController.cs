using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ImportTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ImportResult()
        {
            ImportStatus sts = Utility.GetImportStatus();
            return Json(sts, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImportExcel()
        {          
            ViewBag.Country =  Utility.BindCountry(0);
            
            ImportStatus sts = Utility.GetImportStatus();           
            return View(sts);
        }
        [ActionName("Importexcel")]
        [HttpPost]
        public ActionResult Importexcel1()
        {
            Int64 CountryID = Convert.ToInt64(Request.Form["country"]);
            if (CountryID != 0)
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";

                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }
                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        { System.IO.File.Delete(path1); }
                        Request.Files["FileUpload1"].SaveAs(path1);
                        DataTable dt = new DataTable();
                        if (extension == ".csv")
                        {
                            dt = Utility.ConvertCSVtoDataTable(path1);
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        }
                        dt = dt.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable();
                        try
                        {

                            var thread = new Thread(() => Utility.AddRecordsInTable(dt, CountryID));
                            thread.Start();
                            // ViewBag.Message = "Backend thread is working on importing data.....";
                        }
                        catch (Exception ex)
                        {
                            // ViewBag.Message = "There is some error on importing data.....";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please Upload Files in .xls, .xlsx or .csv format";
                    }
                }
                else
                {
                    ViewBag.Message = "Please upload imported file.";
                }
            }
            else
            {
                ViewBag.Message = "Please select country.";
            }
            ViewBag.Country = Utility.BindCountry(CountryID);
            ImportStatus sts = Utility.GetImportStatus();
            return View(sts);
        }
    }
}