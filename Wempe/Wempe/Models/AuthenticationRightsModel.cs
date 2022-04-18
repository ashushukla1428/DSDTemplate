using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class AuthenticationRightsModel : PageingModel
    {
        public Int32 ActionID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string role { get; set; }
        public string OtherFunctions { get; set; }
        public bool Status { get; set; }
        public bool Access { get; set; }
        public string Message { get; set; }
    }
    public partial class RoleMaster
    {
        public int RoleID { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdate { get; set; }
        public long UpdateBy { get; set; }
        public long OwnerID { get; set; }
        public string  ActionsID { get; set; }
    }

    public class RightsRoleWiseModel
    {
        public Int32 ActionID { get; set; }
        public string ControllerName { get; set; }
        public string RightFor { get; set; }
        public string ActionName { get; set; }
        public bool Access { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class MenuModels
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Id { get; set; }
        public string MenuText { get; set; }
        public string cssClass { get; set; }
        public int MenuIndex { get; set; }


    }


    public class CompanyMenu : MenuModels
    {
        public int pId { get; set; }

        public Boolean Group { get; set; }

    }

    public class AdminMenu: MenuModels
    {      
        public int pId { get; set; }

        public Boolean Group { get; set; }
        
    }
    

}