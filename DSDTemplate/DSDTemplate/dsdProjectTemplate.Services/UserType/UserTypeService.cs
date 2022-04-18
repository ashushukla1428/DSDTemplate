using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.UserType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace dsdProjectTemplate.Services.UserType
{
    public class UserTypeService:BaseServiceClass, IUserTypeService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "user type";
        public UserTypeService()
        {
        }
        public async Task<ResponseModel> AddAsync(UserTypeViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.UserType.ToString(), "TypeName", request.TypeName, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.UserType + " (TypeName,IsActive,CreatedBy,CreatedDate) VALUES (@TypeName, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@TypeName", request.TypeName);

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
                        TableName = AppTable.UserType.ToString(),
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
                return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(UserTypeViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.UserType.ToString(), "TypeName", request.TypeName, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.UserType + " SET  TypeName=@TypeName,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@TypeName", request.TypeName);

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
                        TableName = AppTable.UserType.ToString(),
                        PrimaryKeyId = item.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request, oldRec = JsonConvert.DeserializeObject(oldRecord) })
                    });
                    oldRecord = null;
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectUpdatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
                else
                {
                    return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = request.Id };
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->UpdateAsync", ex);
                return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = request.Id };
            }
        }
        public async Task<IEnumerable<UserTypeViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select Id,TypeName,IsActive from " + AppTable.UserType + " (nolock) order by id desc ";
                    var parameters = new DynamicParameters();
                    IEnumerable<UserTypeViewModel> response = await con.QueryAsync<UserTypeViewModel>(query, parameters, commandType: CommandType.Text);
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
        public async Task<List<SelectListItem>> GetDropUsersTypeAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,TypeName from " + AppTable.UserType + " (nolock) order by TypeName asc ";
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<UserTypeViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.TypeName, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }

              
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropUsersTypeAsync", ex);
                throw;
            }
        }
        private async Task<UserTypeViewModel> GetByIdAsync(long Id)
        {
            UserTypeViewModel response = new UserTypeViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,TypeName,IsActive from " + AppTable.UserType + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<UserTypeViewModel>(query, parameters, commandType: CommandType.Text);
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
