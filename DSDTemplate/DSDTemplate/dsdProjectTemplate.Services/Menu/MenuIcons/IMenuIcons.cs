using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.MenuIcons;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu.MenuIcons
{
    public interface IMenuIcons
    {
        Task<ResponseModel> AddAsync(MenuIconsResponse request);
        Task<ResponseModel> UpdateAsync(MenuIconsResponse request);
        Task<IEnumerable<MenuIconsResponse>> GetAllAsync();
        Task<List<SelectListItem>> GetMenusIcons();

    }
}
