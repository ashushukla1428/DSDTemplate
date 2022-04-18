using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{ 
    public class RegisterModel
    {
        [Display(Name = "UserID")]
        public Int32 UserId { get; set; }
       
        [Display(Name = "Organization")]
        public Int32 OrganizationId { get; set; }
        public List<SelectListItem> ListOrganization { get; set; }
        [Display(Name = "Role")]
        public Int32 RoleId { get; set; }
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
        [Required]
        [Display(Name = "Password")]
        public String Password { get; set; }
        public bool Active { get; set; }
        public Int32 CreateUserId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Int32 UpdateUserId { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
