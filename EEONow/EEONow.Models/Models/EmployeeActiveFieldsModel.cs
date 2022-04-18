using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EmployeeActiveFieldModel
    {
        [ScaffoldColumn(false)]
        public Int32 EmployeeActiveFieldId { get; set; }    
        [Required]
        [Display(Name = "Column Name")]
        public string DisplayLabelData { get; set; }       
        [Display(Name = "Column Key")]
        public string ColumnKey { get; set; }        
        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }
        [Display(Name = "OrganizationId")]
        public Int32 OrganizationId { get; set; }
       
        [Display(Name = "RoleId")]
        public Int32 RoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public String RoleName { get; set; }
        [Required]
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }

}
