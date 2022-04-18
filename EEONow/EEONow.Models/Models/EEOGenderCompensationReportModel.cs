using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EEOGenderCompensationReportModel
    {
        public String OrganizationName { get; set; }
        public List<JobTitleForEEOGenderCompensation> ListJobTitleForEEOGenderCompensation { get; set; }      
        public List<EEOGenderCompensation> ListEEOGenderCompensation { get; set; }
        public List<EEOGenderCompensationEmployeeCount> ListEEOGenderCompensationEmployeeCount { get; set; }
    }
    public class EEOGenderCompensation
    {
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
    
    public class JobTitleForEEOGenderCompensation
    {
        //public int EEOJobCategoryId { get; set; }
        //public string ProgramOffice { get; set; }
        public string JobTitleName { get; set; }
        public int? TotalEmployee { get; set; }
    }
    public class EEOGenderCompensationEmployeeCount
    {
        public int EmployeeCount { get; set; }
        public string JobTitleName { get; set; }
    }

}
