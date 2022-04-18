using Dapper;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.AppActionsHistory
{
    public  class ActionsHistoryService: IActionsHistoryService
    {
        public  async Task<bool> AddAsync(ActionsHistoryViewModel request)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.AppActionsHistory + " (TableName,PrimaryKeyId,ActionType,OrganizationId,Data,UserId,CreatedDate) VALUES (@TableName,@PrimaryKeyId,@ActionType,@OrganizationId,@Data,@UserId,@CreatedDate)";

                    var parameters = new DynamicParameters();
                    parameters.Add("@TableName", request.TableName);
                    parameters.Add("@PrimaryKeyId", request.PrimaryKeyId);
                    parameters.Add("@ActionType", request.ActionType);
                    parameters.Add("@OrganizationId", request.OrganizationId);
                    parameters.Add("@Data", request.Data);

                    parameters.Add("@UserId", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    await con.ExecuteAsync(query, parameters);
                    con.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Low, this.GetType().Name + "->AddAsync", ex);
                return false;
            }
        }
        public  async Task<IEnumerable<ActionsHistoryViewModel>> GetAllAsync(ActionsHistorySearch request)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select Id,[TableName],[PrimaryKeyId],[ActionType],[OrganizationId],[Data],[UserId],[CreatedDate] as ActionDate from " + AppTable.AppActionsHistory + " (nolock) ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query+ " where OrganizationId=@OrganizationId ";
                        parameters.Add("@OrganizationId", UserSession.Current.SelectedOrgId);
                    }

                    query = query + "  order by id desc ";

                    IEnumerable<ActionsHistoryViewModel> response = await con.QueryAsync<ActionsHistoryViewModel>(query, parameters, commandType: CommandType.Text);

                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetAllAsync", ex);
                throw;
            }
        }

        public async Task<ActionsHistoryViewModel> GetByIdAsync(long Id)
        {
            ActionsHistoryViewModel response = new ActionsHistoryViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,[TableName],[PrimaryKeyId],[ActionType],[OrganizationId],[Data],[UserId],[CreatedDate] as ActionDate from " + AppTable.AppActionsHistory + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<ActionsHistoryViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Low, this.GetType().Name + "->GetByIdAsync", ex);
                return response;
            }
           
        }
    }
}
