namespace dsdProjectTemplate.ViewModel.User
{
    public class UserOrgList
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long OrgId { get; set; }
        public string OrgName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailTwoFactorAuthentication { get; set; }
        public bool SMSTwoFactorAuthentication { get; set; }
        public int RegistrationRequestTypeId { get; set; }
    }
}
