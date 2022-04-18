using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class PositionYearsOfServiceModel
    {
        [ScaffoldColumn(false)]
        public Int32 PositionYearsOfServiceId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [UIHint("Integer")]
        [Required]
        [Display(Name = "Position Years Of Service Number")]
        public Int32 Number { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }


        [Display(Name = "Min Value")]
        public decimal MinValue { get; set; }

        [Display(Name = "Max Value")]
        public decimal MaxValue { get; set; }



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

        // [ScaffoldColumn(false)]
        //public Int32 CreateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime CreateDateTime { get; set; }
        //[ScaffoldColumn(false)]
        //public Int32 UpdateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime UpdateDateTime { get; set; }



    }

}
