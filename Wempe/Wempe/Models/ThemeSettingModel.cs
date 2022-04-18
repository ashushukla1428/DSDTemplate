using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class ThemeSettingModel
    {
        public string BodyColor { get; set; }
        public string PageSidebar { get; set; }
        public string PageHeader { get; set; }
        public string PageFooter { get; set; }
        public string PageContent { get; set; }
        public string PageBar { get; set; }
        public string TabContent { get; set; }
        public string PortletTitle { get; set; }
        public string PortletBody { get; set; }
        public string FormActions { get; set; }
        public string FormControl { get; set; }
        public string ModalPopUp { get; set; }
        public string SidebarSubMenu { get; set; }
        public string Button { get; set; }
        public string ActiveTab { get; set; }
        public string ActiveSideBar { get; set; }
        public string SideBarHover { get; set; }
        public string InnerTable { get; set; }
        public string InnerTableHover { get; set; }
        public string ButtonHover { get; set; }
        public Int64 CompanyId { get; set; }
    }
}