using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IAgencyYearsOfService
    {
        Task<List<AgencyYearsOfServiceModel>> GetAgencyYearsOfServiceModel();
        Task<ResponseModel> CreateAgencyYearsOfService(AgencyYearsOfServiceModel _model);
        Task<ResponseModel> UpdateAgencyYearsOfService(AgencyYearsOfServiceModel _model);
        Task<List<SelectListItem>> BindAgencyYearsOfServiceDropDown(int? organizationID);
    }
}
