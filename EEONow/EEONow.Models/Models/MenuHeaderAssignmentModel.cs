using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class MenuHeaderAssignmentModel
    {
        [Display(Name = "Menu Header")]
        public Int32 MenuHeaderId { get; set; }

        [Display(Name = "Menu Header Name")]
        public String MenuHeaderName { get; set; }

        public List<Int32> MenuId { get; set; }
        [Display(Name = "Menu")]
        [UIHint("MultiSelect")]
        public List<SelectListItem> ListMenu { get; set; }

    }
}
