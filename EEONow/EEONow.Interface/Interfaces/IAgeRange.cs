using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IAgeRange
    {
        Task<List<AgeRangeModel>> GetAgeRangeModel();
        Task<ResponseModel> CreateAgeRange(AgeRangeModel _model);
        Task<ResponseModel> UpdateAgeRange(AgeRangeModel _model);
        Task<List<SelectListItem>> BindAgeRangeDropDown(int? organizationID);
    }
}
