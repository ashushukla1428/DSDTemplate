using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.UserType
{
    public class UserTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User Type is a required field.")]
        [MaxLength(50)]
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
