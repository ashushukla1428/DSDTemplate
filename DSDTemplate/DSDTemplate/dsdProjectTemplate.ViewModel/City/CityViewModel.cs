using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.City
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select a state, it is a required.")]
        public int StateId { get; set; }
      
        [Required(ErrorMessage = "City name is a required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
