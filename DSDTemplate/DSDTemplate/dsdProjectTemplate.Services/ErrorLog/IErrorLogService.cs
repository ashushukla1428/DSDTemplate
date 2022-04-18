using dsdProjectTemplate.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.ErrorLog
{
    public interface IErrorLogService
    {
        Task<IEnumerable<ErrorLogViewModel>> GetAllAsync();
    }
}
