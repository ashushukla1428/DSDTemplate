using dsdProjectTemplate.Web.Controllers;
using dsdProjectTemplate.Services.UserType;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.UserType;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{
    public class UsersTypeController : BaseController
    {
        private readonly IUserTypeService _userTypeService;

        public UsersTypeController()
        {
            _userTypeService = new UserTypeService();
        }
        // GET: UsersType
        public ActionResult Index()
        {
            return View();
        }

        #region
        public async Task<ActionResult> BindStates([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _userTypeService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, UserTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _userTypeService.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;

                }
                else
                {

                    _baseResponse = await _userTypeService.UpdateAsync(model);
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