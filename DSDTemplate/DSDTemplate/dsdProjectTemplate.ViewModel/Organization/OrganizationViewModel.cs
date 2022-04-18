using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Organization
{
    public class OrganizationViewModel:BaseModel
    {
        [Display(Name = "Parent Organization")] 
        public long ParentOrganizationId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Organization Name")]
        public String Name { get; set; }
        [Display(Name = "Description")]
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Organization Code")]
        public String OrgCode { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Work Number")]
        public string WorkNumber { get; set; }
        [MaxLength(10)]
        [Display(Name = "Fax Number")]
        public string FaxNumber { get; set; }
        [Display(Name = "Contact Email")]
        [MaxLength(100)]
        [Required(ErrorMessage = "The contact email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }
       
        [Required]
        [MaxLength(100)]
        [Display(Name = "Address")]
        public String Address { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        [MaxLength(10)]
        public String ZipCode { get; set; }
        [UIHint("StateList")]
        [Required]
        [Display(Name = "State")]
        public int StateId { get; set; }
        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public bool IsAdminOnly { get; set; }

    }
}
