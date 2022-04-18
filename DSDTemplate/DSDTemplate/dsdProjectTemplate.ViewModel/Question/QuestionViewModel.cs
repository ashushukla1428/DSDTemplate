using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Question
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Question is a required field.")]
        [MaxLength(50)]
        public string Question { get; set; }
        public bool IsActive { get; set; }
    }
}
