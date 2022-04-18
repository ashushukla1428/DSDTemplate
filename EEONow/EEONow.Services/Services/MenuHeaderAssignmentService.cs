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
    public class MenuHeaderAssignmentService : IMenuHeaderAssignmentService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public MenuHeaderAssignmentService()
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
                _ListMenu.AddRange(_Menu.Where(e => e.IsActive == true && e.IsAdminOnly == false).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.MenuId.ToString() }).ToList());
                return _ListMenu;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindMenuDropDown", "MenuHeaderAssignmentService.cs");
                throw;
            }
        }

        public async Task<List<MenuHeaderAssignmentModel>> GetMenuHeaderAssignmentModel()
        {
            try
            {
                var _AssignedMenuHeader = await _context.AssignMenuHeaders.ToListAsync();
                var _MenuHeaderAssignments = await _context.MenuHeaderConfigurations.Where(e => e.IsHeader == true).ToListAsync();
                //var _MenuConfigurations = _context.MenuConfigurations.Where(e => e.IsActive == true);
                List<MenuHeaderAssignmentModel> _lstmodel = new List<MenuHeaderAssignmentModel>();
                foreach (var item in _MenuHeaderAssignments)
                {
                    MenuHeaderAssignmentModel _model = new MenuHeaderAssignmentModel
                    {
                        MenuHeaderId = item.MenuHeaderID_PK,
                        MenuHeaderName = item.Name,
                        MenuId = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == item.MenuHeaderID_PK).Select(e => e.MenuConfiguration.MenuId).ToList(),
                        ListMenu = _AssignedMenuHeader.Where(e => e.MenuHeaderConfiguration.MenuHeaderID_PK == item.MenuHeaderID_PK).Select(e => new SelectListItem { Text = e.MenuConfiguration.Name, Value = e.MenuConfiguration.MenuId.ToString() }).ToList()
                    };
                    _lstmodel.Add(_model);
                }
                return _lstmodel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMenuHeaderAssignmentModel", "MenuHeaderAssignmentService.cs");
                throw;
            }
        }

        public ResponseModel UpdateMenuHeaderAssignment(MenuHeaderAssignmentModel _model)
        {
            try
            {
                var ExistMenuHeaderAssignment = _context.AssignMenuHeaders.Where(x => x.MenuHeaderConfiguration.MenuHeaderID_PK == _model.MenuHeaderId).ToList();

                if (ExistMenuHeaderAssignment != null && ExistMenuHeaderAssignment.Count() > 0)
                {
                    _context.AssignMenuHeaders.RemoveRange(ExistMenuHeaderAssignment);
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();


                List<AssignMenuHeader> _lstMenuHeaderAssignment = new List<AssignMenuHeader>();
                foreach (var item in _model.ListMenu)
                {
                    int MenuConfigurationsID = Convert.ToInt32(item.Value);
                    AssignMenuHeader MenuHeaderAssignmentToInsert = new AssignMenuHeader
                    {
                        MenuHeaderConfiguration = _context.MenuHeaderConfigurations.Where(e => e.MenuHeaderID_PK == _model.MenuHeaderId).FirstOrDefault(),
                        MenuConfiguration = _context.MenuConfigurations.Where(e => e.MenuId == MenuConfigurationsID).FirstOrDefault(),
                        Cre_User = _Loginmodel.UserId.ToString(),
                        Mod_User = _Loginmodel.UserId.ToString(),
                        Cre_Date = DateTime.UtcNow,
                        Mod_Date = DateTime.UtcNow

                    };
                    _lstMenuHeaderAssignment.Add(MenuHeaderAssignmentToInsert);
                }
                //save in database
                var MenuHeaderAssignmentId = _context.AssignMenuHeaders.AddRange(_lstMenuHeaderAssignment);
                _context.SaveChanges();
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = 1 };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateMenuHeaderAssignment", "MenuHeaderAssignmentService.cs");
                throw;
            }
        }
    }
}
