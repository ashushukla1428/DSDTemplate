using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface ISalaryRange
    {
        Task<List<SalaryRangeModel>> GetSalaryRangeModel();
        Task<ResponseModel> CreateSalaryRange(SalaryRangeModel _model);
        Task<ResponseModel> UpdateSalaryRange(SalaryRangeModel _model);
        Task<List<SelectListItem>> BindSalaryRangeDropDown(int? organizationID);
    }
}
