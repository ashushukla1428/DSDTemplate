using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Counties;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.County
{
    public interface ICountyService
    {
        Task<ResponseModel> AddAsync(CountiesResponse request);
        Task<ResponseModel> UpdateAsync(CountiesResponse request);
        Task<IEnumerable<CountiesResponse>> GetAllAsync();
        Task<List<SelectListItem>> GetDropCountesAsync(int sateId);
    }
}
