using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EEOCompensationReportModel
    {
        public String OrganizationName { get; set; }
        public List<JobTitleForEEOCompensation> ListJobTitleForEEOCompensation { get; set; }
        public List<RacesForEEOCompensation> ListOfRaces { get; set; }
        public List<EEOCompensation> ListEEOCompensation { get; set; }

        public List<EEOCompensationEmployeeCount> ListEEOCompensationEmployeeCount { get; set; }
    }
    public class EEOCompensation
    {
        public int RacesId { get; set; }
        public string JobTitleName { get; set; }
        public int FTSMale { get; set; }
        public int FTSFemale { get; set; }

        public decimal HighMale { get; set; }
        public decimal HighFemale { get; set; }
        public decimal MediumMale { get; set; }
        public decimal MediumFemale { get; set; }
        public decimal LowMale { get; set; }
        public decimal LowFemale { get; set; }

        public decimal ParityMale { get; set; }
        public decimal ParityFemale { get; set; }
        public decimal DifferenceMale { get; set; }
        public decimal DifferenceFemale { get; set; }
        public decimal PercentageMale { get; set; }
        public decimal PercentageFemale { get; set; }
    }
    public class RacesForEEOCompensation
    {
        public int RacesId { get; set; }
        public int RacesNumber { get; set; }
        public string RacesName { get; set; }
    }
    public class JobTitleForEEOCompensation
    {
      //  public int EEOJobCategoryId { get; set; }
      //  public string ProgramOffice { get; set; }
        public string JobTitleName { get; set; }
        public int? TotalEmployee { get; set; }
    }
    public class EEOCompensationEmployeeCount
    {
        public int EmployeeCount { get; set; }
        public string JobTitleName { get; set; }
    }

}
