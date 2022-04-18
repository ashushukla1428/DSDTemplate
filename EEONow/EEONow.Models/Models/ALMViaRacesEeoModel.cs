using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class ALMViaRacesEeoModel
    {
        public String ManagerName { get; set; }
        public List<EEOForALM> ListEEOForALM { get; set; }
        public List<RacesForALM> ListRacesForALM { get; set; }
        public List<ComputeALMValue> ListComputeALMValue { get; set; }
    }
    public class ComputeALMValue
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }

        public int TotalWorkforceMale { get; set; }
        public int TotalWorkforceFemale { get; set; }

        //public int TotalSelectedEEOMale { get; set; }
        //public int TotalSelectedEEOFemale { get; set; }

        public int TotalSelectedEEO { get; set; }

        public int? AMLMale { get; set; }
        public int? AMLFemale { get; set; }

        //public int? AMLSelectedEEOMale { get; set; }
        //public int? AMLSelectedEEOFemale { get; set; }
        public int? AMLSelectedEEO { get; set; }
    }
    public class RacesForALM
    {
        public int RacesId { get; set; }
        public string RacesName { get; set; }
    }
    public class EEOForALM
    {
        public int EEOId { get; set; }
        public string EEOName { get; set; }
    }
    public class EmployeeForALM
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Salary { get; set; }
    }

    public class CountedALM
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int? MaleCount { get; set; }
        public int? FemaleCount { get; set; }
    }
    public class EmployeeWithCountedALM
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; } 
        public int? TotalMale { get; set; }
        public int? TotalFemale { get; set; }
    }


    public class EmployeeWithTotalWorkforce
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int TotalWorkforceCount { get; set; }
    }
    public class EmployeeWithSelectedEEO
    {
        
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int TotalEmployeeWithSelectedEEOCount { get; set; }
    }

}
