using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.User
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string Code { get; set; }
    }
    public class LoginResponse : BaseResponse
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Roles { get; set; }
        public long UserRoleId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long SelectedOrgId { get; set; }
        public string SelectedOrgName { get; set; }
        public List<LoggedInUserOrgList> OrgList { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
        public bool IsSoftware_User { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool SMSTwoFactorAuthentication { get; set; }
        public bool EmailTwoFactorAuthentication { get; set; }
        public bool IsTwoFactorAuthenticationDone { get; set; }
        public bool IsTwoFactorAuthenticationRequested { get; set; }

    }
}
