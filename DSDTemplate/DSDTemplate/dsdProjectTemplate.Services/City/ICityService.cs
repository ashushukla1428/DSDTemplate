using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.City;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.City
{
    public interface ICityService
    {
        Task<ResponseModel> AddAsync(CityViewModel request);
        Task<ResponseModel> UpdateAsync(CityViewModel request);
        Task<IEnumerable<CityViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropCitesAsync();
    }
}
