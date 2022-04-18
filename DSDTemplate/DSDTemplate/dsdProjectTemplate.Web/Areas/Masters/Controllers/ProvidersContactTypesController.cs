using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.Services.Providers.ProvidersContactTypes;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Providers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class ProvidersContactTypesController : BaseController
    {
        // GET: ProvidersContactTypes
        private readonly IProvidersContactTypesService _providersContactTypesService;
        private readonly IOrganizationService _organizationService;

        public ProvidersContactTypesController()
        {
            _organizationService = new OrganizationService();
            _providersContactTypesService = new ProvidersContactTypesService();
        }
        // GET: Cities
        public async Task<ActionResult> Index()
        {
            ViewBag.OrganizationsList = await _organizationService.GetDropOrganizationsAsync();
            return View();
        }
        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _providersContactTypesService.GetAllAsync(0);

                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, ProvidersContactTypesViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _providersContactTypesService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _providersContactTypesService.UpdateAsync(model);
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