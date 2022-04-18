using System.ComponentModel.DataAnnotations;
namespace dsdProjectTemplate.ViewModel.User
{
    public class UserViewModel: BaseModel
    {
        //[Display(Name = "Organization")]
        //[ScaffoldColumn(false)]
        //public long OrganizationId { get; set; }
        //[Display(Name = "Role")]
        //[ScaffoldColumn(false)]
        //public int RoleId { get; set; }
        //[Display(Name = "Registration RequestType")]
        //[ScaffoldColumn(false)]
        //public int RegistrationRequestTypeId { get; set; }
        [Display(Name = "User Type")]
        public int UserTypeId { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
     
        [MaxLength(100)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [ScaffoldColumn(false)]
        public string Role { get; set; }
        [ScaffoldColumn(false)]
        public bool PendingRegistration { get; set; }
        [ScaffoldColumn(false)]
        public bool EmailTwoFactorAuthentication { get; set; }
        [ScaffoldColumn(false)]
        public bool SMSTwoFactorAuthentication { get; set; }
        [ScaffoldColumn(false)]

        public string RegistrationCellPhoneNumber { get; set; }
        [ScaffoldColumn(false)]
        public string Organization { get; set; }
    }
    public class Organizations_UsersList : BaseModel
    {
         
        [Display(Name = "User Type")]
        public int UserTypeId { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Organization { get; set; }
    }
}
