using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class DashboardModel
    {
        public string position { get; set; }
        public string parent { get; set; }
        public string level { get; set; }
        public string color { get; set; }
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string picturePath { get; set; }
        public string gender { get; set; }
        public string race { get; set; }
        public string age { get; set; }
        public string positionTitle { get; set; }
        public string programOffice { get; set; }
        public string salary { get; set; }
        public string eeo4Code { get; set; }
        public string lastPerformanceRating { get; set; }
        public string yearsOfService { get; set; }
        public string agency { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string supervisorFlag { get; set; }
        public string vacantPositions { get; set; }
        public string staffHeadCount { get; set; }
        public string spanOfControl { get; set; }
        public string supervisorStaffRatio { get; set; }
        public string levelsUp { get; set; }
        public string levelsDown { get; set; }
        public string raceid { get; set; }
        public string tysid { get; set; }
        public string eeoid { get; set; }

    }

    public class OrhChartDashborad
    {
        public int FileSubmissionId { get; set; }
        public int OrganizationId { get; set; }
        public String OrganizationName { get; set; }
        public String EffectiveDate { get; set; }
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public String FilePath { get; set; }
        public List<SelectListItem> ListFileSubmission { get; set; }
    }
}
