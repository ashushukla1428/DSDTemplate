using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Clients.ClientsContactTypes
{
    public interface IClientsContactTypeService
    {
        Task<ResponseModel> AddAsync(ClientsContactTypeViewModel request);
        Task<ResponseModel> UpdateAsync(ClientsContactTypeViewModel request);
        Task<IEnumerable<ClientsContactTypeViewModel>> GetAllAsync(long organizationId);
        Task<List<SelectListItem>> GetDropClientsContactTypeAsync(long organizationId);
    }
}
