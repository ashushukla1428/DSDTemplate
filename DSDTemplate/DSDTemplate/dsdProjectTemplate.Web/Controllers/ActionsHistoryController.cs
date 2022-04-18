using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.ViewModel;
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
    public class ActionsHistoryController : BaseController
    {
        // GET: ActionsHistory
        private readonly IOrganizationService _organizationService;
        IActionsHistoryService _actionsHistoryService;
        public ActionsHistoryController()
        {
            _organizationService = new OrganizationService();
            _actionsHistoryService = new ActionsHistoryService();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.OrganizationsList = await _organizationService.GetDropOrganizationsForActionHistoryAsync();
            return View();
        }
        public async Task<ActionResult> BindData([DataSourceRequest] DataSourceRequest request, ActionsHistorySearch searchFilter)
        {
            try
            {
                var model = await _actionsHistoryService.GetAllAsync(searchFilter);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> ViewJsonData(long id)
        {
            try
            {
                var model = await _actionsHistoryService.GetByIdAsync(id);
                ViewBag.Data = model.Data;
                ViewBag.UserId = id;
                return View("ViewJsonData");
            }
            catch (Exception)
            {

                return RedirectToAction("Errorwindow", "Home");
            }
        }
    }
}