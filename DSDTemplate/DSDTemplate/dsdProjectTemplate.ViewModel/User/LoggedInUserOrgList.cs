namespace dsdProjectTemplate.ViewModel.User
{
    public class LoggedInUserOrgList
    {
        public long OrgId { get; set; }
        public string OrgName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool CanAddRecords { get; set; }
        public bool CanEditRecords { get; set; }
    }
}
