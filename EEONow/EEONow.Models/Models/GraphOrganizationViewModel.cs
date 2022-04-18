using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class GraphOrganizationViewModel
    {
        [ScaffoldColumn(false)]
        public Int32 GraphOrganizationViewId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Order")]
        public Int32 Order { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "OrganizationId")]
        public Int32 OrganizationId { get; set; }
        [Required]
        [Display(Name = "Organization")]
        public String Organization { get; set; }
        public bool Active { get; set; }        
    }
    public class AssigGraphOrganizationViewModel
    {
        [Display(Name = "GraphLevelId")]
        public Int32 GraphLevelId { get; set; }
        [Display(Name = "Section")]
        public String GraphLevelName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "OrganizationId")]
        public Int32 OrganizationId { get; set; }
        [Required]
        [Display(Name = "Organization")]
        public String Organization { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "RoleId")]
        public Int32 RoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public String RoleName { get; set; }
        public List<Int32> GraphOrganizationViewId { get; set; }
        [Display(Name = "Pages")]
        [UIHint("GraphOrganizationViewMultiSelect")]
        public List<SelectListItem> ListGraphOrganizationView { get; set; }
    }
}
