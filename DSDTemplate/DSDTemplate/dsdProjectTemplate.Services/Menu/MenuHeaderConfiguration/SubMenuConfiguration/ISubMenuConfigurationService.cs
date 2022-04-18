using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration.SubMenuConfiguration
{
    public interface ISubMenuConfigurationService
    {
        Task<ResponseModel> AddAsync(SubMenuConfigurationViewModel request);
        Task<ResponseModel> AddUpdateSubMenu(List<SelectListItem> listMenu,long id);
        Task<ResponseModel> UpdateAsync(SubMenuConfigurationViewModel request);
        Task<IEnumerable<SubMenuConfigurationViewModel>> GetAllAsync(HeaderConfigurationSearchRequest request);

        Task<List<SelectListItem>> GetAssignedUserSubMenu(long menuHeaderID);
        Task<List<SelectListItem>> GetDropListAsync();
    }
}
