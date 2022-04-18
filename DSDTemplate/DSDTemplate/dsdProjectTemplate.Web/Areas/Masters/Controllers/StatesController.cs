using dsdProjectTemplate.Services.State;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.State;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class StatesController : Controller
    {
        private readonly IStateService _stateService;
        public StatesController()
        {
            _stateService = new StateService();
        }
        // GET: States
        public ActionResult Index()
        {
            return View();
        }
        #region
        public async Task<ActionResult> BindStates([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _stateService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, StateResponse model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _stateService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _stateService.UpdateAsync(model);
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