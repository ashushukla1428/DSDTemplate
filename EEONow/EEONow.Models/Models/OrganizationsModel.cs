using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class OrganizationModel
    {
        [ScaffoldColumn(false)]        
        public Int32 OrganizationId { get; set; }
        [Required]        
        [Display(Name = "Organization Code")]
        public String OrgCode { get; set; }
        [Required]
        [Display(Name = "Organization Name")]
        public String Name { get; set; }
        
        [Display(Name = "Description")]
        public String Description { get; set; }
        [Required]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [UIHint("DefaultStateList")]
        [Required]
        [Display(Name = "Default ALM State")]
        public Int32 DefaultStateId { get; set; } 

        [UIHint("DefaultEEOOccupationalCodeList")]
        [Required]
        [Display(Name = "Default ALM Occupational Code")]
        public Int32 DefaultEEOOccupationalCodeID { get; set; }

        [UIHint("ParentOrganizationList")]
       
        [Display(Name = "Parent Organization")]
        public Int32 ParentOrganizationId { get; set; }

        [UIHint("StateList")]
        [Required]
        [Display(Name = "State")]
        public Int32 StateId { get; set; }
          
        [Required]         
        [Display(Name = "City")]
        public String City { get; set; }
        [Required]       
        [Display(Name = "Zip Code")]
        public String ZipCode { get; set; }
        public bool Active { get; set; }
        public bool EnableTwoFactorAuthentication { get; set; }
        //[Required]
        //[Display(Name = "Vacancies Display Color Code")]
        //public string VacanciesDisplayColorCode { get; set; }
        // [ScaffoldColumn(false)]
        //public Int32 CreateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime CreateDateTime { get; set; }
        //[ScaffoldColumn(false)]
        //public Int32 UpdateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime UpdateDateTime { get; set; }
    }

}
