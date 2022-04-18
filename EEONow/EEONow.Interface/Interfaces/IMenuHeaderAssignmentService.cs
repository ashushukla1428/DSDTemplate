using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IMenuHeaderAssignmentService
    {
        Task<List<SelectListItem>> BindMenuDropDown();
        Task<List<MenuHeaderAssignmentModel>> GetMenuHeaderAssignmentModel();
        ResponseModel UpdateMenuHeaderAssignment(MenuHeaderAssignmentModel _model);       
    }
}
