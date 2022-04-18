using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class EEOProgressReportByRegionModel
    {
        public String OrganizationName { get; set; }

        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String RegionName { get; set; }
        public List<EEOForProgressRegion> ListEEOForProgressRegion { get; set; }
        public List<RacesForProgressRegion> ListRacesForProgressRegion { get; set; }
        public List<ComputeProgressRegionValue> ListComputeProgressRegionValue { get; set; }

        public List<RegionEmployeeDifference> ListRegionEmployeeDifference { get; set; }
        
    }
    public class ComputeProgressRegionValue
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int BeginEnd { get; set; }
        public int TotalEmployeeMale { get; set; }
        public int TotalEmployeeFemale { get; set; }

        public double PercentageEmployeeMale { get; set; }
        public double PercentageEmployeeFemale { get; set; }

    }
    public class RegionEmployeeDifference
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int DifferencesMale { get; set; }
        public int DifferencesFemale { get; set; }
        public double PercentageDifferenceMale { get; set; }
        public double PercentageDifferenceFemale { get; set; }
    }
    public class RacesForProgressRegion
    {
        public int RacesId { get; set; }
        public string RacesName { get; set; }
    }
    public class EEOForProgressRegion
    {
        public int EEOId { get; set; }
        public string EEOName { get; set; }
    }





    public class EmployeeForProgressRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int EmployeeId { get; set; }
        public int BeginEnd { get; set; }
    }
    public class TotalEmployeegForProgressRegion
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }
        public int GenderId { get; set; }
        public int BeginEnd { get; set; }
        public int TotalEmployeeCount { get; set; }
    }


}