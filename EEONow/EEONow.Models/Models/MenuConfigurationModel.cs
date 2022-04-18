using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class MenuConfigurationModel
    {
        [ScaffoldColumn(false)]
        public Int32 MenuID_PK { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Controller")]
        public string MenuController { get; set; }
        [Required]
        [Display(Name = "Action")]
        public string MenuAction { get; set; }
        [Required]
        [Display(Name = "Sort Order")]
        public Int32 SortOrder { get; set; }

        [Display(Name = "Active")]
        public Boolean IsActive { get; set; }
        [Display(Name = "Is Admin Only")]
        public bool IsAdminOnly { get; set; }
    } 
}
