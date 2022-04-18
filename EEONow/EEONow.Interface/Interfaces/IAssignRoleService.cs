using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IAssignRoleService
    {
        Task<List<SelectListItem>> BindMenuDropDown();
        //Task<List<AssignRoleModel>> GetAssignRoleModel();
        //ResponseModel UpdateAssignRole(AssignRoleModel _model);
        Task<List<ManageUserRoleModel>> GetManageUserModel();

        Task<ResponseModel> UpdateUserRole(ManageUserRoleModel _model);
        Task<ResponseModel> CreateUserAccount(ManageUserRoleModel model);
        Task<ResponseModel> ReSendPasswordEmail(string Email);
        //
    }
}
