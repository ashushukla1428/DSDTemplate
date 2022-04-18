using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EEOReportbyRegionModel
    {
        public String OrganizationName { get; set; }
        public String RegionName { get; set; }
        public List<EEOForRegion> ListEEOForRegion { get; set; }
        public List<RacesForRegion> ListRacesForRegion { get; set; }
        public List<ComputeRegionValue> ListComputeRegionValue { get; set; }
    }
    public class ComputeRegionValue
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int TotalWorkforceMale { get; set; }
        public int TotalWorkforceFemale { get; set; }
        public int TotalSelectedEEO { get; set; }
        //public int? AMLMale { get; set; }
        //public int? AMLFemale { get; set; }
        //public int? AMLSelectedEEO { get; set; }
    }
    public class RacesForRegion
    {
        public int RacesId { get; set; }
        public string RacesName { get; set; }
    }
    public class EEOForRegion
    {
        public int EEOId { get; set; }
        public string EEOName { get; set; }
    }
    public class EmployeeForRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class CountedRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int? MaleCount { get; set; }
        public int? FemaleCount { get; set; }
    }
    public class EmployeeWithCountedRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int? TotalMale { get; set; }
        public int? TotalFemale { get; set; }
    }


    public class EmployeeWithTotalWorkforceByRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int TotalWorkforceCount { get; set; }
    }
    public class EmployeeWithSelectedEEOByRegion
    {

        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int TotalEmployeeWithSelectedEEOCount { get; set; }
    }

}