using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class StaffLevelReportModel
    {
        public String OrganizationName { get; set; }
        public List<RacesForStaffLevel> ListOfRaces { get; set; }
        public List<StaffLevel> ListStaffLevel { get; set; }
        public List<NumberOfStaffEmployee> NumberOfStaffEmployee { get; set; }
    }
    public class StaffLevel
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
    }
    public class RacesForStaffLevel
    {
        public int RacesId { get; set; }
        public string RacesName { get; set; }
    }
    public class NumberOfStaffEmployee
    {
        public int RacesId { get; set; }
        public string RaceName { get; set; }
        public int LevelId { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int NoOfStaff { get; set; }
    }

}
