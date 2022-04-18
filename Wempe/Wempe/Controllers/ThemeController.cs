using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class ThemeController : Controller
    {
        //
        // GET: /Theme/
        dbWempeEntities _this = new dbWempeEntities();
        public ActionResult Index()
        {
            var id = SessionMaster.Current.OwnerID;
           
            ThemeSettingModel model = new ThemeSettingModel(); //
            model= GetThemeByCompanyId(id);
            if (model.CompanyId == 0)
            {
                wmpWebsiteTheme newThemek = new wmpWebsiteTheme();
                newThemek.CompanyId = id;
                newThemek.ActiveSideBar = model.ActiveSideBar;
                newThemek.ActiveTab = model.ActiveTab;
                newThemek.BodyColor = model.BodyColor;
                newThemek.Button = model.Button;
                newThemek.ButtonHover = model.ButtonHover;
                newThemek.FormActions = model.FormActions;
                newThemek.FormControl = model.FormControl;
                newThemek.InnerTable = model.InnerTable;
                newThemek.InnerTableHover = model.InnerTableHover;
                newThemek.ModalPopUp = model.ModalPopUp;
                newThemek.PageBar = model.PageBar;
                newThemek.PageContent = model.PageContent;
                newThemek.PageFooter = model.PageFooter;
                newThemek.PageHeader = model.PageHeader;
                newThemek.PageSidebar = model.PageSidebar;
                newThemek.PortletBody = model.PortletBody;
                newThemek.PortletTitle = model.PortletTitle;
                newThemek.SideBarHover = model.SideBarHover;
                newThemek.SidebarSubMenu = model.SidebarSubMenu;
                newThemek.TabContent = model.TabContent;
                using (var dbCtx = new dbWempeEntities())
                {
                    dbCtx.wmpWebsiteThemes.Add(newThemek);
                    dbCtx.SaveChanges();
                }            
            }
            return View(model);
        }
        public ThemeSettingModel GetThemeByCompanyId(Int64 Id)
        {
            ThemeSettingModel clr = new ThemeSettingModel();           
            var k = _this.wmpWebsiteThemes.Where(s => s.CompanyId == Id).FirstOrDefault();
            if (k == null)
            {
               var newId = 0;
                k = _this.wmpWebsiteThemes.Where(s => s.CompanyId == newId).FirstOrDefault();
            }
            clr.CompanyId = Id;
            clr.BodyColor = k.BodyColor;
            clr.FormActions = k.FormActions;
            clr.FormControl = k.FormControl;
            clr.ModalPopUp = k.ModalPopUp;
            clr.PageBar = k.PageBar;
            clr.PageContent = k.PageContent;
            clr.PageFooter = k.PageFooter;
            clr.PageHeader = k.PageHeader;
            clr.PageSidebar = k.PageSidebar;
            clr.PortletBody = k.PortletBody;
            clr.PortletTitle = k.PortletTitle;
            clr.SidebarSubMenu = k.SidebarSubMenu;
            clr.TabContent = k.TabContent;
            clr.Button = k.Button;
            clr.ActiveTab = k.ActiveTab;
            clr.ActiveSideBar = k.ActiveSideBar;
            clr.SideBarHover = k.SideBarHover;
            clr.InnerTable = k.InnerTable;
            clr.InnerTableHover = k.InnerTableHover;            
            clr.ButtonHover = k.ButtonHover;
            return clr;

        }
        [HttpPost]
        public ActionResult UpdateThemeSetting(ThemeSettingModel model)
        {
            var id = SessionMaster.Current.OwnerID;
            var k = _this.wmpWebsiteThemes.Where(s => s.CompanyId == id).FirstOrDefault();
            k.CompanyId = id;
            k.ActiveSideBar = model.ActiveSideBar;
            k.ActiveTab = model.ActiveTab;
            k.BodyColor = model.BodyColor;
            k.Button = model.Button;
            k.ButtonHover = model.ButtonHover;
            k.FormActions = model.FormActions;
            k.FormControl = model.FormControl;
            k.InnerTable = model.InnerTable;
            k.InnerTableHover = model.InnerTableHover;
            k.ModalPopUp = model.ModalPopUp;
            k.PageBar = model.PageBar;
            k.PageContent = model.PageContent;
            k.PageFooter = model.PageFooter;
            k.PageHeader = model.PageHeader;
            k.PageSidebar = model.PageSidebar;
            k.PortletBody = model.PortletBody;
            k.PortletTitle = model.PortletTitle;
            k.SideBarHover = model.SideBarHover;
            k.SidebarSubMenu = model.SidebarSubMenu;
            k.TabContent = model.TabContent;
            _this.SaveChanges();
           // _this.wmpWebsiteThemes.up
            return null;
        }


    }
}
