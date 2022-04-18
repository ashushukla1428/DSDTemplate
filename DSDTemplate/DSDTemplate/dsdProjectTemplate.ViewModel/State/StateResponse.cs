using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.State
{
    public class StateResponse:BaseModel
    {
        [Required(ErrorMessage = "State name is a required field.")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
