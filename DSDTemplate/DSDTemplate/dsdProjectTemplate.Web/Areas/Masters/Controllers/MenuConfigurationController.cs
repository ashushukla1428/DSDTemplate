using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.Menu;
using dsdProjectTemplate.Services.Menu.MenuIcons;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class MenuConfigurationController : BaseController
    {
        // GET: MenuConfiguration
        private readonly IMenuConfigurationService _menuConfigurationService;
        private readonly IMenuIcons _menuIcons;
        public MenuConfigurationController()
        {
            _menuConfigurationService = new MenuConfigurationService();
            _menuIcons = new MenuIcons();
        }

        // GET: States
        public async Task<ActionResult> Index()
        {
            ViewBag.MenuIcons = await _menuIcons.GetMenusIcons();
            return View();
        }
        public ActionResult GetOrganizations()
        {
            try
            {
                MenuIcons obj = new MenuIcons();
                return Json(obj.GetMenusIcons(), JsonRequestBehavior.AllowGet);
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
                var model = await _menuConfigurationService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, MenuConfigurationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _menuConfigurationService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _menuConfigurationService.UpdateAsync(model);
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