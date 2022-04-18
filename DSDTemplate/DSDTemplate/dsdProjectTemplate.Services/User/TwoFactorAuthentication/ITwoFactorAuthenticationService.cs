using dsdProjectTemplate.ViewModel;
using System;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.TwoFactorAuthentication
{
    public interface ITwoFactorAuthenticationService
    {
        Task<ResponseModel> SendEmail_TwoFactorAuthenticationCode_Async();
        Task<ResponseModel> SendMobileNumber_TwoFactorAuthenticationCode_Async();
        Task<ResponseModel> AddUpdateEmail_TwoFactorAuthentication_Async(string code,Boolean EmailTwoFactorAuthentication);
        Task<ResponseModel> AddUpdateMobileNumber_TwoFactorAuthentication_Async(string code,Boolean SMSTwoFactorAuthentication);
        Task<ResponseModel> SendTwoFactorAuthenticationCodeOnLogin_Async(long userId);
    }
}
