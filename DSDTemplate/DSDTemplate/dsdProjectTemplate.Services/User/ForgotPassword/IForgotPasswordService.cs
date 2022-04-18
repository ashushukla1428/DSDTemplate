using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.ForgotPassword
{
    public interface IForgotPasswordService
    {
        Task<ResetPasswordModel> CheckResetPasswordKeyAsync(string Key);
        Task<ResponseModel> ResetPasswordAsync(ResetPasswordModel request);
    }
}
