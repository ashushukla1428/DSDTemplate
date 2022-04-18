using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration
{
    public interface IMenuHeaderConfigurationService
    {
        Task<ResponseModel> AddAsync(MenuHeaderConfigurationViewModel request);
        Task<ResponseModel> UpdateAsync(MenuHeaderConfigurationViewModel request);
        Task<IEnumerable<MenuHeaderConfigurationViewModel>> GetAllAsync(HeaderConfigurationSearchRequest request);
        Task<List<SelectListItem>> GetDropListAsync();
    }
}
