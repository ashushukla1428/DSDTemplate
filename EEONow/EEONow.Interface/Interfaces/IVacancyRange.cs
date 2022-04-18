using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IVacancyRange
    {
        Task<List<VacancyRangeModel>> GetVacancyRangeModel();
        Task<ResponseModel> CreateVacancyRange(VacancyRangeModel _model);
        Task<ResponseModel> UpdateVacancyRange(VacancyRangeModel _model);
        Task<List<SelectListItem>> BindVacancyRangeDropDown(int? organizationID);
    }
}
