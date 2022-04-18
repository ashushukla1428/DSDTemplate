using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EEORatingModel
    {
        [ScaffoldColumn(false)]
        public Int32 EEORatingId { get; set; }

        [UIHint("OrganisationList")]
        [Required]
        [Display(Name = "Organization")]
        public Int32 OrganizationId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public String OrganizationName { get; set; }

        [UIHint("EEORatingTypeList")]
        [Required]
        [Display(Name = "EEORatingType")]
        public Int32 EEORatingTypeId { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "EEORatingType")]
        public String EEORatingTypeName { get; set; }

        [Required]
        [Display(Name = "Non Supervisor Label")]
        public string NonSupervisorLabelDisplay { get; set; }
        [Required]
        [Display(Name = "Race Label")]
        public string RaceLabelDisplay { get; set; }
        [Required]
        [Display(Name = "Gender Label")]
        public string GenderLabelDisplay { get; set; }
        [Required]
        [Display(Name = "Gender And Race Label")]
        public string GenderAndRaceLabelDisplay { get; set; }

        [Required]
        [Display(Name = "Race Value Indicator")]
        public Decimal? RaceValueIndicator { get; set; }
        [Required]
        [Display(Name = "Gender Value Indicator")]
        public Decimal? GenderValueIndicator { get; set; }
        [Required]
        [Display(Name = "Non Supervisor Color")]
        public string NonSupervisorColorCode { get; set; }         
        [Required]
        [Display(Name = "Race Color")]
        public string RaceColorCode { get; set; }
        [Required]
        [Display(Name = "Gender Color")]
        public string GenderColorCode { get; set; }
        [Required]
        [Display(Name = "Gender & Race Color")]
        public string GenderAndRaceColorCode { get; set; }

        public String Remarks { get; set; }
        public bool Active { get; set; }
        

    }

}
