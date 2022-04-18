using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Gender;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Gender
{
    public interface IGenderService
    {
        Task<ResponseModel> AddAsync(GenderViewModel request);
        Task<ResponseModel> UpdateAsync(GenderViewModel request);
        Task<IEnumerable<GenderViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropGendersTypeAsync();
    }
}
