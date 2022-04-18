using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Counties
{
    public class CountiesResponse
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Select a state, it is a required.")]
        public int StateId { get; set; }
        
        [Required(ErrorMessage = "County name is a required.")]
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
