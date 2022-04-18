using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Wempe.Models
{
    public class AdminUserModel : LoginUserModel
    {
        public int UserID { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public String FullName { get; set; }

       
        public bool MyProperty { get; set; }
        public string Note { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    public class LoginUserModel : userEmail
    {
        [Required]
        public String Password { get; set; }
        public String ReturnUrl { get; set; }
    }
    public class userEmail {
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        public String EmailAddress { get; set; }
    }

}