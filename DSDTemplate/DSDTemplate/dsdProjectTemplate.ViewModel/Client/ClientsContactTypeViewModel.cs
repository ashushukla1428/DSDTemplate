using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Client
{
   public class ClientsContactTypeViewModel : OrganizationBaseModel
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Contact type is required field")]
        public string ContactTypeName { get; set; }
    }
}
