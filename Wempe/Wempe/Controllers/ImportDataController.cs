using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;

namespace Wempe.Controllers
{
    public class ImportDataController : Controller
    {
        //
        // GET: /ImportData/
        //
        public ActionResult ImportCities()
        {
            ViewBag.Country = ImportUtility.BindCountry(0);

            ImportStatus sts = ImportUtility.GetImportStatus();
            return View(sts);
        }
        public ActionResult ImportResult()
        {
            ImportStatus sts = ImportUtility.GetImportStatus();
            return Json(sts, JsonRequestBehavior.AllowGet);
        }
        [ActionName("ImportCities")]
        [HttpPost]
        public ActionResult ImportCitiesState()
        {
            Int64 CountryID = Convert.ToInt64(Request.Form["country"]);
            if (CountryID != 0)
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                    string query = null;
                    string connString = "";

                    //string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    string[] validFileTypes = { ".xls", ".xlsx" };

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
                            dt = ImportUtility.ConvertCSVtoDataTable(path1);
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            dt = ImportUtility.ConvertXSLXtoDataTable(path1, connString);
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            dt = ImportUtility.ConvertXSLXtoDataTable(path1, connString);
                        }
                        dt = dt.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable();
                        try
                        {

                            var thread = new Thread(() => ImportUtility.AddRecordsInTable(dt, CountryID));
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
                        // ViewBag.Message = "Please Upload Files in .xls, .xlsx or .csv format";
                        ViewBag.Message = "Please Upload Files in .xls or .xlsx format";
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
            ViewBag.Country = ImportUtility.BindCountry(CountryID);
            ImportStatus sts = ImportUtility.GetImportStatus();
            return View(sts);
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
