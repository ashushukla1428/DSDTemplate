using dsdProjectTemplate.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.AppActionsHistory
{
    public interface IActionsHistoryService
    {
        Task<bool> AddAsync(ActionsHistoryViewModel request);
        Task<IEnumerable<ActionsHistoryViewModel>> GetAllAsync(ActionsHistorySearch request);
        Task<ActionsHistoryViewModel> GetByIdAsync(long Id);
    }
}
