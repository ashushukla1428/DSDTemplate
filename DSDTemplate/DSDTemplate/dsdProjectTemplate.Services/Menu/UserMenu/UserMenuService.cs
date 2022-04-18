using Dapper;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel.Menu;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services.Menu.UserMenu
{
    public class UserMenuService : IUserMenuService
    {

        public List<UserMenuList> GetUserMenusAsync()
        {
            try
            {
                List<UserMenuList> userMenus = new List<UserMenuList>();
                List<UserSubMenuList> subMenus = new List<UserSubMenuList>();
                
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    //string _mainQuery = @"select uMainMenu.Id, DisplayName as Name,menuCon.MenuAction,menuCon.MenuController,menuCon.MenuIcon,uMainMenu.DisplayOrder,area.AreaName from UserMainMenu as uMainMenu
                    //                           inner join
                    //                           UsersRoles as rols on rols.Id=uMainMenu.UserRoleID and uMainMenu.UserRoleID=@UserRoleID and rols.IsActive=1and uMainMenu.IsActive = 1
                    //                           inner join
                    //                           Organizations as org on org.Id=rols.OrganizationId and org.IsActive=1
                    //                           inner join 
                    //                           MenuConfiguration as menuCon on menuCon.Id=uMainMenu.MenuID
                    //                           left join
                    //                           AppAreas as area on area.id=menuCon.areaid
                    //                           order by uMainMenu.DisplayOrder asc";
                    string _mainQuery = @"select uMainMenu.Id, DisplayName as Name,menuCon.MenuAction,menuCon.MenuController,menuCon.MenuIcon,uMainMenu.DisplayOrder,area.AreaName from UserMainMenu as uMainMenu
                                               left join
                                               MenuConfiguration as menuCon on menuCon.Id=uMainMenu.MenuID
                                               inner join
                                               UsersRoles as rols on rols.Id=uMainMenu.UserRoleID and uMainMenu.UserRoleID=@UserRoleID and rols.IsActive=1 and uMainMenu.IsActive = 1
                                               inner join
                                               Organizations as org on org.Id=rols.OrganizationId and org.IsActive=1                                                                     
                                               left join
                    AppAreas as area on area.id=menuCon.areaid
                                               order by uMainMenu.DisplayOrder asc";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserRoleID", UserSession.Current.UserRoleId);
                    var _menus =   con.Query<UserMenuList>(_mainQuery, parameters, commandType: CommandType.Text);
                    con.Close();
                    var _subMenus =   GetSubMenus();
                    foreach (var item in _menus.OrderBy(c => c.DisplayOrder))
                    {   
                        //ADD SUB IN THE LIST MENUS 
                        subMenus = new List<UserSubMenuList>();
                        foreach (var menu in _subMenus.Where(c => c.MenuHeaderID==item.Id))
                        {
                            subMenus.Add(new UserSubMenuList
                            {
                                Name = menu.Name,
                                MenuAction = menu.MenuAction,
                                MenuController = menu.MenuController,
                                MenuIcon = menu.MenuIcon,
                                DisplayOrder = (int)menu.DisplayOrder,
                                AreaName=menu.AreaName
                            });
                        }
                        //ADD MAIN MENU 
                        userMenus.Add(new UserMenuList
                        {
                            Name = item.Name,
                            MenuAction = item.MenuAction,
                            MenuController = item.MenuController,
                            MenuIcon = item.MenuIcon,
                            SubMenus = subMenus ,//ADD SUB MENUS ,
                            AreaName = item.AreaName
                        });
                    }
                }

                //var _menus =   _context.UserMainMenus.Where(c => c.UserRoleID == UserSession.Current.UserRoleId  && c.IsActive==true && c.UsersRole.IsActive==true && c.UsersRole.Organization.IsActive==true).Include(c => c.UserSubMenus).ToList();
                //foreach (var item in _menus.OrderBy(c=>c.DisplayOrder))
                //{
                    
                //    //ADD SUB IN THE LIST MENUS 
                //    subMenus = new List<UserSubMenuList>();
                //    foreach (var menu in item.UserSubMenus.Where(c=>c.MenuConfiguration.IsActive==true && c.IsActive==true))
                //    {
                //        subMenus.Add(new UserSubMenuList
                //        {   
                //            Name= menu.DisplayName,
                //            MenuAction= menu.MenuConfiguration.MenuAction,
                //            MenuController= menu.MenuConfiguration.MenuController,
                //            MenuIcon= menu.MenuConfiguration.MenuIcon,
                //            DisplayOrder= (int)menu.MenuConfiguration.DisplayOrder
                //        });
                //    }
                //    //ADD MAIN MENU 
                //    userMenus.Add(new UserMenuList
                //    {
                //        Name = item.DisplayName,
                //        MenuAction = item.MenuConfiguration.MenuAction,
                //        MenuController = item.MenuConfiguration.MenuController,
                //        MenuIcon = item.MenuConfiguration.MenuIcon,
                //        SubMenus= subMenus //ADD SUB MENUS 
                //    });
                //}

                return userMenus;
            }
            catch (System.Exception )
            {
               // await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->GetUserMenusAsync", ex);
                throw;
            }
        }

        public IEnumerable<UserMenuList> GetSuperAdminMenusAsync()
        {
            try
            {
                //List<UserMenuList> userMenus = new List<UserMenuList>();                
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    string _mainQuery = @"select menuCon.Name,menuCon.MenuAction,menuCon.MenuController,menuCon.MenuIcon,menuCon.DisplayOrder,area.AreaName 
									from MenuConfiguration menuCon left join
									AppAreas as area on area.id=menuCon.areaid
									where menuCon.IsActive = 1 order by menuCon.DisplayOrder asc";
                    return con.Query<UserMenuList>(_mainQuery, commandType: CommandType.Text);
                }
            }
            catch (System.Exception)
            {
                // await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Severe, this.GetType().Name + "->GetUserMenusAsync", ex);
                throw;
            }
        }
        private IEnumerable<UserSubMenuList> GetSubMenus()
        {
           
            using (var con = new SqlConnection(SQLConnectionString.dbConnection))
            {
                string _mainQuery = @"select uSubMenu.Id,uSubMenu.MenuHeaderID,uSubMenu.DisplayName as Name,menuCon.MenuAction,
                    menuCon.MenuController,menuCon.MenuIcon,uSubMenu.DisplayOrder,area.AreaName from UserSubMenu as uSubMenu
                    inner join
                    UserMainMenu as uMainMenu on uMainMenu.Id=uSubMenu.MenuHeaderID  and uMainMenu.IsActive=1
 
                    inner join 
                    MenuConfiguration as menuCon on menuCon.Id=uSubMenu.MenuID
                    left join
									AppAreas as area on area.id=menuCon.areaid
                    and  uMainMenu.UserRoleID=@UserRoleID
                    order by uSubMenu.DisplayOrder asc";
                var parameters = new DynamicParameters();
                parameters.Add("@UserRoleID", UserSession.Current.UserRoleId);
               return  con.Query<UserSubMenuList>(_mainQuery, parameters, commandType: CommandType.Text);
                 
            }
         }
    }
}
