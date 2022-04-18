using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class HeaderMenuSearchModel
    {
        public Int32 FilterRoleId { get; set; }
        public Int32 organizationId { get; set; }

    }
    public class MenuHeaderModel
    {
        [ScaffoldColumn(false)]
        public Int32 MenuHeaderID_PK { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Organization Name")]
        public Int32 OrganizationId { get; set; }
        [Required]
        [UIHint("RoleList")]
        [Display(Name = "Role")]
        public int RoleId { get; set; } 
        //[Required]
        //[Display(Name = "Menu Key")]
        //public string MenuKey { get; set; }
        
        [Display(Name = "Controller")]
        public string MenuController { get; set; }
        
        [Display(Name = "Action")]
        public string MenuAction { get; set; }
         
        [Display(Name = "Icon")]
        public string MenuIcon { get; set; }
        [Required]
        [Display(Name = "Sort Order")]
        public Int32 SortOrder { get; set; }

        [Display(Name = "Is Header")]
        public Boolean IsHeader { get; set; }
        
        [Display(Name = "Active")]
        public Boolean IsActive { get; set; }

        public List<Int32> MenuId { get; set; }
        [Display(Name = "Menu items")]
        [UIHint("MultiSelect")]
        public List<SelectListItem> ListMenu { get; set; }

    }

}
