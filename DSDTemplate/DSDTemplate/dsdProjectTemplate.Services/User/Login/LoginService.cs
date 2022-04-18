using Dapper;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.Login
{
    public class LoginService : ILoginService
    {
        string PassPhrase = string.Empty;
        string HashAlgorithm = string.Empty;
        string InitVector = string.Empty;
        public LoginService()
        {
            PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            HashAlgorithm = ConfigurationManager.AppSettings["HashAlgorithm"];
            InitVector = ConfigurationManager.AppSettings["InitVector"];
        }
        public async Task<LoginResponse> LoginAsync(LoginViewModel request)
        {
            UserService userService = new UserService();
            try
            {
                var user = await userService.GetUsersByUserNameAsync(request.UserName);
                if (user == null)
                {
                    return new LoginResponse { Message = "Invalid user name" };
                }

                if (user.IsActive == false)
                {
                    return new LoginResponse { Message = "Account is no longer active" };
                }
                if (user.PendingRegistration == true)
                {
                    return new LoginResponse { Message = "Registration' steps are pending, please check your welcome mail or request for Forgot Password!" };
                }
                var _userPassword = await GetUserPasswordByIdAsync(user.Id);
                if (_userPassword == null)
                {
                    return new LoginResponse { Message = "Registration' steps are pending, please check your welcome mail or request for Forgot Password!" };
                }
                string decryptedPassword = UPasswordRijndaelAlgorithm.Decrypt(_userPassword.EncryptedPassword, _userPassword.PasswordSalt, PassPhrase, HashAlgorithm, InitVector);


                if (request.Password.Equals(decryptedPassword))
                {

                    //Super Admin check is pending
                    var _superAdminUser = await GetSoftwareUserByIdAsync(user.Id);
                    Boolean _isSoftware_User = false;
                    Boolean _isSuperAdmin = false;
                    if (_superAdminUser != null)
                    {
                        _isSoftware_User = true;
                        _isSuperAdmin = _superAdminUser.IsSuperAdmin;
                    }
                    var _organizationData = await GetLoggedInUserDataByIdAsync(user.Id);
                    if (_organizationData.Count() != 0)
                    {
                       
                        
                      //  var _role = _organizationData.Where(c=>c.OrganizationId== _organizationData.FirstOrDefault().OrganizationId).FirstOrDefault().UsersRole;

                        List<LoggedInUserOrgList> userOrganizationList = new List<LoggedInUserOrgList>();
                        foreach (var item in _organizationData)
                        {
                            userOrganizationList.Add(new LoggedInUserOrgList
                            {
                                OrgId = item.OrgId,
                                OrgName = item.OrgName,
                                RoleId=item.RoleId,
                                CanAddRecords=item.CanAddRecords,
                                CanEditRecords=item.CanAddRecords,
                                RoleName =item.RoleName,
                            });
                        }

                        var selectOrg = _organizationData.FirstOrDefault();
                        if (_isSuperAdmin)
                        {
                            selectOrg.CanEditRecords = true;
                            selectOrg.CanAddRecords = true;
                        }
                        return new LoginResponse {
                            Status = true, UserId = user.Id, UserName = user.UserName, 
                            OrgList = userOrganizationList, SelectedOrgId = selectOrg.OrgId,SelectedOrgName= selectOrg.OrgName,
                            Roles = selectOrg.RoleName, UserRoleId = selectOrg.RoleId, FullName = user.FirstName + ' ' + user.LastName, 
                            CanAddRecords= selectOrg.CanAddRecords,CanEditRecords= selectOrg.CanEditRecords,
                            FirstName = user.FirstName, LastName = user.LastName, Email = user.EmailAddress, 
                            IsSoftware_User=_isSoftware_User,
                            IsSuperAdmin=_isSuperAdmin,
                            EmailTwoFactorAuthentication= user.EmailTwoFactorAuthentication,
                            SMSTwoFactorAuthentication= user.SMSTwoFactorAuthentication
                        };
                    }
                    else
                    {
                        return new LoginResponse { Message = "Your account is not linked with any organization or role." };
                    }

                    
                }  //Login Logic
                else
                {

                    return new LoginResponse { Status = false, Message = ResponseMessages.WRONG_USER };
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->LoginAsync|UName "+request.UserName, ex);
                return new LoginResponse { Status = false, Message = ResponseMessages.System_Error };
            }
        }

        public async Task<UsersLoginPassword> GetUserPasswordByIdAsync(long UserId)
        {
            UsersLoginPassword response = new UsersLoginPassword();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.UsersLoginPasswords + " (nolock) where UserId=@UserId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", UserId);
                    response = await con.QueryFirstOrDefaultAsync<UsersLoginPassword>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->GetUserPasswordByIdAsync", ex);
                return response;
            }
        }
        public async Task<SoftwareUser> GetSoftwareUserByIdAsync(long UserId)
        {
            SoftwareUser response = new SoftwareUser();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.SoftwareUsers + " (nolock) where UserId=@UserId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", UserId);
                    response = await con.QueryFirstOrDefaultAsync<SoftwareUser>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->GetSoftwareUserByIdAsync", ex);
                return response;
            }
        }
        public async Task<IEnumerable<UserOrgList>> GetLoggedInUserDataByIdAsync(long UserId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = @" update  Users set LastLoginDateTime=getdate() where Id=@UserId; 
                       select orgUser.UserId,orgUser.OrganizationId as OrgId,org.Name as OrgName,roles.Id as RoleId
                        ,roles.RoleName,roles.CanAddRecords,roles.CanEditRecords,orgUser.IsActive,orgUser.Note
                        ,usr.FirstName,usr.LastName,usr.EmailAddress,EmailTwoFactorAuthentication,SMSTwoFactorAuthentication
                          from OrganizationUsers as orgUser
                        inner join
                        Organizations as org on org.Id=orgUser.OrganizationId and org.IsActive=1 and orgUser.IsActive=1
                        inner join
                        Users as usr on usr.Id=orgUser.UserId
                        inner join
                        UsersRoles as roles on roles.Id=orgUser.RoleId and orgUser.UserId=@UserId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", UserId);
                    IEnumerable<UserOrgList> response = await con.QueryAsync<UserOrgList>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->Login", ex);
                return null;
            }
        }
    }
}
