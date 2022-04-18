using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Counties
{
    public class CountiesRequest
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
