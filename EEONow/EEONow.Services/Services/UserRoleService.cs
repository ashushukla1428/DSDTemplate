using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEONow.Context;
using System.Configuration;
using System.Web.Mvc;
using EEONow.Context.EntityContext;
using System.Web;
using EEONow.Utilities;
using System.Data.Entity;

namespace EEONow.Services
{
    public class UserRoleService : IUserRolesService
    {
        private readonly EEONowEntity _context;

        public UserRoleService()
        {

            _context = new EEONowEntity();
        }

        //public async Task<ResponseModel> ActiveUserRole(int Id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ResponseModel> CreateUserRole(UserRoleModel model)
        {
            try
            {
                var UserRole = await _context.UserRoles.Where(x => x.Name.ToLower() == model.Name.ToLower() && x.Organization.OrganizationId == model.OrganizationId).FirstOrDefaultAsync();

                if (UserRole != null)
                {
                    return new ResponseModel { Message = "UserRole is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                UserRole UserRoleToInsert = new UserRole
                {
                    Name = model.Name,
                    Active = model.Active,
                    IsAdd = model.IsAdd,
                    IsFilter = model.IsFilter,
                    IsEdit = model.IsEdit,
                    Description = model.Description,
                    CreateUserId = _user,
                    UpdateUserId = _user,
                    Organization = await _context.Organizations.Where(e => e.OrganizationId == model.OrganizationId).FirstOrDefaultAsync(),
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now

                };
                //save in database
                _context.UserRoles.Add(UserRoleToInsert);
                await _context.SaveChangesAsync();

                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = UserRoleToInsert.RoleId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateUserRole", "UserRoleService.cs");
                throw;
            }
        }
        public async Task<List<UserRoleModel>> GetUserRoles()
        {
            try
            {
                var _UserRole = await _context.UserRoles.ToListAsync();

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                if (_Loginmodel.OrgId > 0)
                {
                    _UserRole = _UserRole.Where(e => e.Organization.OrganizationId == _Loginmodel.OrgId).ToList();
                }
           

                List<UserRoleModel> _lstModel = new List<UserRoleModel>();
                _lstModel.AddRange(_UserRole.Select(g => new UserRoleModel { Name = g.Name.ToString(), RoleId = g.RoleId, Active = g.Active, IsAdd = g.IsAdd, IsEdit = g.IsEdit, IsFilter = g.IsFilter, Description = g.Description, OrganizationId = g.Organization.OrganizationId }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUserRoles", "UserRoleService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindUserRoleDropDown(int? OrgId, bool? PublicUrl = false)
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _UserRole = await _context.UserRoles.ToListAsync();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                if (_Loginmodel.OrgId > 0)
                {
                    _UserRole = _UserRole.Where(e => e.Organization.OrganizationId == _Loginmodel.OrgId).ToList();
                }
                if (OrgId > 0)
                {
                    _UserRole = _UserRole.Where(e => e.Organization.OrganizationId == OrgId).ToList();
                }
                //remove PublicUrl;
                if (!PublicUrl.Value)
                {
                    _UserRole = _UserRole.Where(e => e.Name.ToLower() != ConfigurationManager.AppSettings["PublicUrlKey"].ToLower()).ToList();
                }
                var _ListUserRole = new List<SelectListItem>();
                _ListUserRole.AddRange(_UserRole.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.RoleId.ToString() }).ToList());
                return _ListUserRole;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindUserRoleDropDown", "UserRoleService.cs");
                throw;
            }
        }

        public async Task<UserRoleModel> GetUserRolesById(int Id)
        {
            try
            {
                var _UserRole = await _context.UserRoles.Where(x => x.RoleId == Id).FirstOrDefaultAsync();
                UserRoleModel _Model = new UserRoleModel
                { Name = _UserRole.Name.ToString(), RoleId = _UserRole.RoleId, IsAdd = _UserRole.IsAdd, IsEdit = _UserRole.IsEdit, IsFilter = _UserRole.IsFilter, Active = _UserRole.Active, Description = _UserRole.Description, OrganizationId = _UserRole.Organization.OrganizationId };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUserRolesById", "UserRoleService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateUserRole(UserRoleModel model)
        {
            try
            {
                var UserRole = await _context.UserRoles.Where(x => x.RoleId == model.RoleId).FirstOrDefaultAsync();
                if (UserRole != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    UserRole.Name = model.Name;
                    UserRole.Active = model.Active;
                    UserRole.Description = model.Description;
                    UserRole.UpdateUserId = _user;
                    UserRole.IsAdd = model.IsAdd;
                    UserRole.IsFilter = model.IsFilter;
                    UserRole.IsEdit = model.IsEdit;
                    UserRole.Organization = await _context.Organizations.Where(e => e.OrganizationId == model.OrganizationId).FirstOrDefaultAsync();
                    UserRole.UpdateDateTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.RoleId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateUserRole", "UserRoleService.cs");
                throw;
            }
        }
    }
}
