using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.City;
using dsdProjectTemplate.ViewModel.MenuIcons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace dsdProjectTemplate.Services.Menu.MenuIcons
{
    public class MenuIcons: BaseServiceClass,IMenuIcons
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "MenuIcons";
        #region core functions of the service
        public async Task<ResponseModel> AddAsync(MenuIconsResponse request)
        {
            try
            {
                if (!this.CanAddRecords())
                {
                    return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
                }
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.MenuIcons.ToString(), "Name", request.Name, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.MenuIcons + " (Name,Value,IsActive,CreatedBy,CreatedDate) VALUES (@Name,@Value, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", request.Name);
                    parameters.Add("@Value", request.Value);
                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();
                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Insert.ToString(),
                        TableName = AppTable.States.ToString(),
                        PrimaryKeyId = request.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                }

                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(MenuIconsResponse request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.States.ToString(), "StateName", request.Name, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    // Update record in the database
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.MenuIcons + " SET  Name=@Name,Value=@Value,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@Name", request.Name);
                        parameters.Add("@Value", request.Value);
                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.MenuIcons.ToString(),
                        PrimaryKeyId = item.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request, oldRec = JsonConvert.DeserializeObject(oldRecord) })
                    });
                    oldRecord = null;
                    return new ResponseModel { Message = ResponseMessages.SubjectUpdatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
                else
                {
                    return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->UpdateAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = request.Id };
            }
        }
        public async Task<IEnumerable<MenuIconsResponse>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select Id,Name,Value,IsActive from " + AppTable.MenuIcons + " (nolock) order by id desc ";
                    var parameters = new DynamicParameters();
                    IEnumerable<MenuIconsResponse> response = await con.QueryAsync<MenuIconsResponse>(query, parameters, commandType: CommandType.Text);
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
        public async Task<List<SelectListItem>> GetMenusIcons()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Name,Value  from " + AppTable.MenuIcons + " (nolock) where IsActive=1 order by Name asc ";                    
                    var _data = await con.QueryAsync(query, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Value }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetMenusIcons", ex);
                throw;
            }
        }
        private async Task<MenuIconsResponse> GetByIdAsync(long Id)
        {
            MenuIconsResponse response = new MenuIconsResponse();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,Name,Value,IsActive from " + AppTable.MenuIcons + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<MenuIconsResponse>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }

    }
}
#endregion