namespace dsdProjectTemplate.ViewModel.User
{
    public class UsersLoginPassword:BaseModel
    {
        public long UserId { get; set; }
        public string EncryptedPassword { get; set; }
        public string PasswordSalt { get; set; }
        public string SecurityToken { get; set; }
    }
    public class SoftwareUser:BaseModel
    {
        public long UserId { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
