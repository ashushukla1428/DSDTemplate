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
using System.IO;

namespace EEONow.Services
{
    public class AssignRoleService : IAssignRoleService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public AssignRoleService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<SelectListItem>> BindMenuDropDown()
        {
            try
            {
                var _Menu = await _repository.GetAllAsync<MenuConfiguration>();
                var _ListMenu = new List<SelectListItem>();
                _ListMenu.AddRange(_Menu.Where(e => e.IsActive == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.MenuId.ToString() }).ToList());
                return _ListMenu;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindMenuDropDown", "AssignRoleService.cs");
                throw;
            }
        }

        //public async Task<List<AssignRoleModel>> GetAssignRoleModel()
        //{
        //    try { 
        //    var _Roles = await _context.UserRoles.Where(e => e.Active == true).ToListAsync();
        //    var _AssignRoles = await _context.AssignRoles.ToListAsync();
        //    var _MenuConfigurations = _context.MenuConfigurations.Where(e => e.IsActive == true);
        //    List<AssignRoleModel> _lstmodel = new List<AssignRoleModel>();
        //    foreach (var item in _Roles)
        //    {
        //        AssignRoleModel _model = new AssignRoleModel
        //        {
        //            RoleId = item.RoleId,
        //            RoleName = item.Name,
        //            MenuId = _AssignRoles.Where(e => e.UserRole.RoleId == item.RoleId).Select(e => e.MenuConfiguration.MenuId).ToList(),
        //            ListMenu = _AssignRoles.Where(e => e.UserRole.RoleId == item.RoleId).Select(e => new SelectListItem { Text = e.MenuConfiguration.Name, Value = e.MenuConfiguration.MenuId.ToString() }).ToList()
        //        };

        //        _lstmodel.Add(_model);
        //    }
        //    return _lstmodel;
        //    }
        //    catch (Exception ex)
        //    {
        //        AppUtility.LogMessage(ex, "GetAssignRoleModel", "AssignRoleService.cs");
        //        throw;
        //    }
        //}

        //public ResponseModel UpdateAssignRole(AssignRoleModel _model)
        //{
        //    try
        //    {
        //        var ExistAssignRole = _context.AssignRoles.Where(x => x.UserRole.RoleId == _model.RoleId).ToList();

        //        if (ExistAssignRole != null && ExistAssignRole.Count() > 0)
        //        {
        //            _context.AssignRoles.RemoveRange(ExistAssignRole);
        //        }
        //        LoginResponse _Loginmodel = AppUtility.DecryptCookie();
        //        int _user = Convert.ToInt32(_Loginmodel.UserId);

        //        List<AssignRole> _lstAssignRole = new List<AssignRole>();
        //        foreach (var item in _model.ListMenu)
        //        {
        //            int MenuConfigurationsID = Convert.ToInt32(item.Value);
        //            AssignRole AssignRoleToInsert = new AssignRole
        //            {
        //                UserRole = _context.UserRoles.Where(e => e.RoleId == _model.RoleId).FirstOrDefault(),
        //                MenuConfiguration = _context.MenuConfigurations.Where(e => e.MenuId == MenuConfigurationsID).FirstOrDefault(),
        //                CreateUserId = _user,
        //                UpdateUserId = _user,
        //                CreateDateTime = DateTime.Now,
        //                UpdateDateTime = DateTime.Now

        //            };
        //            _lstAssignRole.Add(AssignRoleToInsert);
        //        }
        //        //save in database
        //        var AssignRoleId = _context.AssignRoles.AddRange(_lstAssignRole);
        //        _context.SaveChanges();
        //        return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = 1 };
        //    }
        //    catch (Exception ex)
        //    {
        //        AppUtility.LogMessage(ex, "UpdateAssignRole", "AssignRoleService.cs");
        //        throw;
        //    }
        //}

        public async Task<List<ManageUserRoleModel>> GetManageUserModel()
        {

            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            List<User> _User = new List<User>();

            if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
            {
                _User = await _context.Users.Where(e => e.UserRole != null).ToListAsync();
            }
            else
            {
                int CurrentUserOrganizationId = Convert.ToInt32(_Loginmodel.OrgId);
                _User = await _context.Users.Where(e => e.UserRole != null && e.UserRole.Organization.OrganizationId == CurrentUserOrganizationId).ToListAsync();
            }

            List<ManageUserRoleModel> _lstModel = new List<ManageUserRoleModel>();
            try
            {
                _lstModel.AddRange(_User.Select(g => new ManageUserRoleModel
                {
                    FirstName = g.FirstName.ToString(),
                    LastName = g.LastName.ToString(),
                    MiddleName = g.MiddleName,
                    UserId = g.UserId,
                    IsActive = g.Active,
                    Email = g.Email,
                    RoleId = g.UserRole.RoleId,
                    OrganizationId = g.UserRole.Organization == null ? 0 : g.UserRole.Organization.OrganizationId,
                    OrganizationName = g.UserRole.Organization == null ? "" : g.UserRole.Organization.Name,
                    LastLoginDate= g.LastLoginDateTime==null?"Never Logged-IN":   g.LastLoginDateTime.Value.ToString("MMM dd yyyy hh:mm tt"),
                    DaysSinceLastLogin= g.LastLoginDateTime == null ? "Never Logged-IN" : Convert.ToInt32((DateTime.Now - g.LastLoginDateTime.Value).TotalDays).ToString()
                }
                                  ).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetManageUserModel", "AssignRoleService.cs");
                throw;
            }

            return _lstModel;
        }
        public async Task<ResponseModel> UpdateUserRole(ManageUserRoleModel _model)
        {
            try
            {
                var _User = await _repository.FindAsync<User>(x => x.UserId == _model.UserId);
                if (_User != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    _User.UserRole = await _repository.FindAsync<UserRole>(x => x.RoleId == _model.RoleId);
                    _User.FirstName = _model.FirstName.ToString();
                    _User.LastName = _model.LastName.ToString();
                    _User.Email = _model.Email.ToString();
                    _User.MiddleName = _model.MiddleName;
                    _User.Active = _model.IsActive;                   
                    _User.UpdateUserId = _user;
                    _User.UpdateDateTime = DateTime.Now;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.RoleId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateUserRole", "AssignRoleService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> CreateUserAccount(ManageUserRoleModel model)
        {
            try
            {
                //check if user with same email is already exists
                var user = await _repository.FindAsync<User>(x => x.Email == model.Email);
                if (user != null)
                {
                    return new ResponseModel { Message = "User with this email is already exists." };
                }
                User userToInsert = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    SecurityToken = Guid.NewGuid().ToString(),
                    UserRole = await _repository.FindAsync<UserRole>(x => x.RoleId == model.RoleId),
                    //Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == model.OrganizationId),
                    Active = true,
                    MiddleName = model.MiddleName,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now
                };
                //create password salt 
                userToInsert.PasswordSalt = AppUtility.CreateSalt();
                //generate random password using salt
                string Password = RandomPasswordString(10, false);
                userToInsert.Password = AppUtility.Encrypt(Password, userToInsert.PasswordSalt);
                //save in database
                int userId = await _repository.Insert<User>(userToInsert);
                string encryptedPassword = AppUtility.EncryptURl(userToInsert.UserId.ToString());
                //Send Email to User 
                string URL = ConfigurationManager.AppSettings["AppUrl"] + "/Account/SetPassword?key=" + encryptedPassword;
                string Subject = ConfigurationManager.AppSettings["RegisterEmailSubject"];
                String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/RegisterMessageTemplate.html").ToString();
                string EmailHTML = String.Format(RawHtml, URL);
                
                AppUtility.SendEmail(model.Email, EmailHTML, Subject);
                return new ResponseModel { Message = "User register successfully", Succeeded = true, Id = userId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "Register", "AccountService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> ReSendPasswordEmail(string Email)
        {
            var user = await _repository.FindAsync<User>(x => x.Email == Email);
            if (user != null)
            {
                string encryptedPassword = AppUtility.EncryptURl(user.UserId.ToString());
                //Send Email to User 
                string URL = ConfigurationManager.AppSettings["AppUrl"] + "/Account/SetPassword?key=" + encryptedPassword;
                string Subject = ConfigurationManager.AppSettings["RegisterEmailSubject"];
                String RawHtml = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"EmailTemplates/RegisterMessageTemplate.html").ToString();
                string EmailHTML = String.Format(RawHtml, URL);

                AppUtility.SendEmail(user.Email, EmailHTML, Subject);
                return new ResponseModel { Message = "Initial password Email sent successfully", Succeeded = true, Id = user.UserId };
            }
            return new ResponseModel { Message = "Error", Succeeded = true, Id = 0 };
        }
        private static string RandomPasswordString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
