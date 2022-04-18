using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class AssignRoleModel
    {
        [Display(Name = "Role")]
        public Int32 RoleId { get; set; }

        [Display(Name = "Role")]
        public String RoleName { get; set; }

        public List<Int32> MenuId { get; set; }
        [Display(Name = "Menu")]
        [UIHint("MultiSelect")]
        public List<SelectListItem> ListMenu { get; set; }

    }


    public class ManageUserRoleModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "User ID")]
        public Int32 UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public String MiddleName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }

        //[Display(Name = "User Name")]
        //[Editable(false)]
        //public String UserName { get; set; }
        //[Display(Name = "Email")]
        //public String EmailAddress { get; set; }
        [Display(Name = "Organization")]
        [UIHint("OrganisationList")]
        [Required]
        public Int32 OrganizationId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public String OrganizationName { get; set; }


        [UIHint("RoleList")]
        [Display(Name = "Role")]
        [Required]
        public Int32 RoleId { get; set; }
        public Boolean IsActive { get; set; }


        [Display(Name = "Last Login Date")]         
        public string LastLoginDate { get; set; }

        [Display(Name = "Days Since Last Login ")]       
        public String DaysSinceLastLogin  { get; set; }
        
    }


}
