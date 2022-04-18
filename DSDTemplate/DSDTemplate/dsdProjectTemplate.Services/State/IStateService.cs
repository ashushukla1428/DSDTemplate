using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.State;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.State
{
    public interface IStateService
    {
        Task<ResponseModel> AddAsync(StateResponse request);
        Task<ResponseModel> UpdateAsync(StateResponse request);
        Task<IEnumerable<StateResponse>> GetAllAsync();
        Task<List<SelectListItem>> GetDropStatsAsync();
    }
}
