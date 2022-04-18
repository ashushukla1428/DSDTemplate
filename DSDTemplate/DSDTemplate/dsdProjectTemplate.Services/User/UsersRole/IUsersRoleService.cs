using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.User.UsersRole
{
    public interface IUsersRoleService
    {
        Task<ResponseModel> AddAsync(UsersRoleViewModel request);
        Task<ResponseModel> UpdateAsync(UsersRoleViewModel request);
        Task<IEnumerable<UsersRoleViewModel>> GetAllAsync(long organizationId);
        Task<List<SelectListItem>> GetDropListAsync(long organizationId, bool GetAll = false);
        Task<UsersRoleViewModel> GetByIdAsync(long Id);
    }
}
