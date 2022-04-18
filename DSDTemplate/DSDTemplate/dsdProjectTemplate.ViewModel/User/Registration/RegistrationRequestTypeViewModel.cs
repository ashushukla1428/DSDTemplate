using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.User.Registration
{
    public class RegistrationRequestTypeViewModel: OrganizationBaseModel
    {
        [Required(ErrorMessage = "Type name is required field")]
        [MaxLength(50)]
        public string TypeName { get; set; }
    }
}
