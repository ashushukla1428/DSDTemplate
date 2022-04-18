using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class MainMenuController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        //
        // GET: /MainMenu/
        [ChildActionOnly]
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult CompanyMenuLayout()
        {
            //SessionMaster.Current.LoginId = 0;
            if (SessionMaster.Current.LoginId == 0)
            {
                TempData["SessionTimeout"] = "Your session has been timeout. Please login again.";
                Response.Redirect("/");
              // return view
            }

            //Get the menuItems collection from somewhere
            List<CompanyMenu> _listMainMenu = new List<CompanyMenu>();

            List<CompanyMenu> _items = db.Database.SqlQuery<CompanyMenu>("USP_GetMenuRoleWise @p0", SessionMaster.Current.LoginId).OrderBy(c => c.MenuIndex).ToList();

           

         

            // _listMainMenu.Add(new AdminMenu { ActionName = "Search", ControllerName = "Repair", cssClass = "icon-magnifier", Id = "", MenuIndex = 0, MenuText = "Search", pId = 0, Group = false });

            


            //_listMainMenu.Add(new AdminMenu { ActionName = "Manage", ControllerName = "Manage", cssClass = "icon-layers", Id = "", MenuIndex = 0, MenuText = "Manage", pId = 100 });

              _listMainMenu.Add(new CompanyMenu { ActionName = "ManageItems", ControllerName = "ManageItems", cssClass = "icon-layers", Id = "liManageItems", MenuIndex = 0, MenuText = "Items", pId = 1, Group=true });


            
            _listMainMenu.Add(new CompanyMenu { ActionName = "ManageTasks", ControllerName = "ManageTasks", cssClass = "icon-layers", Id = "liManageTasks", MenuIndex = 0, MenuText = "Tasks", pId = 7, Group = true });
            _listMainMenu.Add(new CompanyMenu { ActionName = "ManageUsers", ControllerName = "ManageUsers", cssClass = "icon-users", Id = "liManageUsers", MenuIndex = 0, MenuText = "User", pId = 4, Group = true });




            _listMainMenu.Add(new CompanyMenu { ActionName = "Reports", ControllerName = "Reports", cssClass = "icon-list", Id = "liReports", MenuIndex = 0, MenuText = "Reports", pId = 6, Group = true });


            _listMainMenu.Add(new CompanyMenu { ActionName = "ManageVendors", ControllerName = "ManageVendors", cssClass = "icon-users", Id = "liManageVendors", MenuIndex = 0, MenuText = "Vendors", pId = 5, Group = true });






            //_listMainMenu.Add(new CompanyMenu { ActionName = "ManagePrinter", ControllerName = "ManagePrinter", cssClass = "icon-printer", Id = "", MenuIndex = 0, MenuText = "Manage Printer", pId = 3 });


            //    var Menus = db.wmpMVCAuthenticationRights.Where(s => s.OnlyForCompanyUsers != false);
            var x = db.wmpEmployees.Where(s => s.UserID == SessionMaster.Current.LoginId).FirstOrDefault();
            ViewBag.ImageLogo = x.Image;
            ViewBag.Menus = _items;


            return View(_listMainMenu);
        }

        [ChildActionOnly]
        public ActionResult AdminMenuLayout()
        {

            if (SessionMaster.Current.LoginId == 0)
            {
                TempData["SessionTimeout"] = "Your session has been timeout. Please login again.";
                Response.Redirect("/");
                // return view
            }

            List<AdminMenu> _listMainMenu = new List<AdminMenu>();
            List<AdminMenu> _items = db.Database.SqlQuery<AdminMenu>("USP_GetMenuRoleWise @p0", SessionMaster.Current.LoginId).OrderBy(c => c.MenuIndex).ToList();

            _listMainMenu.Add(new AdminMenu {ActionName="Dashboard",ControllerName= "Admin", cssClass= "icon-screen-desktop", Id="liDashboard",MenuIndex=0,MenuText= "Dashboard", pId=0, Group = false });


            //_listMainMenu.Add(new AdminMenu { ActionName = "Index", ControllerName = "CompanyProfile", cssClass = "icon-wallet", Id = "", MenuIndex = 0, MenuText = "Profile", pId = 0, Group = false });



            if (_items.Any(c => c.ActionName == "Index" && c.ControllerName == "Company"))
            {
                _listMainMenu.Add(new AdminMenu { ActionName = "Index", ControllerName = "Company", cssClass = "icon-globe", Id = "liCompany", MenuIndex = 0, MenuText = "Company", pId = 0, Group = false });
            }

           // _listMainMenu.Add(new AdminMenu { ActionName = "Search", ControllerName = "Repair", cssClass = "icon-magnifier", Id = "", MenuIndex = 0, MenuText = "Search", pId = 0, Group = false });

            _listMainMenu.Add(new AdminMenu { ActionName = "NewRepair", ControllerName = "Repair", cssClass = "icon-pencil", Id = "liRepair", MenuIndex = 0, MenuText = "Repair", pId = 0, Group = false });



            //_listMainMenu.Add(new AdminMenu { ActionName = "Manage", ControllerName = "Manage", cssClass = "icon-layers", Id = "", MenuIndex = 0, MenuText = "Manage", pId = 100 });

            //  _listMainMenu.Add(new AdminMenu { ActionName = "ManageItems", ControllerName = "ManageItems", cssClass = "icon-layers", Id = "liManageItems", MenuIndex = 0, MenuText = "Items", pId = 1, Group=true });


            _listMainMenu.Add(new AdminMenu { ActionName = "ManageCity", ControllerName = "ManageCity", cssClass = "icon-map", Id = "liManageLocation", MenuIndex = 0, MenuText = "Locations", pId = 2, Group = true });
            //_listMainMenu.Add(new AdminMenu { ActionName = "ManageTasks", ControllerName = "ManageTasks", cssClass = "icon-layers", Id = "liManageTasks", MenuIndex = 0, MenuText = "Tasks", pId = 7, Group = true });
            _listMainMenu.Add(new AdminMenu { ActionName = "ManageUsers", ControllerName = "ManageUsers", cssClass = "icon-users", Id = "liManageUsers", MenuIndex = 0, MenuText = "User", pId = 4, Group = true });
            _listMainMenu.Add(new AdminMenu { ActionName = "Reports", ControllerName = "Reports", cssClass = "icon-list", Id = "liReports", MenuIndex = 0, MenuText = "Reports", pId = 6, Group = true });





            // _listMainMenu.Add(new AdminMenu { ActionName = "ManageVendors", ControllerName = "ManageVendors", cssClass = "icon-users", Id = "liManageVendors", MenuIndex = 0, MenuText = "Vendors", pId = 5, Group = true });






            //_listMainMenu.Add(new AdminMenu { ActionName = "ManagePrinter", ControllerName = "ManagePrinter", cssClass = "icon-printer", Id = "", MenuIndex = 0, MenuText = "Manage Printer", pId = 3 });


            //    var Menus = db.wmpMVCAuthenticationRights.Where(s => s.OnlyForCompanyUsers != false);

            var x = db.wmpEmployees.Where(s => s.UserID == SessionMaster.Current.LoginId).FirstOrDefault();
            ViewBag.ImageLogo = x.Image;
            ViewBag.Menus = _items;
                       
            //Get the menuItems collection from somewhere
            
            return View(_listMainMenu);
        }

        //AdminMenu

    }
}
