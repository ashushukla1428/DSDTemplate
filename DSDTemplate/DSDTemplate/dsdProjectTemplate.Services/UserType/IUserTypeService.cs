using dsdProjectTemplate.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using dsdProjectTemplate.ViewModel.UserType;
namespace dsdProjectTemplate.Services.UserType
{
    public interface IUserTypeService
    {
        Task<ResponseModel> AddAsync(UserTypeViewModel request);
        Task<ResponseModel> UpdateAsync(UserTypeViewModel request);
        Task<IEnumerable<UserTypeViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropUsersTypeAsync();
    }
}
