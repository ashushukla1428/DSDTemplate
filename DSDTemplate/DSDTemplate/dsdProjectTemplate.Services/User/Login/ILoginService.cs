using dsdProjectTemplate.ViewModel.User;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.Login
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(LoginViewModel request);
    }
}
