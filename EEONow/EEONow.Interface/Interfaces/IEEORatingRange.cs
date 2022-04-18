using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IEEORating
    {
    
        Task<List<EEORatingModel>> GetEEORatingModel();

        Task<ResponseModel> CreateEEORating(EEORatingModel _model);

        Task<ResponseModel> UpdateEEORating(EEORatingModel _model);

        Task<List<SelectListItem>> BindEEORatingDropDown();
        Task<List<SelectListItem>> BindEEORatingTypeDropDown();
        decimal GetBenchMarkValue(int OrganizationId);
        //
    }
}
