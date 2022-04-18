
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
using System.Text.RegularExpressions;

namespace EEONow.Services
{
    public class MenuHeaderService : IMenuHeaderService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public MenuHeaderService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public async Task<ResponseModel> CreateMenuHeader(MenuHeaderModel model)
        {
            try
            {
                var MenuHeader = await _context.MenuHeaderConfigurations.Where(x => x.Name.ToLower() == model.Name.ToLower() && x.UserRole.RoleId == model.RoleId).FirstOrDefaultAsync();
                if (MenuHeader != null)
                {
                    return new ResponseModel { Message = "Menu Header is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();


                // Menu And Menu header links Start                

                List<AssignMenuHeader> _lstMenuHeaderAssignment = new List<AssignMenuHeader>();
                if (model.ListMenu != null)
                {
                    foreach (var item in model.ListMenu)
                    {
                        int MenuConfigurationsID = Convert.ToInt32(item.Value);
                        AssignMenuHeader MenuHeaderAssignmentToInsert = new AssignMenuHeader
                        {
                            MenuConfiguration = _context.MenuConfigurations.Where(e => e.MenuId == MenuConfigurationsID).FirstOrDefault(),
                            Cre_User = _Loginmodel.UserId.ToString(),
                            Mod_User = _Loginmodel.UserId.ToString(),
                            Cre_Date = DateTime.UtcNow,
                            Mod_Date = DateTime.UtcNow

                        };
                        _lstMenuHeaderAssignment.Add(MenuHeaderAssignmentToInsert);
                    }

                }
                var role = await _context.UserRoles.Where(x => x.RoleId == model.RoleId).FirstOrDefaultAsync();
                //Menu And Menu header links Start
                MenuHeaderConfiguration MenuHeaderToInsert = new MenuHeaderConfiguration
                {
                    Name = model.Name,
                    UserRole = role,
                    IsHeader = model.IsHeader,
                    MenuKey = Regex.Replace(role.Name + model.Name + Convert.ToString(model.MenuController) + Convert.ToString(model.MenuAction), @"\s+", ""),
                    MenuAction = model.MenuAction,
                    MenuController = model.MenuController,
                    MenuIcon = "fa fa-gift fa-lg",
                    SortOrder = model.SortOrder,
                    IsActive = model.IsActive,
                    AssignMenuHeaders = _lstMenuHeaderAssignment,
                    Cre_User = _Loginmodel.UserId.ToString(),
                    Mod_User = _Loginmodel.UserId.ToString(),
                    Cre_Date = DateTime.UtcNow,
                    Mod_Date = DateTime.UtcNow
                };
                //save in database
                var MenuHeaderId = _context.MenuHeaderConfigurations.Add(MenuHeaderToInsert);
                await _context.SaveChangesAsync();
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = MenuHeaderToInsert.MenuHeaderID_PK };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateMenuHeader", "MenuHeaderService.cs");
                throw;
            }
        }

