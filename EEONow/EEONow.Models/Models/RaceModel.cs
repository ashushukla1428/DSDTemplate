using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class RaceModel
    {
        [ScaffoldColumn(false)]
        public Int32 RaceId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [UIHint("Integer")]
        [Required]
        [Display(Name = "Race Number")]
        public Int32 RaceNumber { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Display(Name = "Color Code")]
        public string DisplayColorCode { get; set; }
        [UIHint("OrganisationList")]
        [Required]
        [Display(Name = "Organization")]
        public Int32 OrganizationId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public String OrganizationName { get; set; }
        public bool Active { get; set; }        
    }

}
