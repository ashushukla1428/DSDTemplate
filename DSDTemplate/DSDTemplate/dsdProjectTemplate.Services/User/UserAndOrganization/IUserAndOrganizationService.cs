using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.User.UserAndOrganization
{
    public interface IUserAndOrganizationService
    {
        Task<ResponseModel> AddAsync(UserAndOrganizationViewModel request);
        Task<ResponseModel> UpdateAsync(UserAndOrganizationViewModel request);
        Task<IEnumerable<UserAndOrganizationViewModel>> GetUserAndOrganizationListAsync(long userId);

    }
}
