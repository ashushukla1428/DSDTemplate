using dsdProjectTemplate.Services.Menu.MenuIcons;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.MenuIcons;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Areas.Masters.Controllers
{    
    public class MenuIconsController : Controller
    {
        private readonly IMenuIcons _MenuIcons;
        public MenuIconsController()
        {
            _MenuIcons = new MenuIcons();
        }
        // GET: Masters/MenuIcons
        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<ActionResult> BindMenuIcons([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await _MenuIcons.GetAllAsync();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        [HttpPost]
        public async Task<JsonResult> Save([DataSourceRequest] DataSourceRequest request, MenuIconsResponse model)
        {
            if (ModelState.IsValid)
            {
                ResponseModel _baseResponse = new ResponseModel();
                if (model.Id == 0)
                {

                    _baseResponse = await _MenuIcons.AddAsync(model);
                    model.Id = (int)_baseResponse.Id;
                }
                else
                {
                    _baseResponse = await _MenuIcons.UpdateAsync(model);
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
        
    }
}