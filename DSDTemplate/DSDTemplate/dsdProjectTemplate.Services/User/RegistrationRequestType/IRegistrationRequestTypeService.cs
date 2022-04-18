using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User.Registration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.User.RegistrationRequestType
{
    public interface IRegistrationRequestTypeService
    {
        Task<ResponseModel> AddAsync(RegistrationRequestTypeViewModel request);
        Task<ResponseModel> UpdateAsync(RegistrationRequestTypeViewModel request);
        Task<IEnumerable<RegistrationRequestTypeViewModel>> GetAllAsync(long organizationId);
        Task<List<SelectListItem>> GetDropListAsync(long organizationId);
    }
}
