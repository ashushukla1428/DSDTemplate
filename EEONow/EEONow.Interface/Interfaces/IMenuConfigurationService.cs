using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IMenuConfigurationService
    {
        Task<List<MenuConfigurationModel>> GetMenuConfiguration();
        Task<ResponseModel> CreateMenuConfiguration(MenuConfigurationModel model);
        Task<ResponseModel> UpdateMenuConfiguration(MenuConfigurationModel model);
        Task<MenuConfigurationModel> GetMenuConfigurationById(Int32 Id);
        //Task<List<SelectListItem>> BindMenuConfigurationDropDown();
    }
}
