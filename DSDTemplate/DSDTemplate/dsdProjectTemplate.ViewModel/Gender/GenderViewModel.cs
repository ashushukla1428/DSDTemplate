using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Gender
{
    public class GenderViewModel: BaseModel
    {
        [Required(ErrorMessage = "Gender Type is a required field.")]
        [MaxLength(50)]
        public string GenderType { get; set; }
    }
}
