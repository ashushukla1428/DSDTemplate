using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
         
    }

    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class LoginResponse : BaseResponseModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Roles { get; set; }
        public long UserRoleId { get; set; }
        
        public int OrgId { get; set; }

        public bool IsFilter { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }


    }
    public class ResetPasswordModel
    { 
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string ResetPasswordKey { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }
        [DisplayName("Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
