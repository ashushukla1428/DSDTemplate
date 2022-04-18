using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Menu
{
    public class MenuConfigurationViewModel:BaseModel
    {
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(50)]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Menu Key is a required field.")]
        //[MaxLength(50)]
        //[Display(Name = "Menu Key")]
        //public string MenuKey { get; set; }
        [Required(ErrorMessage = "Menu controller is a required field.")]
        [MaxLength(50)]
        [Display(Name = "Menu Controller")]
        public string MenuController { get; set; }
        [Required(ErrorMessage = "Menu action is a required field.")]
        [MaxLength(50)]
        [Display(Name = "Menu Action")]
        public string MenuAction { get; set; }
        [MaxLength(50)]
        [Display(Name = "Menu Icon")]
        public string MenuIcon { get; set; }
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public bool IsAdminOnly { get; set; }
        [Display(Name="Area")]
        public int AreaId { get; set; }
        [Display(Name = "Area Name")]
        [ScaffoldColumn(false)]
        public string AreaName { get; set; }
    }
}
