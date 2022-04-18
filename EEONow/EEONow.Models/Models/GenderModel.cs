using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class GenderModel
    {

        [ScaffoldColumn(false)]
        [Display(Name = "OrganizationID")]
        public Int32 OrganizationId { get; set; }
        [Required]
        [Display(Name = "Male Color")]
        public string MaleDisplayColorCode { get; set; }

        [Required]
        [Display(Name = "Female Color")]
        public string FemaleDisplayColorCode { get; set; }         
        [Display(Name = "Organization")]
        public String Organization { get; set; }
        public bool Active { get; set; }




    }

}
