using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace dsdProjectTemplate.Utility
{
    public static class ErrorLogUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorRroperty">High, Medium, Low</param>
        /// <param name="LogTitle"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static async Task SaveErrorLogAsync(ErrorPriority LogType, string LogTitle, Exception ex)
        {
            try
            {
                 
                if (LogTitle.Length >= 200)
                {
                    LogTitle = LogTitle.Substring(0, 190);
                }
         

                   // System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                    string _message = string.Empty;
                    //Console.WriteLine(trace.GetFrame(0).GetMethod().ReflectedType.FullName);
                    //Console.WriteLine("Line: " + trace.GetFrame(0).GetFileLineNumber());
                    //Console.WriteLine("Column: " + trace.GetFrame(0).GetFileColumnNumber());
                    //_message = "Method Name " + trace.GetFrame(0).GetMethod().ReflectedType.FullName;
                    //_message = _message + ", Line Number" + trace.GetFrame(0).GetFileLineNumber();
                    //_message = _message + ", ColumnNumber" + trace.GetFrame(0).GetFileColumnNumber();
                    _message = ex.Message;
                    if (_message.Length >= 2000)
                    {
                        _message = _message.Substring(0, 1999);
                    }
                     
                    if (LogTitle.Length >= 1000)
                    {
                        LogTitle = LogTitle.Substring(0, 190);
                    }
                string _stackTrace = ex.StackTrace;
                if (_stackTrace.Length >= 4000)
                {
                    _stackTrace = _stackTrace.Substring(0, 3999);
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.ErrorLogs + " (ErrorSource,LogMessage,LogTitle,LogType,LogDate) VALUES (@ErrorSource, @LogMessage,@LogTitle,@LogType,@LogDate)";
                   // query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    
                    parameters.Add("@ErrorSource", _stackTrace);
                    parameters.Add("@LogMessage", _message);
                    parameters.Add("@LogTitle", LogTitle);
                    parameters.Add("@LogType", LogType.ToString());
                    parameters.Add("@LogDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    await con.ExecuteScalarAsync(query, parameters);
                    con.Close();
                    LogTitle= _message = _stackTrace = null;
                }

                //TODO : If it is true then we will send an email, please set "SendErrorEmailEnabled" key in Web.config file <appSettings>
                if (WebConfigurationManager.AppSettings["SendErrorEmailEnabled"].ToString().ToLower()== "true")
                {

                }
                else if(LogType.ToString()== "Severe")
                {
                    // Send an error mail in Severe case 
                }
            }
            catch (Exception)
            {
                //try to write error log in a text file if system is not able to add log in the database 

            }

        }
    }
 
}
