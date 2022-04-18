using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.Question;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Question;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class SecurityQuestionsController : BaseController
    {
        // GET: SecurityQuestions
        private readonly IQuestionService _questionService;
        public SecurityQuestionsController()
        {
            _questionService = new QuestionService();
        }
        public ActionResult Index()
        {
            return View();
        }
        #region
        public async Task<ActionResult> BindStates([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _questionService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _questionService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _questionService.UpdateAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                if (!_baseResponse.Status)
                {
                    ModelState.AddModelError("error", _baseResponse.Message);

                }

                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
            else
            {
                ModelState.AddModelError("error", ResponseMessages.requiredFields);
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
        }

        #endregion
    }
}