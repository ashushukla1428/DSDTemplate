using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.User
{
    public class UserProfileViewModel
    {
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
        public string EmailAddress { get; set; }

        [Display(Name = "Security Question 1")]
        public int? SecurityQuestion1 { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Answer Security Question 1")]
        public string AnswerSecurityQuestion1 { get; set; }
        [Display(Name = "Security Question 2")]
        public int? SecurityQuestion2 { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Answer Security Question 2 ")]
        public string AnswerSecurityQuestion2 { get; set; }
        [Display(Name = "Security question 3")]
        public int? SecurityQuestion3 { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Answer Security Question 3 ")]
        public string AnswerSecurityQuestion3 { get; set; }
        [MaxLength(15)]
        [Display(Name = "Cell PhoneNumber")]
        public string RegistrationCellPhoneNumber { get; set; }
        [MaxLength(15)]
        [Required]
        [Display(Name = "Work PhoneNumber")]
        public string RegistrationWorkPhoneNumber { get; set; }
        [MaxLength(15)]
        [Display(Name = "Work Phone Number ext.")]
        public string RegistrationWorkPhoneNumberExt { get; set; }
        [Display(Name = "Email Two Factor Authentication")]
        public bool EmailTwoFactorAuthentication { get; set; }
        [Display(Name = "SMS Two Factor Authentication")]
        public bool SMSTwoFactorAuthentication { get; set; }
    }
}
