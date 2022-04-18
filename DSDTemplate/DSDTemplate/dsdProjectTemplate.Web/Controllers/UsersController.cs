using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.Services.User;
using dsdProjectTemplate.Services.User.RegistrationRequestType;
using dsdProjectTemplate.Services.User.UserAndOrganization;
using dsdProjectTemplate.Services.User.UsersRole;
using dsdProjectTemplate.Services.UserType;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User;
using dsdProjectTemplate.ViewModel.User.SearchFilter;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly IUserService _userService;
        private readonly IUsersRoleService _usersRoleService;
        private readonly IUserAndOrganizationService _userAndOrganizationService;
        private readonly IRegistrationRequestTypeService _registrationRequestTypeService;

        // GET: Users
        public UsersController()
        {
            _organizationService = new OrganizationService();
            _usersRoleService = new UsersRoleService();
            _userAndOrganizationService = new UserAndOrganizationService();
            _registrationRequestTypeService = new RegistrationRequestTypeService();
            _userService = new UserService();
        }
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
                var model = await _userService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _userService.AddUserByAdminAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _userService.UpdateUserByAdminAsync(model);
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

        #endregion User user
        #region User and Org functions 
        public async Task<ActionResult> UserAndOrganizations(long id)
        {
            ViewBag.OrganizationsList = await _organizationService.GetDropOrganizationsAsync();
            ViewBag.Roles = await _usersRoleService.GetDropListAsync(0, true);
            ViewBag.RequestType = await _registrationRequestTypeService.GetDropListAsync(0);
            ViewBag.UserId = id;
            return View("UserAndOrganizations");
        }
        public async Task<ActionResult> BindUserAndOrganizations([DataSourceRequest] DataSourceRequest request, UserAndOrganizationsSearch searchFilter)
        {
            try
            {
                var model = await _userAndOrganizationService.GetUserAndOrganizationListAsync(searchFilter.UserId);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveUserAndOrganization([DataSourceRequest] DataSourceRequest request, UserAndOrganizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {
                    _baseResponse = await _userAndOrganizationService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _userAndOrganizationService.UpdateAsync(model);
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