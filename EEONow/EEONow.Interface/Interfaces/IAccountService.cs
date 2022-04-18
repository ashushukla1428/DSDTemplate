using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginModel model);
        Task<BaseResponseModel> ForgotPassword(ForgotPasswordModel model);
        Task<BaseResponseModel> ChangePassword(ChangePasswordModel model);
        Task<BaseResponseModel> ResetPassword(ResetPasswordModel model);
        Task<ResetPasswordModel> ResetPasswordByUrl(string Key);
        Task<RegisterModel> BindRegisterModel();
        List<MenuUIModel> BindMenuUi();
        Int32 GetOrganisationID();
        Task<String> GetUserEmail();
    }
}
