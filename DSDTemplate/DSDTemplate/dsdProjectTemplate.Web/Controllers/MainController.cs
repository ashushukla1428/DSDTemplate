using dsdProjectTemplate.Services.Menu;
using dsdProjectTemplate.Services.Menu.UserMenu;
using dsdProjectTemplate.Services.Organization;
using dsdProjectTemplate.Services.User.RegistrationRequestType;
using dsdProjectTemplate.Services.User.UsersRole;
using dsdProjectTemplate.Services.UserType;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dsdProjectTemplate.Web.Controllers
{
    public class MainController : Controller
    {
        
        public MainController()
        {

        }
        // GET: Main
        public PartialViewResult GetMenu()
        {
            UserMenuService obj = new UserMenuService();
            try
            {
                if (UserSession.Current.IsSuperAdmin)
                {
                    //bind static menus
                    //return PartialView("_MenuForSuperAdmin");
                    return PartialView("_MenuForUser", obj.GetSuperAdminMenusAsync());
                }
                else
                {
                    return PartialView("_MenuForUser",  obj.GetUserMenusAsync());
                }                
            }
            catch
            {
                throw;
            }
        }
        public async Task<ActionResult> GetOrganizations()
        {
            try
            {
                OrganizationService obj = new OrganizationService();
                return Json(await obj.GetDropOrganizationsAsync(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public ActionResult GetUserOrganizations()
        {
            try
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                if (UserSession.Current.OrgList != null)
                {
                    foreach (var item in UserSession.Current.OrgList)
                    {
                        SelectListItem selectItem = new SelectListItem
                        {
                            Text = item.OrgName,
                            Value = item.OrgId.ToString(),

                        };
                        //if (item.OrgId == UserSession.Current.SelectedOrgId)
                        //{
                        //    selectItem.Selected = true;
                        //}
                        selectListItems.Add(selectItem);
                    }
                }
                return Json(selectListItems, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Low, "Error", ex);
                return RedirectToAction("Errorwindow", "Home");
            }
          
        }
        [HttpPost]
        public ActionResult SetUserSelectedOrganization(long orgId)
        {
            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                   
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        LoginResponse serializeModel = JsonConvert.DeserializeObject<LoginResponse>(authTicket.UserData);
                        var _orgData = UserSession.Current.OrgList.Where(c => c.OrgId == orgId).FirstOrDefault();
                        UserSession.Current.SelectedOrgId = orgId;
                        UserSession.Current.SelectedOrgName = _orgData.OrgName;
                        UserSession.Current.UserRoleId = _orgData.RoleId;
                        UserSession.Current.Roles = _orgData.RoleName;
                        UserSession.Current.CanEditRecords = _orgData.CanEditRecords;
                        UserSession.Current.CanAddRecords = _orgData.CanAddRecords;

                        serializeModel.SelectedOrgId = orgId;
                        serializeModel.SelectedOrgName = _orgData.OrgName;
                        serializeModel.UserRoleId = _orgData.RoleId;
                        serializeModel.Roles = _orgData.RoleName;

                        serializeModel.CanEditRecords = _orgData.CanEditRecords;
                        serializeModel.CanAddRecords = _orgData.CanAddRecords;
                        UserSession.SetLoginCookiesData(serializeModel);
                    }
                    
                  
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }

        }

        public async Task<ActionResult> GetUserTypesList()
        {
            try
            {
                UserTypeService obj = new UserTypeService();
                return Json(await obj.GetDropUsersTypeAsync(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }

        public async Task<ActionResult> GetAllAppAreaMasterAsync()
        {
            try
            {
                MenuConfigurationService obj = new MenuConfigurationService();
                return Json(await obj.GetAllAppAreaMasterAsync(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> GetMenuList()
        {
            try
            {
                MenuConfigurationService obj = new MenuConfigurationService();
                return Json(await obj.GetDropListAsync(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> GetRolesList(long orgId)
        {
            try
            {
                UsersRoleService obj = new UsersRoleService();
                return Json(await obj.GetDropListAsync(orgId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        public async Task<ActionResult> GetRegistrationRequestTypeList(long orgId)
        {
            try
            {
                RegistrationRequestTypeService obj = new RegistrationRequestTypeService();
                return Json(await obj.GetDropListAsync(orgId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Errorwindow", "Home");
            }
        }
        
    }

}