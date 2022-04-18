using EEONow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IRaceService
    {
        Task<List<RaceModel>> GetRaceModel();
        Task<ResponseModel> CreateRace(RaceModel _model);
        Task<ResponseModel> UpdateRace(RaceModel _model);
        Task<List<SelectListItem>> BindRaceDropDown(int? organizationID);
    }
}
