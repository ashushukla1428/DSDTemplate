using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IUserRolesService
    { 
        Task<List<UserRoleModel>> GetUserRoles();
        Task<ResponseModel> CreateUserRole(UserRoleModel model);
        Task<ResponseModel> UpdateUserRole(UserRoleModel model);
       // Task<ResponseModel> ActiveUserRole(Int32 Id);
        Task<UserRoleModel> GetUserRolesById(Int32 Id);

        Task<List<SelectListItem>> BindUserRoleDropDown(int? OrgId,bool? PublicUrl= false);
        
    }
}
