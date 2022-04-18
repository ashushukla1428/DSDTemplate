using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Providers
{
   public class ProvidersContactTypesViewModel:OrganizationBaseModel
    {
        [Required(ErrorMessage = "Contact type is required field")]
        [MaxLength(50)]
        public string ContactTypeName { get; set; }
    }
}
