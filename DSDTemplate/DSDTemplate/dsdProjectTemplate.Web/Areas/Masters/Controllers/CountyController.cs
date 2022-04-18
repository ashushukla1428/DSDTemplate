using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.County;
using dsdProjectTemplate.Services.State;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Counties;
using dsdProjectTemplate.ViewModel.State;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class CountyController : BaseController
    {
        private readonly ICountyService _countyService;
        private readonly IStateService _stateService;
        public CountyController()
        {
            _countyService = new CountyService();
            _stateService = new StateService();
        }
        // GET: County
        public async Task<ActionResult> Index()
        {
            ViewBag.StateList = await _stateService.GetDropStatsAsync();
            return View();
        }
        public async Task<ActionResult> GetStatesList()
        {
            try
            {

                return Json(await _stateService.GetDropStatsAsync(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _countyService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, CountiesResponse model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _countyService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;


                }
                else
                {

                    _baseResponse = await _countyService.UpdateAsync(model);
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