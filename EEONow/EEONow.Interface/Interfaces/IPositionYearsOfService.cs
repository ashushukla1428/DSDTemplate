using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IPositionYearsOfService
    {
        Task<List<PositionYearsOfServiceModel>> GetPositionYearsOfServiceModel();
        Task<ResponseModel> CreatePositionYearsOfService(PositionYearsOfServiceModel _model);
        Task<ResponseModel> UpdatePositionYearsOfService(PositionYearsOfServiceModel _model);
        Task<List<SelectListItem>> BindPositionYearsOfServiceDropDown(int? organizationID);
    }
}
