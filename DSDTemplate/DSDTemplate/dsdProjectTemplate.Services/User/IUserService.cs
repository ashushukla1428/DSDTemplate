using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User
{
    public interface IUserService
    {
        Task<ResponseModel> AddUserByAdminAsync(UserViewModel request);
        Task<ResponseModel> UpdateUserByAdminAsync(UserViewModel request);
        Task<ResponseModel> UserLinkWithOrganizationAsync(long OrganizationId,long UserId,int RoleId);
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserProfileViewModel> GetMyProfileAsync();
        Task<ResponseModel> UpdateMyProfileAsync(UserProfileViewModel request);
        Task<IEnumerable<Organizations_UsersList>> GetOrganizations_UsersAsync(long OrganizationId);
        Task<ResponseModel> ActiveOrDeActiveOrganizations_UserAsync(long Id);
        Task<UserViewModel> GetUsersByEamilAsync(string EmailAddress);
        Task<UserViewModel> GetUsersByUserNameAsync(string userName);
        Task<UserViewModel> GetUsersByIdAsync(long Id);
        Task<UserProfileViewModel> GetUsersProfileByIdAsync(long Id);

    }
}
