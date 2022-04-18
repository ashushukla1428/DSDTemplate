using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IOrganizationsService
    { 
        Task<List<OrganizationModel>> GetOrganization();
        Task<ResponseModel> CreateOrganization(OrganizationModel model);
        Task<ResponseModel> UpdateOrganization(OrganizationModel model);
       
        Task<OrganizationModel> GetOrganizationsById(Int32 Id);
        Int32 GetFirstOrganizationsId();
        Task<List<SelectListItem>> BindOrganizationDropDown();
        Task<List<SelectListItem>> BindParentOrganizationDropDown();

        Task<List<VacancyPositionColorModel>> GetVacancyPositionColor();

        Task<ResponseModel> UpdateVacancyPositionColor(VacancyPositionColorModel model);

        Task<DefaultALMValue> GetDefaultALMValue();
    }
}
