using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEONow.Context;
using EEONow.Utilities;
using System.Configuration;
using System.Web.Mvc;
using System.Web;
using EEONow.Context.EntityContext;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.Entity;

using System.Web.Routing;
using System.IO;
//using System.Web.Security;

namespace EEONow.Services
{
    public class AccountService : IAccountService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public AccountService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();

        }

        public async Task<LoginResponse> Login(LoginModel model)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new LoginResponse { Message = "Invalid user email" };
                }

                if (user.Active == false)
                {
                    return new LoginResponse { Message = "Account is no longer active" };
                }

                string decryptedPassword = AppUtility.Decrypt(user.Password, user.PasswordSalt);

                if (decryptedPassword.Trim() == model.Password.Trim())
                {
                    user.LastLoginDateTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    string _UserName = user.LastName + ", " + user.FirstName + " " + user.MiddleName;
                    if (user.UserRole == null)
                    {
                        return new LoginResponse { Message = "Success", Succeeded = true, Email = model.Email, UserId = user.UserId, Roles = "DefinedSoftwareAdministrator", OrgId = 0, UserRoleId = 0, IsAdd = true, IsEdit = true, IsFilter = true, UserName = _UserName };
                    }
                    else
                    {
                        return new LoginResponse { Message = "Success", Succeeded = true, Email = model.Email, UserId = user.UserId, Roles = user.UserRole.Name, OrgId = user.UserRole.Organization.OrganizationId, IsFilter = user.UserRole.IsFilter, IsEdit = user.UserRole.IsEdit, IsAdd = user.UserRole.IsAdd, UserRoleId = user.UserRole.RoleId, UserName = _UserName };
                    }

                }
                else
                {
                    return new LoginResponse { Message = "Invalid username or password" };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "Login", "AccountService.cs");
                throw;
            }
        }
        public async Task<BaseResponseModel> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new BaseResponseModel { Message = "Username not found." };
                }
                if (user.Active == false)
                {
                    return new BaseResponseModel { Message = "Account is no longer active" };
                }

                string encryptedPassword = AppUtility.EncryptURl(user.UserId.ToString());
                if (user.Email != null)
                {
                    string URL = ConfigurationManager.AppSettings["AppUrl"] + "/Account/ResetPassword?key=" + encryptedPassword;

                    var Body = "<b> Please click the following link to reset your password <b/> <br/><br/>" +
                               "<a href='" + URL + "'>Click here</a> "; ;

                    await Task.Run(() => AppUtility.SendEmail(user.Email, string.Format("{0}", Body), "Requested reset password Information from EEO Now"));


                    return new BaseResponseModel { Message = "Your information has been sent.", Succeeded = true };

                }
                else
                {
                    return new BaseResponseModel { Message = "No email address is linked with this account, please contact system administrator.", Succeeded = true };

                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ForgotPassword", "AccountService.cs");
                throw;
            }
        }

        public async Task<RegisterModel> BindRegisterModel()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _Organization = await _repository.GetAllAsync<Organization>();
                model.ListOrganization = new List<SelectListItem>();
                model.ListOrganization.AddRange(_Organization.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                model.OrganizationId = 0;
                return model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindRegisterModel", "AccountService.cs");
                throw;
            }
        }

        public List<MenuUIModel> BindMenuUi()
        {
            try
            {
                string baseUrl = ConfigurationManager.AppSettings["AppUrl"];
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    var _MenuHeader = _context.MenuConfigurations.Where(e => e.IsActive == true).OrderBy(e => e.SortOrder).ToList();
                    List<MenuUIModel> _defaultMenu = new List<MenuUIModel> {
                        new MenuUIModel
                        {
                            Name ="Software Administrator",
                            IsActive =true,
                            IsHeader =true,
                            MenuIcon ="fa fa-dashboard fa-lg",
                            MenuId =1,
                            MenuKey ="DefinedSoftwareAdministrator",
                            MenuUrl =  "#",
                            SortOrder =1,
                            InnerMenuList = _MenuHeader.Select(e=> new MenuUIModel{
                                 Name =e.Name,
                                IsActive =e.IsActive,
                                IsHeader =false,
                                MenuIcon =e.MenuIcon,
                                MenuId =e.MenuId,
                                MenuKey =e.MenuKey,
                                MenuUrl =   baseUrl + @"\" + e.MenuController + @"\" + e.MenuAction,
                                SortOrder =e.SortOrder,
                         }).OrderBy(x=>x.Name).ToList()
                        }
                    };

                    return _defaultMenu;
                }
                else
                {

                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    var Outeritem = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole).OrderBy(e => e.Name).FirstOrDefault();

                    List<MenuUIModel> _MainList = new List<MenuUIModel>();

                    var _MenuHeader = _context.MenuHeaderConfigurations.Where(e => e.IsActive == true).OrderBy(e => e.SortOrder).ToList();

                    if (Outeritem != null)
                    {
                        _MenuHeader = _MenuHeader.Where(e => e.IsActive == true && e.UserRole.RoleId == Outeritem.RoleId).OrderBy(e => e.SortOrder).ToList();

                    }
                    //Create Header
                    List<MenuUIModel> _HeaderList = new List<MenuUIModel>();

                    foreach (var item in _MenuHeader)
                    {
                        MenuUIModel _MenuHeaderUiModel = new MenuUIModel
                        {
                            MenuId = item.MenuHeaderID_PK,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsHeader = item.IsHeader,
                            MenuIcon = item.MenuIcon,
                            MenuKey = item.MenuKey,
                            SortOrder = item.SortOrder,
                            MenuUrl = item.IsHeader == true ? "#" : baseUrl + @"\" + item.MenuController + @"\" + item.MenuAction,
                            InnerMenuList = item.AssignMenuHeaders.Count() == 0 ? null : item.AssignMenuHeaders.Select(e => new MenuUIModel
                            {
                                MenuId = e.MenuConfiguration.MenuId,
                                Name = e.MenuConfiguration.Name,
                                IsActive = e.MenuConfiguration.IsActive,
                                MenuIcon = "#",
                                SortOrder = e.MenuConfiguration.SortOrder,
                                MenuKey = e.MenuConfiguration.MenuKey,
                                MenuUrl = baseUrl + @"\" + e.MenuConfiguration.MenuController + @"\" + e.MenuConfiguration.MenuAction
                            }).OrderBy(e => e.Name).OrderBy(e => e.SortOrder).ToList()
                        };
                        _HeaderList.Add(_MenuHeaderUiModel);
                    }

                    //MenuUIModel _MenuRoleUIModel = new MenuUIModel
                    //{
                    //    MenuId = 999999,
                    //    Name = Outeritem.Name,
                    //    IsActive = true,
                    //    IsHeader = true,
                    //    MenuIcon = "fa fa-gift fa-lg",
                    //    MenuKey = Outeritem.Name,
                    //    MenuUrl = "#",
                    //    SortOrder = Outeritem.RoleId,
                    //    InnerMenuList = _HeaderList.OrderBy(e => e.Name).OrderBy(e => e.SortOrder).OrderBy(e => e.IsHeader).ToList()
                    //};
                    _MainList.AddRange(_HeaderList.OrderBy(e => e.Name).OrderBy(e => e.SortOrder).OrderBy(e => e.IsHeader).ToList());
                    //}

                    return _MainList.OrderBy(e => e.Name).OrderBy(e => e.SortOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindMenuUi", "AccountService.cs");
                throw;
            }

        }

        public static bool ValidateUserRole(String _Controller, String _Action)
        {
            try
            {
                EEONowEntity _context = new EEONowEntity();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    return true;
                }
                List<Int32> _UserModel = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.RoleId).ToList();

                if (_UserModel != null)
                {
                    foreach (var _UserRoleID in _UserModel)
                    {
                        var MenuHeaderList = _context.MenuHeaderConfigurations.Where(e => e.UserRole.RoleId == _UserRoleID).ToList();

                        var IsMenuExist = MenuHeaderList.Where(e => e.MenuController == _Controller && e.MenuAction == _Action).ToList();
                        if (IsMenuExist.Count() > 0)
                        {
                            return true;
                        }
                        var _MenuHeaderSecton = MenuHeaderList.SelectMany(e => e.AssignMenuHeaders).ToList();
                        var _ValidatePage = _MenuHeaderSecton.Select(e => e.MenuConfiguration).ToList();
                        foreach (var item in _ValidatePage)
                        {
                            if (item.MenuController == _Controller && item.MenuAction == _Action)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ValidateUserRole", "AccountService.cs");
                throw;
            }

        }

        public int GetOrganisationID()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                if (user != null)
                {
                    if (user.UserRole == null)
                    {

                        return 0;
                    }
                    else
                    {
                        return user.UserRole.Organization.OrganizationId;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOrganisationID", "AccountService.cs");
                throw;
            }
        }
        public static int GetDashboardOrganisationID()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                EEONowEntity _context = new EEONowEntity();
                var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                if (user != null)
                {
                    if (user.UserRole != null)
                    {
                        return user.UserRole.Organization.OrganizationId;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    Int32 adminselectedOrgId = 0;
                    int _organization = 0;
                    if (AppUtility.GetOrgIdForAdminView().Length > 0)
                    {
                        adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                        _organization = _context.Organizations.Where(e => e.OrganizationId == adminselectedOrgId).Select(e => e.OrganizationId).FirstOrDefault();
                    }
                    return _organization;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDashboardOrganisationID", "AccountService.cs");
                throw;
            }
        }
        public static int GetFileSubmissionID(int OrganizationId, String filename)
        {
            try
            {
                //LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                //int _user = Convert.ToInt32(_Loginmodel.UserId);
                EEONowEntity _context = new EEONowEntity();
                var FileSubmissionId = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == OrganizationId && e.FileName.ToLower() == filename.ToLower()).Select(e => e.FileSubmission.FileSubmissionId).FirstOrDefault();
                return FileSubmissionId;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileSubmissionID", "AccountService.cs");
                throw;
            }
        }
        public static int GetFileSubmissionID(int OrganizationId)
        {
            try
            {
                //LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                //int _user = Convert.ToInt32(_Loginmodel.UserId);
                EEONowEntity _context = new EEONowEntity();
                var FileSubmissionId = _context.FileSubmissions.Where(e => e.Organization.OrganizationId == OrganizationId && e.FileSubmissionStatu.Status == "Validated")
                                       .Max(e => e.FileSubmissionId);
                return FileSubmissionId;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileSubmissionID", "AccountService.cs");
                throw;
            }
        }
        public static bool CheckIsGraphRender()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                EEONowEntity _context = new EEONowEntity();
                var user = _context.Users.Where(e => e.UserId == _user).FirstOrDefault();

                if (user != null)
                {
                    if (user.UserRole != null)
                    {
                        int orgID = user.UserRole.Organization.OrganizationId;
                        var graphCount = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == orgID && e.Status == true).FirstOrDefault();
                        if (graphCount != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CheckIsGraphRender", "AccountService.cs");
                throw;
            }

        }

        public async Task<BaseResponseModel> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                Int32 userId = Convert.ToInt32(AppUtility.DecryptUrl(model.ResetPasswordKey));
                var user = await _context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (model.ConfirmPassword != model.Password)
                {
                    return new BaseResponseModel { Message = "Password and confirm password not matched" };
                }
                if (user == null)
                {
                    return new BaseResponseModel { Message = "User Name not found." };
                }
                if (user.Active == false)
                {
                    return new BaseResponseModel { Message = "Account is De-Activated by administrator" };
                }
                if (user.Email.Trim().ToLower() != model.Email.Trim().ToLower())
                {
                    return new BaseResponseModel { Message = "User Name not found." };
                }
                user.SecurityToken = Guid.NewGuid().ToString();
                user.PasswordSalt = AppUtility.CreateSalt();
                //generate random password using salt
                user.Password = AppUtility.Encrypt(model.Password, user.PasswordSalt);
                await _context.SaveChangesAsync();
                return new BaseResponseModel { Message = "Your password has been changed.", Succeeded = true };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ForgotPassword", "AccountService.cs");
                throw;
            }
        }

        public async Task<ResetPasswordModel> ResetPasswordByUrl(string Key)
        {
            try
            {
                Int32 UserName_PK = Convert.ToInt32(AppUtility.DecryptUrl(Key));
                var user = await _context.Users.Where(x => x.UserId == UserName_PK).FirstOrDefaultAsync();
                ResetPasswordModel _model = new ResetPasswordModel
                {
                    Email = user.Email
                };

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ForgotPassword", "AccountService.cs");
                throw;
            }
        }

        public async Task<BaseResponseModel> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var user = await _repository.FindAsync<User>(x => x.UserId == _user);

                if (model.ConfirmPassword != model.NewPassword)
                {
                    return new BaseResponseModel { Message = "Password and confirm password not matched" };
                }
                if (user == null)
                {
                    return new BaseResponseModel { Message = "User Name not found." };
                }
                string decryptedPassword = AppUtility.Decrypt(user.Password, user.PasswordSalt);
                if (model.Password != decryptedPassword)
                {
                    return new BaseResponseModel { Message = "Old password not matched" };
                }

                user.SecurityToken = Guid.NewGuid().ToString();
                user.PasswordSalt = AppUtility.CreateSalt();
                //generate random password using salt
                user.Password = AppUtility.Encrypt(model.NewPassword, user.PasswordSalt);
                await _repository.SaveChangesAsync();
                //string encryptedPassword = AppUtility.Decrypt(user.Password, user.PasswordSalt);                

                return new BaseResponseModel { Message = "Your password has been changed.", Succeeded = true };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ChangePassword", "AccountService.cs");
                throw;
            }
        }

        public async Task<String> GetUserEmail()
        {
            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            int _user = Convert.ToInt32(_Loginmodel.UserId);
            var result = await _context.Users.Where(e => e.UserId == _user).FirstOrDefaultAsync();
            return result.Email;
        }
    }


}
