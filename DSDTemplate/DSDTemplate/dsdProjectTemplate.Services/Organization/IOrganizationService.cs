using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Organization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Organization
{
    public interface IOrganizationService
    {
        Task<ResponseModel> AddAsync(OrganizationViewModel request);
        Task<ResponseModel> UpdateAsync(OrganizationViewModel request);
        Task<IEnumerable<OrganizationViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropOrganizationsAsync();
        Task<List<SelectListItem>> GetDropOrganizationsForActionHistoryAsync(); 
    }
}
