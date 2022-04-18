using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Services.SendEmail.User;
using dsdProjectTemplate.Services.SMSService.APITwilio;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace dsdProjectTemplate.Services.User.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationService: ITwoFactorAuthenticationService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        IUserSendEmailService _userSendEmailService = new UserSendEmailService();
        //string _serviceFor = "Two Factor Authentication";
        IUserService _userService;
        public TwoFactorAuthenticationService()
        {
            _userService = new UserService();
        }

        public async Task<ResponseModel> AddUpdateEmail_TwoFactorAuthentication_Async( string code, Boolean EmailTwoFactorAuthentication)
        {

            try
            {
                if (HttpContext.Current.Session["Email_" + UserSession.Current.loggedIn_UserId.ToString()].ToString() == code)
                {
                    var editModel = await _userService.GetUsersByIdAsync(UserSession.Current.loggedIn_UserId);
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Users + " SET  EmailTwoFactorAuthentication=@EmailTwoFactorAuthentication,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

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
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }

                    HttpContext.Current.Session["Email_" + UserSession.Current.loggedIn_UserId.ToString()] = "";
                    return new ResponseModel { Message = "EMail two factor authentication has been successfully updated", Status = true };
                }
                else
                {
                    return new ResponseModel { Message = "Verification Code is invalid or already expired ", Status = false };
                }

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->AddUpdateMobileNumber_TwoFactorAuthentication_Async" + UserSession.Current.UserName, ex);
                return new ResponseModel { Message = "Verification Code is invalid or already expired ", Status = false };
            }
        }

        public async Task<ResponseModel> AddUpdateMobileNumber_TwoFactorAuthentication_Async(string code, Boolean SMSTwoFactorAuthentication)
        {
            try
            {
                if(HttpContext.Current.Session["Mobile_" + UserSession.Current.loggedIn_UserId.ToString()].ToString() == code)
                {
                    var editModel = await _userService.GetUsersByIdAsync(UserSession.Current.loggedIn_UserId);

                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Users + " SET  SMSTwoFactorAuthentication=@SMSTwoFactorAuthentication,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", UserSession.Current.loggedIn_UserId);
                        if (editModel.SMSTwoFactorAuthentication)
                        {
                            parameters.Add("@SMSTwoFactorAuthentication", false);
                        }
                        else
                        {
                            parameters.Add("@SMSTwoFactorAuthentication", true);
                        }
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }


                    HttpContext.Current.Session["Mobile_" + UserSession.Current.loggedIn_UserId.ToString()] = "";
                    return new ResponseModel { Message = "Mobile two factor authentication has been successfully updated", Status = true };
                }
                else
                {
                    return new ResponseModel { Message = "Verification Code is invalid or already expired ", Status = false };
                }
               
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->AddUpdateMobileNumber_TwoFactorAuthentication_Async" + UserSession.Current.UserName, ex);
                return new ResponseModel { Message = "Verification Code is invalid or already expired ", Status = false };
            }
        }
        public async Task<ResponseModel> SendEmail_TwoFactorAuthenticationCode_Async()
        {
            string code = RandomString(6);
            var _result =await _userSendEmailService.SendTwoFactorAuthenticationCodeEmailAsync(code, UserSession.Current.loggedIn_UserId);
            if (_result.Status)
            {
                HttpContext.Current.Session["Email_"+UserSession.Current.loggedIn_UserId.ToString()] = code;
            }
             
            return _result;

        }

        public async Task<ResponseModel> SendMobileNumber_TwoFactorAuthenticationCode_Async()
        {
            try
            {
                var _user = await _userService.GetUsersByIdAsync(UserSession.Current.loggedIn_UserId); 
                if (_user == null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectNotFound("user"), Status = false };
                }
                if (string.IsNullOrEmpty(_user.RegistrationCellPhoneNumber))
                {
                    return new ResponseModel { Message = ResponseMessages.ProvideMobile, Status = false };
                }
                APITwilioService _twilioService = new APITwilioService();
                string code = RandomString(6); 

               var _data = await _twilioService.SendSMSByNumber(_user.RegistrationCellPhoneNumber, "Two factor authentication code "+System.Environment.NewLine+ code);
                if (string.IsNullOrEmpty(_data.ErrorMessage))
                {
                    HttpContext.Current.Session["Mobile_" + UserSession.Current.loggedIn_UserId.ToString()] = code;

                    return new ResponseModel { Message = "Two factor authentication code has been sent to your mobile number", Status = true };
                }
                else
                {
                    return new ResponseModel { Message = _data.ErrorMessage, Status = false };
                }
            }
            catch (Exception)
            {

                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false  };
            }
        }

        public async Task<ResponseModel> SendTwoFactorAuthenticationCodeOnLogin_Async(long userId)
        {
            var _user = await _userService.GetUsersByIdAsync(userId);
            
            if (_user == null)
            {
                return new ResponseModel { Message = ResponseMessages.SubjectNotFound("user"), Status = false };
            }

            // Send Two FactorAuthentication Code OnLogin On Email or Mobile 
            string code = RandomString(6);
            bool _sentCodeOnSMS = false;
            string _errorMessageSMS = string.Empty;
            bool _sentCodeOnEmail = false;
            string _errorMessageEmail = string.Empty;
            if ((bool)_user.SMSTwoFactorAuthentication)
            {
                APITwilioService _twilioService = new APITwilioService();
                //send code on mobile
                var _data = await _twilioService.SendSMSByNumber(_user.RegistrationCellPhoneNumber, "Two factor authentication login code " + System.Environment.NewLine + code);
                try
                {
                    if (string.IsNullOrEmpty(_data.ErrorMessage))
                    {
                        _sentCodeOnSMS = true;
                    }
                    else
                    {
                        _errorMessageSMS = _data.ErrorMessage;
                         _sentCodeOnSMS = false;
                    }
                }
                catch (Exception)
                {

                }
            }
            if ((bool)_user.EmailTwoFactorAuthentication)
            {
                //send code on email
                var _result = await _userSendEmailService.SendTwoFactorAuthenticationCodeEmailAsync(code,userId);
                if (_result.Status)
                {
                    _sentCodeOnEmail = true;
                }
                else
                {
                    _errorMessageEmail = _result.Message;
                    _sentCodeOnEmail = false;
                }
            }

            if(_sentCodeOnEmail || _sentCodeOnSMS)
            {
                HttpContext.Current.Session["TwoFactorAuthenticationCode_OnLogin" + _user.UserName.ToString()] = code;
                return new ResponseModel { Message = "Two factor authentication code has been sent to your mobile number or email address", Status = true };
            }
            else
            {
                return new ResponseModel { Message = _errorMessageSMS+System.Environment.NewLine+ _errorMessageEmail, Status = false };
            }
        }

        private  string RandomString(int length)
        {
            //Random random = new Random();
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, length)
            //  .Select(s => s[random.Next(s.Length)]).ToArray());
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }
    }
}
