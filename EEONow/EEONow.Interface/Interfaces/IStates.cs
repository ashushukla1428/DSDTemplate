using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IStatesService
    { 
        Task<List<StateModel>> GetStates();
        Task<ResponseModel> CreateState(StateModel model);
        Task<ResponseModel> UpdateState(StateModel model);
      
        Task<StateModel> GetStatesById(Int32 Id);

        Task<List<SelectListItem>> BindStateDropDown();
        
    }
}
