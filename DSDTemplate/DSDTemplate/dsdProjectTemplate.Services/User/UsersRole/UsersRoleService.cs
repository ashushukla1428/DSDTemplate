using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.User.UsersRole
{
    public class UsersRoleService:BaseServiceClass, IUsersRoleService
    {
       
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "User role";

        #region Main
        public UsersRoleService()
        {
        }
        public async Task<ResponseModel> AddAsync(UsersRoleViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.UsersRoles.ToString(), "RoleName", request.RoleName, request.Id, request.OrganizationId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.UsersRoles + " (OrganizationId,RoleName,CanAddRecords,CanEditRecords,IsActive,CreatedBy,CreatedDate) VALUES (@OrganizationId,@RoleName,@CanAddRecords,@CanEditRecords, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", request.OrganizationId);
                    parameters.Add("@RoleName", request.RoleName);
                    parameters.Add("@CanAddRecords", request.CanAddRecords);
                    parameters.Add("@CanEditRecords", request.CanEditRecords);

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
                        TableName = AppTable.UsersRoles.ToString(),
                        PrimaryKeyId = request.Id,
                        OrganizationId = request.OrganizationId,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(UsersRoleViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.UsersRoles.ToString(), "RoleName", request.RoleName, request.Id, request.OrganizationId);
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
                        var query = "UPDATE " + AppTable.UsersRoles + " SET  OrganizationId=@OrganizationId,RoleName=@RoleName,CanEditRecords=@CanEditRecords,CanAddRecords=@CanAddRecords,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@OrganizationId", request.OrganizationId);
                        parameters.Add("@RoleName", request.RoleName);
                        parameters.Add("@CanAddRecords", request.CanAddRecords);
                        parameters.Add("@CanEditRecords", request.CanEditRecords);

                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.UsersRoles.ToString(),
                        PrimaryKeyId = item.Id,
                        OrganizationId=item.OrganizationId,
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

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->UpdateAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<IEnumerable<UsersRoleViewModel>> GetAllAsync(long organizationId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                   // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = "select Id,OrganizationId,RoleName,IsActive,CanAddRecords,CanEditRecords from " + AppTable.UsersRoles + " (nolock)  ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by id desc";
                    IEnumerable<UsersRoleViewModel> response = await con.QueryAsync<UsersRoleViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }
                
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetAllAsync", ex);
                throw;
            }
        }

        public async Task<List<SelectListItem>> GetDropListAsync(long organizationId,bool GetAll=false)
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,RoleName from " + AppTable.UsersRoles + " (nolock) ";
                    var parameters = new DynamicParameters();
                    if (!GetAll)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by RoleName asc ";

                    var _data = await con.QueryAsync<UsersRoleViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.RoleName, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetDropListAsync", ex);
                throw;
            }
        }
        #endregion

        public async Task<UsersRoleViewModel> GetByIdAsync(long Id)
        {
            UsersRoleViewModel response = new UsersRoleViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.UsersRoles + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<UsersRoleViewModel>(query, parameters, commandType: CommandType.Text);
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
