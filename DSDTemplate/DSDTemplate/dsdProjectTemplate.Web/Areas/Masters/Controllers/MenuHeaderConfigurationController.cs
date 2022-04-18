using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.Menu;
using dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration;
using dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration.SubMenuConfiguration;
using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.Services.User.UsersRole;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class MenuHeaderConfigurationController : BaseController
    {
        // GET: MenuHeaderConfiguration
        private readonly IMenuHeaderConfigurationService _menuHeaderConfigurationService;
        private readonly IOrganizationService _organizationService;
        private readonly IUsersRoleService _usersRoleService;
        private readonly IMenuConfigurationService _menuConfigurationService;
        private readonly ISubMenuConfigurationService _subMenuConfigurationService;
        public MenuHeaderConfigurationController()
        {
            _menuHeaderConfigurationService = new MenuHeaderConfigurationService();
            _organizationService = new OrganizationService();
            _usersRoleService = new UsersRoleService();
            _menuConfigurationService = new MenuConfigurationService();
            _subMenuConfigurationService = new SubMenuConfigurationService();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.OrganizationsList = await _organizationService.GetDropOrganizationsAsync();
            ViewBag.MenuList = await _menuConfigurationService.GetDropListAsync();
            ViewBag.RoleList = await _usersRoleService.GetDropListAsync(0, true);

            return View();
        }
        public async Task<ActionResult> SubMenuConfiguration(int id)
        {
            ViewBag.MenuList = await _menuConfigurationService.GetDropListAsync();
            ViewBag.MainMenuId = id;
            return View("_SubMenuConfiguration");
        }
        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request, HeaderConfigurationSearchRequest searchFilter)
        {
            try
            {
                var model = await _menuHeaderConfigurationService.GetAllAsync(searchFilter);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, MenuHeaderConfigurationViewModel model)
        {
            if (model.MainMenuId == 0 && (model.ListMenu == null || model.ListMenu.Count == 0))
            {
                //ModelState.AddModelError("Error Message", "Please select at least one Menu item");
                ModelState.AddModelError("Error Message", "Please select Menu Page");
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
            if (ModelState.IsValid)
            {               
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _menuHeaderConfigurationService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _menuHeaderConfigurationService.UpdateAsync(model);
                    model.Id = (int)_baseResponse.Id;

                }
                if (!_baseResponse.Status)
                {
                    ModelState.AddModelError("error", _baseResponse.Message);

                }
                _baseResponse = null;
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
            else
            {
                ModelState.AddModelError("error", ResponseMessages.requiredFields);
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }
        }

        #endregion
        #region sub menus
        public async Task<ActionResult> BindSubMenuData([DataSourceRequest] DataSourceRequest request, HeaderConfigurationSearchRequest searchFilter)
        {
            try
            {
                var model = await _subMenuConfigurationService.GetAllAsync(searchFilter);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveSubMenu([DataSourceRequest] DataSourceRequest request, SubMenuConfigurationViewModel model)
        {

            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _subMenuConfigurationService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _subMenuConfigurationService.UpdateAsync(model);
                    model.Id = (int)_baseResponse.Id;

                }
                if (!_baseResponse.Status)
                {
                    ModelState.AddModelError("error", _baseResponse.Message);

                }
                _baseResponse = null;
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