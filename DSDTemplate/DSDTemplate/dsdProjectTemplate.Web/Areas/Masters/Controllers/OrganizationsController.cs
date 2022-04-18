using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.City;
using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.Services.State;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Organization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly IOrganizationService _organizationService;
        public OrganizationsController()
        {
            _cityService = new CityService();
            _stateService = new StateService();
            _organizationService = new OrganizationService();
        }
        // GET: Organization
        public async Task<ActionResult> Index()
        {
            ViewBag.StateList = await _stateService.GetDropStatsAsync();
            ViewBag.CityList = await _cityService.GetDropCitesAsync();
            ViewBag.OrganizationList = await _organizationService.GetDropOrganizationsAsync();
            return View();
        }
        public async Task<ActionResult> OrganizationList()
        {
            try
            {

                return Json(await _organizationService.GetDropOrganizationsAsync(), JsonRequestBehavior.AllowGet);
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
                var model = await _organizationService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, OrganizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _organizationService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                    if (!_baseResponse.Status)
                    {
                        ModelState.AddModelError("Error Message", _baseResponse.Message);

                    }

                    return Json(new[] { model }.ToDataSourceResult(request, ModelState));

                }
                else
                {

                    _baseResponse = await _organizationService.UpdateAsync(model);
                    model.Id = (int)_baseResponse.Id;
                    if (!_baseResponse.Status)
                    {
                        ModelState.AddModelError("Error Message", _baseResponse.Message);

                    }
                    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                }
            }
            else
            {
                ModelState.AddModelError("Error Message", ResponseMessages.requiredFields);
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
        }

        #endregion
    }
}