        public async Task<List<MenuHeaderModel>> GetMenuHeader(HeaderMenuSearchModel _HeaderMenuSearchModel)
        {
            try
            {
                var _AssignedMenuHeader = await _context.AssignMenuHeaders.ToListAsync();
                var _MenuHeader = await _context.MenuHeaderConfigurations.Where(e => e.UserRole.Organization.OrganizationId == _HeaderMenuSearchModel.organizationId && e.UserRole.RoleId == (_HeaderMenuSearchModel.FilterRoleId > 0 ? _HeaderMenuSearchModel.FilterRoleId : e.UserRole.RoleId)).ToListAsync();
                List<MenuHeaderModel> _lstModel = new List<MenuHeaderModel>();
                _lstModel.AddRange(_MenuHeader.Select(g => new MenuHeaderModel
                {
                    MenuHeaderID_PK = g.MenuHeaderID_PK,
                    RoleId = g.UserRole.RoleId,
                    //MenuKey = g.MenuKey,
                    Name = g.Name,
                    OrganizationId = g.UserRole.Organization.OrganizationId,
                    MenuController = g.MenuController,
                    MenuAction = g.MenuAction,
                    SortOrder = g.SortOrder,
                    MenuIcon = g.MenuIcon,
                    IsHeader = g.IsHeader == null ? false : g.IsHeader.Value,
                    IsActive = g.IsActive == null ? false : g.IsActive.Value,
                    MenuId = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == g.MenuHeaderID_PK).Select(e => e.MenuConfiguration.MenuId).ToList(),
                    ListMenu = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == g.MenuHeaderID_PK).Select(e => new SelectListItem { Text = e.MenuConfiguration.Name, Value = e.MenuConfiguration.MenuId.ToString() }).ToList()
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMenuHeader", "MenuHeaderService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindMenuHeaderDropDown()
        {
            try
            {
                var _MenuHeader = await _repository.GetAllAsync<MenuHeaderConfiguration>();
                var _ListMenuHeader = new List<SelectListItem>();
                _ListMenuHeader.AddRange(_MenuHeader.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.MenuHeaderID_PK.ToString() }).OrderBy(e => e.Text).ToList());
                return _ListMenuHeader;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindMenuHeaderDropDown", "MenuHeaderService.cs");
                throw;
            }
        }

        public async Task<MenuHeaderModel> GetMenuHeaderById(int Id)
        {
            try
            {
                var _AssignedMenuHeader = await _context.AssignMenuHeaders.ToListAsync();
                var _MenuHeader = await _context.MenuHeaderConfigurations.Where(x => x.MenuHeaderID_PK == Id).FirstOrDefaultAsync();
                MenuHeaderModel _Model = new MenuHeaderModel
                {
                    MenuHeaderID_PK = _MenuHeader.MenuHeaderID_PK,
                    RoleId = _MenuHeader.UserRole.RoleId,
                    //MenuKey = _MenuHeader.MenuKey,
                    SortOrder = _MenuHeader.SortOrder,
                    Name = _MenuHeader.Name,
                    MenuController = _MenuHeader.MenuController,
                    MenuAction = _MenuHeader.MenuAction,
                    MenuIcon = _MenuHeader.MenuIcon,
                    IsHeader = _MenuHeader.IsHeader == null ? false : _MenuHeader.IsHeader.Value,
                    IsActive = _MenuHeader.IsActive == null ? false : _MenuHeader.IsActive.Value,
                    MenuId = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == _MenuHeader.MenuHeaderID_PK).Select(e => e.MenuConfiguration.MenuId).ToList(),
                    ListMenu = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == _MenuHeader.MenuHeaderID_PK).Select(e => new SelectListItem { Text = e.MenuConfiguration.Name, Value = e.MenuConfiguration.MenuId.ToString() }).ToList()

                };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMenuHeaderById", "MenuHeaderService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateMenuHeader(MenuHeaderModel model)
        {
            try
            {
                var MenuHeader = await _context.MenuHeaderConfigurations.Where(x => x.MenuHeaderID_PK == model.MenuHeaderID_PK).FirstOrDefaultAsync();
                if (MenuHeader != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();



                    var ExistMenuHeaderAssignment = _context.AssignMenuHeaders.Where(x => x.MenuHeaderConfiguration.MenuHeaderID_PK == model.MenuHeaderID_PK).ToList();

                    if (ExistMenuHeaderAssignment != null && ExistMenuHeaderAssignment.Count() > 0)
                    {
                        _context.AssignMenuHeaders.RemoveRange(ExistMenuHeaderAssignment);
                    }
                    List<AssignMenuHeader> _lstMenuHeaderAssignment = new List<AssignMenuHeader>();

                    if (model.ListMenu != null)
                    {
                        foreach (var item in model.ListMenu)
                        {
                            int MenuConfigurationsID = Convert.ToInt32(item.Value);
                            AssignMenuHeader MenuHeaderAssignmentToInsert = new AssignMenuHeader
                            {
                                MenuConfiguration = _context.MenuConfigurations.Where(e => e.MenuId == MenuConfigurationsID).FirstOrDefault(),
                                Cre_User = _Loginmodel.UserId.ToString(),
                                Mod_User = _Loginmodel.UserId.ToString(),
                                Cre_Date = DateTime.UtcNow,
                                Mod_Date = DateTime.UtcNow

                            };
                            _lstMenuHeaderAssignment.Add(MenuHeaderAssignmentToInsert);
                        }
                    }

                    MenuHeader.Name = model.Name;
                    MenuHeader.UserRole = await _context.UserRoles.Where(x => x.RoleId == model.RoleId).FirstOrDefaultAsync();
                    MenuHeader.IsHeader = model.IsHeader;
                    // MenuHeader.MenuKey = model.MenuKey;
                    MenuHeader.SortOrder = model.SortOrder;
                    MenuHeader.AssignMenuHeaders = _lstMenuHeaderAssignment;
                    MenuHeader.MenuAction = model.MenuAction;
                    MenuHeader.MenuController = model.MenuController;
                    MenuHeader.MenuIcon = "fa fa-gift fa-lg";
                    MenuHeader.IsActive = model.IsActive;
                    MenuHeader.Mod_User = _Loginmodel.UserId.ToString();
                    MenuHeader.Mod_Date = DateTime.UtcNow;
                    MenuHeader.Mod_User = _Loginmodel.UserId.ToString();
                    MenuHeader.Mod_Date = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.MenuHeaderID_PK };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateMenuHeader", "MenuHeaderService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindMenuDropDown()
        {
            try
            {
                var _Menu = await _repository.GetAllAsync<MenuConfiguration>();
                var _ListMenu = new List<SelectListItem>();
                _ListMenu.AddRange(_Menu.Where(e => e.IsActive == true && e.IsAdminOnly == false).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.MenuId.ToString() }).ToList());
                return _ListMenu;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindMenuDropDown", "MenuHeaderAssignmentService.cs");
                throw;
            }
        }
    }
}
