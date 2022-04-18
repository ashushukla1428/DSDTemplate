using dsdProjectTemplate.Services.User;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Controllers
{
    public class OrganizationUsersController : BaseController
    {
     
        private readonly IUserService _userService;
           // GET: Users
        public OrganizationUsersController()
        {
          
            _userService = new UserService();
        }
        // GET: OrganizationUsers
        public ActionResult Index()
        {
            return View();
        }
        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _userService.GetOrganizations_UsersAsync(0);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ActiveOrDeActiveOrganizations_UserAsync(long Id)
        {
            return Json(await _userService.ActiveOrDeActiveOrganizations_UserAsync(Id));
        }

            #endregion User user
        }
}