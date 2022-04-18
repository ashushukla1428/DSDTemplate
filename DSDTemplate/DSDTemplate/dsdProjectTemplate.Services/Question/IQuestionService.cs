using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Question;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Question
{
    public interface IQuestionService
    {
        Task<ResponseModel> AddAsync(QuestionViewModel request);
        Task<ResponseModel> UpdateAsync(QuestionViewModel request);
        
        Task<IEnumerable<QuestionViewModel>> GetAllAsync();
        Task<List<SelectListItem>> GetDropListAsync();
    }
}
