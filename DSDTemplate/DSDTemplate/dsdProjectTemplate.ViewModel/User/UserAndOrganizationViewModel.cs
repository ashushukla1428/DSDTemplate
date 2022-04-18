using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.User
{
    public class UserAndOrganizationViewModel:BaseModel
    {
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }
        public int RoleId { get; set; }
        [Display(Name = "Registration RequestType")]
        public int RegistrationRequestTypeId { get; set; }
        public long UserId { get; set; }
    }
}
