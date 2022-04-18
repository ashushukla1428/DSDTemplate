using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IMenuHeaderService
    {
        Task<List<MenuHeaderModel>> GetMenuHeader(HeaderMenuSearchModel _HeaderMenuSearchModel);
        Task<ResponseModel> CreateMenuHeader(MenuHeaderModel model);
        Task<ResponseModel> UpdateMenuHeader(MenuHeaderModel model);
        Task<MenuHeaderModel> GetMenuHeaderById(Int32 Id);
        Task<List<SelectListItem>> BindMenuHeaderDropDown();
        Task<List<SelectListItem>> BindMenuDropDown();
    }
}
