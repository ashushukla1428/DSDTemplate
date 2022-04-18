using dsdProjectTemplate.Services.User;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.SendEmail.User
{
    public class UserSendEmailService : IUserSendEmailService
    {
        IUserService _userService;
        public UserSendEmailService()
        {
            _userService = new UserService();
        }
        public async Task<bool> SendWelcomeMailAsync(long UserId, string Name, string ToEmailAddress,string userName)
        {
            try
            {
                string encryptedPassword = CommonUtility.EncryptURl(UserId.ToString());
                string pageLink = ConfigurationManager.AppSettings["AppUrl"] + "/SetPassword?key=" + encryptedPassword;
                string htmlBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/RegisterMessageTemplate.html").ToString();
                htmlBody = htmlBody.Replace("@headerHTML", BaseEMailTemplates.GetHeader());
                htmlBody = htmlBody.Replace("@pageLink", pageLink);
                htmlBody = htmlBody.Replace("@name", Name);
                htmlBody = htmlBody.Replace("@userName", userName);
                htmlBody = htmlBody.Replace("@footerHTML", BaseEMailTemplates.GetFooter());
                htmlBody = htmlBody.Replace("@AppUrl", ConfigurationManager.AppSettings["AppUrl"]);//MUST WITHOUT THIS IMAGES AND OTHER LINKS WILL NOT WORK IN THE EMAIL
                MailUtility.SendEmail(ToEmailAddress, htmlBody, ConfigurationManager.AppSettings["RegisterEmailSubject"]);
                return true;

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendWelcomeMailAsync", ex);
                return  false;
            }
        }
        public async Task<ResponseModel> SendForgotPasswordMailAsync(string EmailAddress)
        {
            try
            {
                var _user = await _userService.GetUsersByEamilAsync( EmailAddress);
                if (_user == null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectNotFound(EmailAddress), Status = false };
                }
                string encryptedPassword = CommonUtility.EncryptURl(_user.Id.ToString());
                string pageLink = ConfigurationManager.AppSettings["AppUrl"] + "/SetPassword?key=" + encryptedPassword;
                string htmlBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/ForgotPassword.html").ToString();
                htmlBody = htmlBody.Replace("@headerHTML", BaseEMailTemplates.GetHeader());
                htmlBody = htmlBody.Replace("@pageLink", pageLink);
                htmlBody = htmlBody.Replace("@name", _user.FirstName);
                htmlBody = htmlBody.Replace("@footerHTML", BaseEMailTemplates.GetFooter());
                htmlBody = htmlBody.Replace("@AppUrl", ConfigurationManager.AppSettings["AppUrl"]);//MUST WITHOUT THIS IMAGES AND OTHER LINKS WILL NOT WORK IN THE EMAIL
                MailUtility.SendEmail(EmailAddress, htmlBody, ConfigurationManager.AppSettings["ForgotPasswordSubject"]);
                return new ResponseModel { Message = "password reset link has been sent to your email address", Status = true };

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendForgotPasswordMailAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false };
            }
        }
        public async Task<ResponseModel> SendForgotUserNameMailAsync(string EmailAddress)
        {
            try
            {
                var _user = await  _userService.GetUsersByEamilAsync(EmailAddress); 
                if (_user == null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectNotFound(EmailAddress), Status = false };
                }
                string encryptedPassword = CommonUtility.EncryptURl(_user.Id.ToString());
                string pageLink = ConfigurationManager.AppSettings["AppUrl"] + "/SetPassword?key=" + encryptedPassword;
                string htmlBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/ForgotUserName.html").ToString();
                htmlBody = htmlBody.Replace("@headerHTML", BaseEMailTemplates.GetHeader());
                htmlBody = htmlBody.Replace("@userName", _user.UserName);
                htmlBody = htmlBody.Replace("@pageLink", "login");
                htmlBody = htmlBody.Replace("@name", _user.FirstName);
                htmlBody = htmlBody.Replace("@footerHTML", BaseEMailTemplates.GetFooter());
                htmlBody = htmlBody.Replace("@AppUrl", ConfigurationManager.AppSettings["AppUrl"]);//MUST WITHOUT THIS IMAGES AND OTHER LINKS WILL NOT WORK IN THE EMAIL
                MailUtility.SendEmail(EmailAddress, htmlBody, ConfigurationManager.AppSettings["ForgotUserNameSubject"]);
                return new ResponseModel { Message = "Username has been sent to your email address", Status = true };

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendForgotUserNameMailAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false };
            }
        }

        public async Task<ResponseModel> SendTwoFactorAuthenticationCodeEmailAsync(string code,long userId)
        {

            try
            {
                var _user = await _userService.GetUsersByIdAsync(userId);
                if (_user == null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectNotFound("user"), Status = false };
                }
                if (string.IsNullOrEmpty(_user.EmailAddress))
                {
                    return new ResponseModel { Message = ResponseMessages.ProvideEmail, Status = false };
                }
                //  string encryptedPassword = CommonUtility.EncryptURl(_user.Id.ToString());
                //  string pageLink = ConfigurationManager.AppSettings["AppUrl"] + "/SetPassword?key=" + encryptedPassword;
                string htmlBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/TwoFactorAuthenticationCode.html").ToString();
                htmlBody = htmlBody.Replace("@headerHTML", BaseEMailTemplates.GetHeader());
                htmlBody = htmlBody.Replace("@code", code);
                //htmlBody = htmlBody.Replace("@pageLink", "login");
                htmlBody = htmlBody.Replace("@name", _user.FirstName);
                htmlBody = htmlBody.Replace("@footerHTML", BaseEMailTemplates.GetFooter());
               // htmlBody = htmlBody.Replace("@AppUrl", ConfigurationManager.AppSettings["AppUrl"]);//MUST WITHOUT THIS IMAGES AND OTHER LINKS WILL NOT WORK IN THE EMAIL
                MailUtility.SendEmail(_user.EmailAddress, htmlBody, ConfigurationManager.AppSettings["TwoFactorAuthenticationCode"]);
                return new ResponseModel { Message = "Two factor authentication code has been sent to your email address", Status = true };

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendForgotUserNameMailAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false };
            }
        }

        public async Task<ResponseModel> SendOrg_UserActiveOrDeActiveNotificationAsync(long UserId,string code, string orgName)
        {
            try
            {
                var _user =  await _userService.GetUsersByIdAsync(UserId); ;
                if (_user == null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectNotFound("user"), Status = false };
                }

 
                string htmlBody = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Content/EmailTemplates/Organizations_User_Active_De-ActiveNotification.html").ToString();
                htmlBody = htmlBody.Replace("@headerHTML", BaseEMailTemplates.GetHeader());
                htmlBody = htmlBody.Replace("@code", code);
                htmlBody = htmlBody.Replace("@orgName", orgName);
                htmlBody = htmlBody.Replace("@name", _user.FirstName);
                htmlBody = htmlBody.Replace("@footerHTML", BaseEMailTemplates.GetFooter());
                // htmlBody = htmlBody.Replace("@AppUrl", ConfigurationManager.AppSettings["AppUrl"]);//MUST WITHOUT THIS IMAGES AND OTHER LINKS WILL NOT WORK IN THE EMAIL
                MailUtility.SendEmail(_user.EmailAddress, htmlBody, "Your account has been " + code + " with organization " + orgName);
                return new ResponseModel { Message = "Two factor authentication code has been sent to your email address", Status = true };

            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->SendForgotUserNameMailAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false };
            }
        }
    }
}
