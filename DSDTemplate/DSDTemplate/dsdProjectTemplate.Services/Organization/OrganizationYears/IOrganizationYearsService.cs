using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Organization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Organization.OrganizationYears
{
    public interface IOrganizationYearsService
    {
        Task<ResponseModel> AddAsync(OrganizationYearsViewModel request);
        Task<ResponseModel> UpdateAsync(OrganizationYearsViewModel request);
        Task<IEnumerable<OrganizationYearsViewModel>> GetAllAsync(long organizationId);
        Task<List<SelectListItem>> GetDropOrganizationYearsAsync(long organizationId);
    }
}
