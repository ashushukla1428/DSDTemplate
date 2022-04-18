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

namespace EEONow.Services
{
    public class MenuConfigurationService : IMenuConfigurationService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public MenuConfigurationService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public async Task<ResponseModel> CreateMenuConfiguration(MenuConfigurationModel model)
        {
            try
            {
                var MenuConfiguration = await _repository.FindAsync<MenuConfiguration>(x => x.MenuController.ToLower() == model.MenuController.ToLower() && x.MenuAction.ToLower() == model.MenuAction.ToLower());
                if (MenuConfiguration != null)
                {
                    return new ResponseModel { Message = "Menu is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int Create_User = Convert.ToInt32(_Loginmodel.UserId);
                MenuConfiguration MenuConfigurationToInsert = new MenuConfiguration
                {                   
                    MenuIcon="NA",
                    MenuKey= model.MenuController+"_"+model.MenuAction,
                    Name = model.Name,
                    MenuAction = model.MenuAction,
                    MenuController = model.MenuController,
                    IsActive = model.IsActive,
                    IsAdminOnly=model.IsAdminOnly,
                    SortOrder=model.SortOrder,
                    CreateUserId = 1,
                    UpdateUserId = 1,
                    CreateDateTime = DateTime.UtcNow,
                    UpdateDateTime = DateTime.UtcNow

                };
                //save in database
                int MenuConfigurationId = await _repository.Insert<MenuConfiguration>(MenuConfigurationToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = MenuConfigurationToInsert.MenuId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateMenuConfiguration", "MenuConfigurationService.cs");
                throw;
            }
        }

        public async Task<List<MenuConfigurationModel>> GetMenuConfiguration()
        {
            try
            {
                var _MenuConfiguration = await _repository.GetAllAsync<MenuConfiguration>();
                List<MenuConfigurationModel> _lstModel = new List<MenuConfigurationModel>();
                _lstModel.AddRange(_MenuConfiguration.Select(g => new MenuConfigurationModel {
                    Name = g.Name,
                    MenuID_PK = g.MenuId,
                    IsActive=g.IsActive.Value,
                    IsAdminOnly=g.IsAdminOnly,
                    SortOrder = g.SortOrder,
                    MenuController =g.MenuController,
                    MenuAction=g.MenuAction
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMenuConfiguration", "MenuConfigurationService.cs");
                throw;
            }
        }

        public async Task<MenuConfigurationModel> GetMenuConfigurationById(int Id)
        {
            try
            {
                var _MenuConfiguration = await _repository.FindAsync<MenuConfiguration>(x => x.MenuId == Id);
                MenuConfigurationModel _Model = new MenuConfigurationModel
                {
                    Name = _MenuConfiguration.Name,
                    MenuID_PK = _MenuConfiguration.MenuId,
                    IsActive = _MenuConfiguration.IsActive.Value,
                    IsAdminOnly=_MenuConfiguration.IsAdminOnly,
                    MenuController = _MenuConfiguration.MenuController,
                    MenuAction = _MenuConfiguration.MenuAction,
                    SortOrder = _MenuConfiguration.SortOrder

                };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMenuConfigurationById", "MenuConfigurationService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateMenuConfiguration(MenuConfigurationModel model)
        {
            try
            {
                var MenuConfiguration = await _repository.FindAsync<MenuConfiguration>(x => x.MenuId == model.MenuID_PK);
                if (MenuConfiguration != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    
                    MenuConfiguration.Name = model.Name;
                    MenuConfiguration.MenuAction = model.MenuAction;
                    MenuConfiguration.MenuController = model.MenuController;
                    MenuConfiguration.IsActive = model.IsActive;
                    MenuConfiguration.IsAdminOnly = model.IsAdminOnly;
                    MenuConfiguration.SortOrder = model.SortOrder;
                    MenuConfiguration.UpdateUserId = Convert.ToInt32(_Loginmodel.UserId); ;
                    MenuConfiguration.UpdateDateTime = DateTime.UtcNow;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.MenuID_PK };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateMenuConfiguration", "MenuConfigurationService.cs");
                throw;
            }
        }
    }
}
