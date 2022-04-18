
using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;

using dsdProjectTemplate.Services.SendEmail.User;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.UserAndOrganization
{
    public class UserAndOrganizationService:BaseServiceClass, IUserAndOrganizationService
    {
        
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();

        string _serviceFor = " user";
        public UserAndOrganizationService()
        {
        }

        public async Task<ResponseModel> AddAsync(UserAndOrganizationViewModel request)
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
                if (request.RoleId == 0)
                {
                    return new ResponseModel { Message = "Please select a role ", Status = false, Id = request.Id };
                }
                if (request.RegistrationRequestTypeId == 0)
                {
                    return new ResponseModel { Message = "Please select a registration request type ", Status = false, Id = request.Id };
                }

                var _existRecordResponse = await CheckIfOrgUsersIsExist(request.Id, request.UserId, request.OrganizationId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.OrganizationUsers + " (OrganizationId,RoleId,UserId,RegistrationRequestTypeId,IsActive) VALUES (@OrganizationId,@RoleId,@UserId,@RegistrationRequestTypeId,@IsActive)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", request.OrganizationId);

                    parameters.Add("@RoleId", request.RoleId);
                    parameters.Add("@UserId", request.UserId);
                    parameters.Add("@RegistrationRequestTypeId", request.RegistrationRequestTypeId);
                    parameters.Add("@IsActive", request.IsActive);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();
                }
                //start ActionsHistory
                await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                {
                    ActionType = UserActon.Insert.ToString(),
                    TableName = AppTable.OrganizationUsers.ToString(),
                    PrimaryKeyId = request.Id,
                    OrganizationId= request.OrganizationId,
                    Data = JsonConvert.SerializeObject(new { newRec = request })
                });
                //end ActionsHistory
                string _code = string.Empty;
                var userData = await GetUserAndOrgDetail(request.UserId, request.OrganizationId);
                if (userData.IsActive)
                {
                    _code = "Activated";
                    IUserSendEmailService userSendEmailService = new UserSendEmailService();
                    await userSendEmailService.SendOrg_UserActiveOrDeActiveNotificationAsync(userData.UserId, _code, userData.OrgName);
                    userSendEmailService = null;
                }
              
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(UserAndOrganizationViewModel request)
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
                if (request.RoleId == 0)
                {
                    return new ResponseModel { Message = "Please select a role ", Status = false, Id = request.Id };
                }
                if (request.RegistrationRequestTypeId == 0)
                {
                    return new ResponseModel { Message = "Please select a registration request type ", Status = false, Id = request.Id };
                }

                var _existRecordResponse = await CheckIfOrgUsersIsExist(request.Id,request.UserId, request.OrganizationId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                var item = await GetByIdAsync( request.Id );
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    // Update record in the database
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.OrganizationUsers + " SET  OrganizationId=@OrganizationId,RoleId=@RoleId,UserId=@UserId,RegistrationRequestTypeId=@RegistrationRequestTypeId,IsActive=@IsActive WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@OrganizationId", request.OrganizationId);
                        parameters.Add("@RoleId", request.RoleId);
                        parameters.Add("@UserId", request.UserId);
                        parameters.Add("@RegistrationRequestTypeId", request.RegistrationRequestTypeId);

                        parameters.Add("@IsActive", request.IsActive);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    string _code = string.Empty;
                    var userData = await GetUserAndOrgDetail(request.UserId, request.OrganizationId);
                    if (userData.IsActive)
                    {
                        _code = "Activated";                        
                    }
                    else
                    {
                        _code = "De-Activated";
                    }

                    IUserSendEmailService userSendEmailService = new UserSendEmailService();
                    await userSendEmailService.SendOrg_UserActiveOrDeActiveNotificationAsync(userData.UserId, _code, userData.OrgName);
                    userSendEmailService = null;
                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.OrganizationUsers.ToString(),
                        PrimaryKeyId = item.Id,
                        OrganizationId = item.OrganizationId,
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
        public async Task<IEnumerable<UserAndOrganizationViewModel>> GetUserAndOrganizationListAsync(long userId)
        {
            try
            {
                List<UserAndOrganizationViewModel> _respone = new List<UserAndOrganizationViewModel>();
                var _data = await GetUsersByOrgIdListAsync(userId);
                foreach (var item in _data)
                {
                    _respone.Add(new UserAndOrganizationViewModel
                    {
                        Id = item.Id,
                        OrganizationId = item.OrgId,
                        RegistrationRequestTypeId= item.RegistrationRequestTypeId,
                        RoleId = item.RoleId,
                        UserId = item.UserId,
                        IsActive= item.IsActive
                    });
                }
                return _respone;
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetUserAndOrganizationListAsync", ex);
                throw;
            }
        }
        public async Task<ResponseModel> CheckIfOrgUsersIsExist(long Id, long UserId, long OrganizationId)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                query = "SELECT count(id) FROM " + AppTable.OrganizationUsers + " WHERE  OrganizationId=@OrganizationId and UserId=@UserId ";
                parameters.Add("@OrganizationId", OrganizationId);
                parameters.Add("@UserId", UserId);
                if (Id != 0)
                {
                    query = query + " and Id!=@Id";
                    parameters.Add("@Id", Id);
                }
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = ResponseMessages.SubjectAlreadyExists("user") };
                    }
                    else
                    {
                        return new ResponseModel { Status = true, Message = null };
                    }
                }

            }
            catch (Exception ex)
            {
                query = null;
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<UserOrgList> GetUserByIdAndOrgIdAsync(long Id)
        {
            UserOrgList response = new UserOrgList();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = @"select usr.Id as UserId,org.Name as OrgName,orgUser.IsActive	 from OrganizationUsers as orgUser
                        inner join
                        Users as usr on usr.id = orgUser.UserId
                        inner join
                        Organizations as org on org.Id = orgUser.OrganizationId and orgUser.UserId = @Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    if (!UserSession.Current.IsSoftware_User)
                    {
                        query = query + " and orgUser.OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", UserSession.Current.SelectedOrgId);
                    }

                    response = await con.QueryFirstOrDefaultAsync<UserOrgList>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }

        private async Task<UserOrgList> GetUserAndOrgDetail(long userId,long orgId)
        {
            UserOrgList response = new UserOrgList();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = @"select usr.Id as UserId,org.Name as OrgName,orgUser.IsActive	 from OrganizationUsers as orgUser
                        inner join
                        Users as usr on usr.id = orgUser.UserId
                        inner join
                        Organizations as org on org.Id = orgUser.OrganizationId and orgUser.UserId = @userId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@userId", userId);
                    query = query + " and orgUser.OrganizationId=@OrganizationId";
                    parameters.Add("@OrganizationId", orgId);

                    response = await con.QueryFirstOrDefaultAsync<UserOrgList>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        private async Task<IEnumerable<UserOrgList>> GetUsersByOrgIdListAsync(long userId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = @"select orgUser.Id, usr.Id as UserId,org.Id as OrgId,orgUser.RoleId,org.Name as OrgName,orgUser.IsActive,orgUser.RegistrationRequestTypeId	 from OrganizationUsers as orgUser
                        inner join
                        Users as usr on usr.id = orgUser.UserId
                        inner join
                        Organizations as org on org.Id = orgUser.OrganizationId and orgUser.UserId = @userId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@userId", userId);


                    IEnumerable<UserOrgList> response = await con.QueryAsync<UserOrgList>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }
        private async Task<UserAndOrganizationViewModel> GetByIdAsync(long Id)
        {
            UserAndOrganizationViewModel response = new UserAndOrganizationViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.OrganizationUsers + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<UserAndOrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        private async Task<IEnumerable<UserAndOrganizationViewModel>> GetByUserIdAsync(long Id)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.OrganizationUsers + " (nolock) where UserId=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    IEnumerable<UserAndOrganizationViewModel> response     = await con.QueryAsync<UserAndOrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
