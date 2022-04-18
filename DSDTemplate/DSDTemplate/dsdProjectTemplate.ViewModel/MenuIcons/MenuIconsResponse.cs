using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsdProjectTemplate.ViewModel.MenuIcons
{
    public class MenuIconsResponse: BaseModel
    {
        [Required(ErrorMessage = "Menu icon name is a required field.")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Menu icon value is a required field.")]
        [MaxLength(50)]
        [Display(Name = "Menu Icon")]
        public string Value { get; set; }
    }
}
