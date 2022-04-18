using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace dsdProjectTemplate.ViewModel.Menu
{
    public class MenuHeaderConfigurationViewModel:BaseModel
    {
        [Display(Name = "Organization")]
        public long OrganizationId { get; set; }
        [Display(Name = "Role")]
        public int UserRoleId { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(50)]
        [Display(Name="Display Name")]
        public string Name { get; set; }
        
        [Display(Name = "Menu Page")]
        public int MainMenuId { get; set; }
        
        //[Display(Name="Main Menu")]
        //public string MainMenu { get; set; }
        [Display(Name = "Menu Action")]
        public string MenuAction { get; set; }
        [Display(Name = "Menu Controller")]
        public string MenuController { get; set; }
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        //public bool IsHeader { get; set; }
        //public List<Int32> MenuId { get; set; }
        [Display(Name = "Sub Menu Pages")]
        [UIHint("MultiSelect")]
        public List<SelectListItem> ListMenu { get; set; }

    }
    public class SubMenuConfigurationViewModel: BaseModel
    {
        public int MenuHeaderID { get; set; }
      
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(50)]
        [Display(Name = "Display Name")]
        public string Name { get; set; }
        [Display(Name = "Menu Page")]
        public int MainMenuId { get; set; }
        [Display(Name = "Menu Action")]
        public string MenuAction { get; set; }
        [Display(Name = "Menu Controller")]
        public string MenuController { get; set; }
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
