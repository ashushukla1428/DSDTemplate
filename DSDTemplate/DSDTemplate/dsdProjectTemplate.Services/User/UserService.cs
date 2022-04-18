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
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User
{
    public class UserService :BaseServiceClass, IUserService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "User";
      
        public UserService()
        {
        }

        public async Task<ResponseModel> AddUserByAdminAsync(UserViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.Users.ToString(), "UserName", request.UserName, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                var emailExist = await CheckIfEmailAlreadyLinkedToAnOtherUser(request.EmailAddress, 0);
                if (!emailExist.Status)
                {
                    return emailExist;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.Users + " (UserName,UserTypeId,FirstName,MiddleName,LastName,ProfilePicture" +
                        ",EmailAddress,LastLoginDateTime,RegistrationDate,EmailTwoFactorAuthentication,SMSTwoFactorAuthentication,IsActive,CreatedBy,CreatedDate,PendingRegistration) " +
                        "VALUES (@UserName,@UserTypeId,@FirstName,@MiddleName,@LastName,@ProfilePicture" +
                        ",@EmailAddress,@LastLoginDateTime,@RegistrationDate,0,0, @IsActive,@CreatedBy,@CreatedDate,1)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", request.UserName);
                    parameters.Add("@UserTypeId", request.UserTypeId);
                    parameters.Add("@FirstName", request.FirstName);
                    parameters.Add("@MiddleName", request.MiddleName);
                    parameters.Add("@LastName", request.LastName);
                    parameters.Add("@ProfilePicture", "https://image.flaticon.com/icons/png/512/1946/1946429.png");
                    parameters.Add("@EmailAddress", request.EmailAddress);
                    parameters.Add("@LastLoginDateTime", DateTime.UtcNow);
                    parameters.Add("@RegistrationDate", DateTime.UtcNow);

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
                        TableName = AppTable.Users.ToString(),
                        PrimaryKeyId = request.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                }
                 
                if (request.Id != 0)
                {
                    IUserSendEmailService userSendEmailService = new UserSendEmailService();
                    await userSendEmailService.SendWelcomeMailAsync(request.Id, request.FirstName, request.EmailAddress,request.UserName);
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
        public async Task<ResponseModel> UpdateUserByAdminAsync(UserViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.Users.ToString(), "UserName", request.UserName, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                var emailExist = await CheckIfEmailAlreadyLinkedToAnOtherUser(request.EmailAddress, request.Id);
                if (!emailExist.Status)
                {
                    return emailExist;
                }
                var dataModel = await GetUsersByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(dataModel);
                
                if(request.Id== UserSession.Current.loggedIn_UserId)
                {
                    if (dataModel.IsActive != request.IsActive)
                    {
                        return new ResponseModel { Message = "You can't active or de-active your own acccount", Status = false };
                    }
                }

                if (dataModel != null)
                {
                    // Update record in the database
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Users + " SET  UserName=@UserName,FirstName=@FirstName,MiddleName=@MiddleName,LastName=@LastName,EmailAddress=@EmailAddress,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@UserName", request.UserName);
                        parameters.Add("@FirstName", request.FirstName);
                        parameters.Add("@MiddleName", request.MiddleName);
                        parameters.Add("@LastName", request.LastName);
                        parameters.Add("@EmailAddress", request.EmailAddress);

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
                        TableName = AppTable.Users.ToString(),
                        PrimaryKeyId = dataModel.Id,
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
      
        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = @"select us.Id,us.UserTypeId,us.IsActive,us.UserName,us.EmailAddress,us.FirstName,us.LastName from Users as us";
                        //inner join
                        //OrganizationUsers as orgUs on orgUs.UserId = us.id";
                        //and us.IsActive = 1";
                    
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + "  Where  us.Id not in (select userId from  SoftwareUsers where IsSuperAdmin=1)";
                      
                    }
                    query = query + " order by us.FirstName desc";
                    IEnumerable<UserViewModel> response = await con.QueryAsync<UserViewModel>(query, parameters, commandType: CommandType.Text);
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
        public async Task<ResponseModel> UserLinkWithOrganizationAsync(long OrganizationId, long UserId, int RoleId)
        {
            try
            {
                var _existRecordResponse = await CheckIfOrgUsersIsExist( RoleId,UserId, OrganizationId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                long Id = 0;
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.OrganizationUsers + " (OrganizationId,RoleId,UserId) VALUES (@OrganizationId,@RoleId,@UserId)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", OrganizationId);

                    parameters.Add("@RoleId", RoleId);
                    parameters.Add("@UserId", UserId);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();
                }
                   
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess("user role"), Status = true, Id = Id };
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->UserLinkWithOrganizationAsync", ex);
                throw;
            }
        }

        public async Task<UserProfileViewModel> GetMyProfileAsync()
        {
            try
            {
                return await GetUsersProfileByIdAsync(UserSession.Current.loggedIn_UserId);
              
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->GetMyProfileAsync error for username "+ UserSession.Current.UserName, ex);
                throw;
            }
        }

        public async Task<ResponseModel> UpdateMyProfileAsync(UserProfileViewModel request)
        {
            try
            {
                
                var _existRecordResponse = await CheckIfEmailAlreadyLinkedToAnOtherUser(request.EmailAddress, UserSession.Current.loggedIn_UserId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                if (request.SecurityQuestion1 == null)
                {
                    return new ResponseModel { Message = "Please select all security questions", Status = false };
                }
                if (request.SecurityQuestion2 == null)
                {
                    return new ResponseModel { Message = "Please select all security questions", Status = false };

                }
                if (request.SecurityQuestion3 == null)
                {
                    return new ResponseModel { Message = "Please select all security questions", Status = false };

                }
                List<int> _securityQuestions = new List<int>()
                {
                   (int) request.SecurityQuestion1,
                   (int) request.SecurityQuestion2,
                   (int)request.SecurityQuestion3
                };
                if (_securityQuestions.Count !=_securityQuestions.Distinct().Count())
                {
                    return new ResponseModel { Message = "Security questions should not be repeated", Status = false };
                }
                List<string> _securityAnswers = new List<string>()
                {
                    request.AnswerSecurityQuestion1,
                    request.AnswerSecurityQuestion2,
                   request.AnswerSecurityQuestion3
                };
                if (_securityAnswers.Count != _securityAnswers.Distinct().Count() )
                {
                    return new ResponseModel { Message = "Security answers should not be repeated", Status = false };
                }

                var editModel = await GetUsersByIdAsync(UserSession.Current.loggedIn_UserId);
                if (editModel != null)
                {
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Users + " SET  SMSTwoFactorAuthentication=@SMSTwoFactorAuthentication, EmailTwoFactorAuthentication=@EmailTwoFactorAuthentication,FirstName=@FirstName,MiddleName=@MiddleName,LastName=@LastName,EmailAddress=@EmailAddress" +
                            ",SecurityQuestion1=@SecurityQuestion1,SecurityQuestion2=@SecurityQuestion2,SecurityQuestion3=@SecurityQuestion3" +
                            ",AnswerSecurityQuestion1=@AnswerSecurityQuestion1,AnswerSecurityQuestion2=@AnswerSecurityQuestion2,AnswerSecurityQuestion3=@AnswerSecurityQuestion3" +
                            ",RegistrationCellPhoneNumber=@RegistrationCellPhoneNumber,RegistrationWorkPhoneNumber=@RegistrationWorkPhoneNumber,RegistrationWorkPhoneNumberExt=@RegistrationWorkPhoneNumberExt,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", UserSession.Current.loggedIn_UserId);
                        if (editModel.EmailTwoFactorAuthentication)
                        {
                            parameters.Add("@EmailTwoFactorAuthentication", false);
                        }
                        else
                        {
                            parameters.Add("@EmailTwoFactorAuthentication", true);
                        }
                        if (editModel.SMSTwoFactorAuthentication)
                        {
                            parameters.Add("@SMSTwoFactorAuthentication", false);
                        }
                        else
                        {
                            parameters.Add("@SMSTwoFactorAuthentication", true);
                        }
                        parameters.Add("@FirstName", request.FirstName);
                        parameters.Add("@MiddleName", request.MiddleName);
                        parameters.Add("@LastName", request.LastName);
                        parameters.Add("@EmailAddress", request.EmailAddress);
                        parameters.Add("@SecurityQuestion1", request.SecurityQuestion1);
                        parameters.Add("@SecurityQuestion2", request.SecurityQuestion2);
                        parameters.Add("@SecurityQuestion3", request.SecurityQuestion3);
                        parameters.Add("@AnswerSecurityQuestion1", request.AnswerSecurityQuestion1);
                        parameters.Add("@AnswerSecurityQuestion2", request.AnswerSecurityQuestion2);
                        parameters.Add("@AnswerSecurityQuestion3", request.AnswerSecurityQuestion3);

                        parameters.Add("@RegistrationCellPhoneNumber", request.RegistrationCellPhoneNumber);
                        parameters.Add("@RegistrationWorkPhoneNumber", request.RegistrationWorkPhoneNumber);
                        parameters.Add("@RegistrationWorkPhoneNumberExt", request.RegistrationWorkPhoneNumberExt);


                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    } 
                    return new ResponseModel { Message = "your profile has been successfully updated", Status = true };
                    }
                else
                {
                    return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false };
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->UpdateMyProfileAsync error for username " + UserSession.Current.UserName, ex);
                throw;
            }
        }

        public static bool IsValidateUserRole(String _Controller, String _Action)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                  
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserRoleID", UserSession.Current.UserRoleId);
                    parameters.Add("@MenuAction", _Action);
                    parameters.Add("@MenuController", _Controller);
                    var response =   con.Query<int>("usp_IsValidateUserRole", parameters, commandType: CommandType.StoredProcedure);
                    con.Close();
                    if (response != null)
                    {
                        if (response.Count() != 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                 }
                return false;
            }
            catch (Exception )
            {
                //AppUtility.LogMessage(ex, "ValidateUserRole", "AccountService.cs");
                throw;
            }

        }

        public async Task<IEnumerable<Organizations_UsersList>> GetOrganizations_UsersAsync(long OrganizationId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = @"select  org.Name as Organization ,orgUser.Id,usr.UserTypeId,orgUser.IsActive,usr.UserName,usr.EmailAddress,usr.FirstName,usr.LastName,usRole.RoleName as Role from OrganizationUsers as orgUser
                    inner join
                    users as usr on usr.Id = orgUser.UserId
                    inner join
                    UsersRoles as usRole on usRole.Id = orgUser.RoleId
                    inner join
					Organizations as org on org.Id=orgUser.OrganizationId
                    ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + " and orgUser.UserId not in (select userId from  SoftwareUsers where IsSuperAdmin=1) and orgUser.OrganizationId = @OrganizationId ";
                        if (OrganizationId == 0)
                        {
                            parameters.Add("@OrganizationId", UserSession.Current.SelectedOrgId);
                        }
                        else
                        {
                            parameters.Add("@OrganizationId", OrganizationId);

                        }
                    }
                    query = query + " order by usr.FirstName desc";
                    IEnumerable<Organizations_UsersList> response = await con.QueryAsync<Organizations_UsersList>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetOrganizations_UsersAsync", ex);
                throw;
            }
        }
        public async Task<ResponseModel> ActiveOrDeActiveOrganizations_UserAsync(long Id)
        {
            try
            {
                var existingData = await GetUserByIdAndOrgIdAsync(Id);
                string _code = string.Empty;
                if (existingData != null)
                {
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.OrganizationUsers + " SET IsActive=@IsActive WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", Id);
                        if (!UserSession.Current.IsSoftware_User)
                        {
                            query = query + " and OrganizationId=@OrganizationId";
                            parameters.Add("@OrganizationId", UserSession.Current.SelectedOrgId);
                        }

                        if (existingData.IsActive)
                        {

                            parameters.Add("@IsActive", false);
                            _code = "De-Activated";
                        }
                        else
                        {
                            parameters.Add("@IsActive", true);
                            _code = "Re-Activated";
                        }
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();

                        IUserSendEmailService userSendEmailService = new UserSendEmailService();
                        await userSendEmailService.SendOrg_UserActiveOrDeActiveNotificationAsync(existingData.UserId, _code, existingData.OrgName);
                        userSendEmailService = null;
                    }

                    return new ResponseModel { Message = ResponseMessages.SubjectUpdatedSuccess(_serviceFor), Status = true, Id = Id };
                }
                return new ResponseModel { Message = ResponseMessages.SubjectNotFound("user"), Status = false, Id = 0 };
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->ActiveOrDeActiveOrganizations_UserAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        
        public async Task<UserViewModel> GetUsersByEamilAsync(string EmailAddress)
        {
            UserViewModel response = new UserViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.Users + " (nolock) where EmailAddress=@EmailAddress ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmailAddress", EmailAddress);
                    response = await con.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        public async Task<UserViewModel> GetUsersByUserNameAsync(string userName)
        {
            UserViewModel response = new UserViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.Users + " (nolock) where userName=@userName ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@userName", userName);
                    response = await con.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        public async Task<UserViewModel> GetUsersByIdAsync(long Id)
        {
            UserViewModel response = new UserViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.Users + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
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
                        Organizations as org on org.Id = orgUser.OrganizationId and orgUser.Id = @Id ";
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

        public async Task<UserProfileViewModel> GetUsersProfileByIdAsync(long Id)
        {
            UserProfileViewModel response = new UserProfileViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.Users + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<UserProfileViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        public async Task<ResponseModel> CheckIfOrgUsersIsExist(int RoleId, long UserId , long OrganizationId)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                query = "SELECT count(id) FROM " + AppTable.OrganizationUsers + " WHERE  OrganizationId=@OrganizationId and UserId=@UserId and RoleId=@RoleId";
                parameters.Add("@OrganizationId", OrganizationId);
                parameters.Add("@RoleId", RoleId);
                parameters.Add("@UserId", UserId);
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
        private async Task<ResponseModel> CheckIfEmailAlreadyLinkedToAnOtherUser(string emailAddrees,long userId)
        {
            //UserViewModel response = new UserViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select [Id] from " + AppTable.Users + " (nolock) where EmailAddress=@EmailAddress ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmailAddress", emailAddrees);
                    if (userId != 0)
                    {
                        query += " and id!=@userId ";
                        parameters.Add("@userId", userId);
                    }
                    con.Close();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = "this email address is already in use with another user" };
                    }
                    else
                    {
                        return new ResponseModel { Status = true, Message = null };
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }
    }
}
