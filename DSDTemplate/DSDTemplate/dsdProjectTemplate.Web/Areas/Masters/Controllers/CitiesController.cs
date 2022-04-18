using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.City;
using dsdProjectTemplate.Services.State;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.City;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class CitiesController : BaseController
    {
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        public CitiesController()
        {
            _cityService = new CityService();
            _stateService = new StateService();
        }
        // GET: Cities
        public async Task<ActionResult> Index()
        {
            ViewBag.StateList = await _stateService.GetDropStatsAsync();
            return View();
        }

        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _cityService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _cityService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _cityService.UpdateAsync(model);
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