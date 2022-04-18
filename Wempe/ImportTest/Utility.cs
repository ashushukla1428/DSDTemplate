using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImportTest
{
    public static class Utility
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
                DataView view = new DataView(dt);
                dt = view.ToTable(false, new string[] { "ID", "County", "State", "City", "Zip", "Abbrev" });
                dt = dt.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable();

            }


            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            // DataTable dt = new DataTable();
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$] where [State] IS Not NULL", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                DataColumn Col = dt.Columns.Add("ID", typeof(int));
                Col.SetOrdinal(0);
                Col.AutoIncrement = true;
                Col.AutoIncrementSeed = 1;
                Col.AutoIncrementStep = 1;
                oleda.Fill(dt);

                DataView view = new DataView(dt);
                dt = view.ToTable(false, new string[] { "ID", "County", "State", "City", "Zip", "Abbrev" });
                dt = dt.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable();

            }
            catch (Exception ex)
            {

            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }
        public static int AddRecordsInTable(DataTable dt,Int64 CountryId)
        {
            try
            {
                DataColumn newCol = new DataColumn("CountryId", typeof(Int64));
                newCol.DefaultValue = CountryId;
                dt.Columns.Add(newCol);

                string consString = ConfigurationManager.ConnectionStrings["dbWempeEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_UploadCity_State_Country"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Connection = con;

                        SqlParameter parameter = new SqlParameter();
                        //The parameter for the SP must be of SqlDbType.Structured 
                        parameter.ParameterName = "@TableVariable";
                        parameter.SqlDbType = System.Data.SqlDbType.Structured;
                        parameter.Value = dt;
                        cmd.Parameters.Add(parameter);

                        //cmd.Parameters.AddWithValue("@TableVariable", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static ImportStatus GetImportStatus()
        {
            ImportStatus sts = new ImportStatus();
            string consString = ConfigurationManager.ConnectionStrings["dbWempeEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("select IsActive,TotalRecords,ImportedRecords from wmpImportStatus where Id=1"))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 0;
                        cmd.Connection = con;
                        SqlDataAdapter adp = new SqlDataAdapter();
                        adp.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        if (ds.Tables[0] != null)
                        {
                            sts.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]);
                            sts.ImportedRecords = Convert.ToInt64(ds.Tables[0].Rows[0]["ImportedRecords"]);
                            sts.TotalRecords = Convert.ToInt64(ds.Tables[0].Rows[0]["TotalRecords"]);
                            sts.ResultCount = sts.TotalRecords - sts.ImportedRecords;
                        }
                        else
                            sts = null;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return sts;
        }
        public static List<SelectListItem> BindCountry(Int64 selectedID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            bool selected = false;
            if (selectedID == 0)
            {
                selected = true;
            }
            items.Add(new SelectListItem
            {
                Text = "Select Country",
                Value = "0",
                Selected = selected
            });
            string consString = ConfigurationManager.ConnectionStrings["dbWempeEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("select Id,country from wmpcountry where isactive=1"))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 0;
                        cmd.Connection = con;
                        SqlDataAdapter adp = new SqlDataAdapter();
                        adp.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        if (ds.Tables[0] != null)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                selected = false;
                                if (Convert.ToInt64(ds.Tables[0].Rows[i]["Id"]) == selectedID)
                                {
                                    selected = true;
                                }
                                items.Add(new SelectListItem
                                { Text = ds.Tables[0].Rows[i]["country"].ToString(), Value = ds.Tables[0].Rows[i]["Id"].ToString(), Selected = selected });
                            }
                        }
                        return items;                        
                    }
                    catch (Exception ex)
                    { return null; }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
    }
    public class ImportStatus
    {
        public bool IsActive { get; set; }
        public Int64 TotalRecords { get; set; }
        public Int64 ImportedRecords { get; set; }
        public Int64 ResultCount { get; set; }
    }
}
