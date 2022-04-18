using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Providers.ProvidersContactTypes
{
    public interface IProvidersContactTypesService
    {
        Task<ResponseModel> AddAsync(ProvidersContactTypesViewModel request);
        Task<ResponseModel> UpdateAsync(ProvidersContactTypesViewModel request);
        Task<IEnumerable<ProvidersContactTypesViewModel>> GetAllAsync(long organizationId);
        Task<List<SelectListItem>> GetDropProvidersContactTypesAsync(long organizationId);
    }
}
