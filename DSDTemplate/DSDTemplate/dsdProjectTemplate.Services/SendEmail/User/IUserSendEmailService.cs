using dsdProjectTemplate.ViewModel;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.SendEmail.User
{
    public interface IUserSendEmailService
    {
        Task<bool> SendWelcomeMailAsync(long UserId, string Name, string ToEmailAddress, string userName);
        Task<ResponseModel> SendForgotPasswordMailAsync(string EmailAddress);
        Task<ResponseModel> SendForgotUserNameMailAsync(string EmailAddress);
        Task<ResponseModel> SendTwoFactorAuthenticationCodeEmailAsync(string code,long userId);
        Task<ResponseModel> SendOrg_UserActiveOrDeActiveNotificationAsync(long UserId,string code,string orgName);
    }
}
