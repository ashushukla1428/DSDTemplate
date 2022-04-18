using Dapper;
using dsdProjectTemplate.Services.User.Login;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.ForgotPassword
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        string PassPhrase = string.Empty;
        string HashAlgorithm = string.Empty;
        string InitVector = string.Empty;
        //Entities _context = new Entities();
        IUserService _userService;
        public ForgotPasswordService()
        {
            PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            HashAlgorithm = ConfigurationManager.AppSettings["HashAlgorithm"];
            InitVector = ConfigurationManager.AppSettings["InitVector"];
            _userService =new UserService();
        }

        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordModel request)
        {
            try
            {
                
                var user = await _userService.GetUsersByEamilAsync( request.Email);
                if (request.ConfirmPassword != request.Password)
                {
                    return new ResponseModel { Message = "Password and confirm password not matched" };
                }
                if (user == null)
                {
                    return new ResponseModel { Message = "User Name not found." };
                }
                if (user.IsActive == false)
                {
                    return new ResponseModel { Message = "Account is De-Activated by administrator" };
                }
                if (string.IsNullOrEmpty(request.Key))
                {
                    return new ResponseModel { Message = "Your password reset link has expired" };
                }
                string  password = request.Password;
                long userId = Convert.ToInt32(CommonUtility.DecryptUrl(request.Key));

                var _user = await  _userService.GetUsersByIdAsync(  userId);
                LoginService _loginService = new LoginService();
                var _userPassword = await _loginService.GetUserPasswordByIdAsync(userId);
                string _encryptedPassword = string.Empty;
                string saltValue = UPasswordRijndaelAlgorithm.CreateSalt();
                if (string.IsNullOrEmpty(password))
                {
                    //If password is null then system will generate auto password 
                    password = UPasswordRijndaelAlgorithm.CreateRandomPassword(8);
                }
                _encryptedPassword = UPasswordRijndaelAlgorithm.Encrypt(password, saltValue, PassPhrase, HashAlgorithm, InitVector);

                if (_userPassword == null)
                {   // Insert record in the database
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "INSERT INTO " + AppTable.UsersLoginPasswords + " (UserId,EncryptedPassword,PasswordSalt,SecurityToken,IsActive,CreatedBy,CreatedDate) VALUES (@UserId,@EncryptedPassword,@PasswordSalt,@SecurityToken, @IsActive,@CreatedBy,@CreatedDate)";
                        query = query + " SELECT CAST(scope_identity() AS int);";
                        var parameters = new DynamicParameters();
                        parameters.Add("@EncryptedPassword", _encryptedPassword);
                        parameters.Add("@PasswordSalt", saltValue);
                        parameters.Add("@SecurityToken", Guid.NewGuid().ToString());
                        parameters.Add("@UserId", userId);

                        parameters.Add("@IsActive", true);
                        parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);

                    }

                    if (user.PendingRegistration)
                    { // Update record in the database
                        await PendingRegistrationSteps(userId);

                    }

                    return new ResponseModel { Message = "Your password has been changed.", Status = true };
                }
                else
                {
                    //update
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.UsersLoginPasswords + " SET  EncryptedPassword=@EncryptedPassword," +
                            "PasswordSalt=@PasswordSalt,SecurityToken=@SecurityToken,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE userId=@userId";

                        var parameters = new DynamicParameters();
                        parameters.Add("@userId", userId);

                        parameters.Add("@EncryptedPassword", _encryptedPassword);
                        parameters.Add("@PasswordSalt", saltValue);
                        parameters.Add("@SecurityToken", Guid.NewGuid().ToString());

                        parameters.Add("@UpdatedBy", userId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    
                    if (user.PendingRegistration)
                    {
                        await PendingRegistrationSteps(userId);
                    }
                    return new ResponseModel { Message = "Your password has been changed.", Status = true };
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->ResetPasswordByUrlAsync", ex);
                throw;
            }
        }

        private static async Task PendingRegistrationSteps(long userId)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "UPDATE " + AppTable.Users + " SET  PendingRegistration=0,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", userId);

                    parameters.Add("@UpdatedBy", userId);
                    parameters.Add("@UpdatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    await con.ExecuteAsync(query, parameters);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, "ForgotPasswordService->PendingRegistrationSteps", ex);
                throw;
            }
        }

        public async Task<ResetPasswordModel> CheckResetPasswordKeyAsync(string Key)
        {
            try
            {
                Int32 UserName_PK = Convert.ToInt32(CommonUtility.DecryptUrl(Key));
                var user =  await _userService.GetUsersByIdAsync(UserName_PK);
                ResetPasswordModel _model = new ResetPasswordModel
                {
                    Email = user.EmailAddress
                };

                return _model;
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->ResetPasswordByUrlAsync", ex);
                throw;
            }
        }
    }
}
