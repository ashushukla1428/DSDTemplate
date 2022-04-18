using dsdProjectTemplate.ViewModel.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.Menu.UserMenu
{
    public interface IUserMenuService
    {
        List<UserMenuList> GetUserMenusAsync();
        IEnumerable<UserMenuList> GetSuperAdminMenusAsync();
    }
}
