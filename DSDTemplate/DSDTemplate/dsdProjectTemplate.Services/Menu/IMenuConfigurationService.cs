using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu
{
    public interface IMenuConfigurationService
    {
        Task<ResponseModel> AddAsync(MenuConfigurationViewModel request);
        Task<ResponseModel> UpdateAsync(MenuConfigurationViewModel request);
        Task<IEnumerable<MenuConfigurationViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropListAsync();
        Task<MenuConfigurationViewModel> GetByIdAsync(long? Id);
        Task<List<SelectListItem>> GetAllAppAreaMasterAsync();
    }
}
