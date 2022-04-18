using dsdProjectTemplate.Services.ErrorLog;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Controllers
{
    public class SystemErrorsLogController : BaseController
    {
        //
        private readonly IErrorLogService _errorLogService;
        public SystemErrorsLogController()
        {
            _errorLogService = new ErrorLogService();
        }

      //  GET: SystemErrors
        public ActionResult Index()
        {
            return View();
        }
        #region
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _errorLogService.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
      

        #endregion
    }
}