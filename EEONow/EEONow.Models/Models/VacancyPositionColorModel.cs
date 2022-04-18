using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class VacancyPositionColorModel
    {
        //[Required]
        [Display(Name = "Organization Name")]
        public Int32 OrganizationId { get; set; }
        
        //[Required]
        [Display(Name = "Organization Name")]
        public String Name { get; set; }
        
        [Required]
        [Display(Name = "Vacancies")]
        public string VacanciesDisplayColorCode { get; set; }
        [Required]
        [Display(Name = "Non Vacancies")]
        public string NonVacanciesDisplayColorCode { get; set; }

    }

}
