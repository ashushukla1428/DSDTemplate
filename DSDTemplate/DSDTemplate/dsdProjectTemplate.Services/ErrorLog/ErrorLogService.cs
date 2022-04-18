using Dapper;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.ErrorLog
{
    public class ErrorLogService : IErrorLogService
    {
        public ErrorLogService()
        {
        }
        
        public async Task<IEnumerable<ErrorLogViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select LogId as Id,LogMessage,ErrorSource,LogDate,LogTitle,LogType  from " + AppTable.ErrorLogs + " (nolock) order by LogDate desc ";
                    var parameters = new DynamicParameters();
                    IEnumerable<ErrorLogViewModel> response = await con.QueryAsync<ErrorLogViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }
               
            }
            catch (System.Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Low, this.GetType().Name + "->GetAllAsync", ex);
                throw;
            }
        }
    }
}
