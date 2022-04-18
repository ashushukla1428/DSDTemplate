using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface ICountiesService
    { 
        Task<List<CountyModel>> GetCounties();
        Task<ResponseModel> CreateCounty(CountyModel model);
        Task<ResponseModel> UpdateCounty(CountyModel model);
        
        Task<CountyModel> GetCountiesById(Int32 Id);

        Task<List<SelectListItem>> BindCountyDropDown();
        
    }
}
