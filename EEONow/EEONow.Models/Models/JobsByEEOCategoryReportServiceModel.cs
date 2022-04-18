using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class JobsByEEOCategoryReportModel
    {
        public String OrganizationName { get; set; } 
        public List<EEOJobsCategory> ListJobsByEEOCategory { get; set; }
        public List<PositionAndProgram> ListPositionAndProgram { get; set; }
    }
    public class EEOJobsCategory
    {
        public int EEOJobCategoryNumber { get; set; }
        public string EEOJobCategoryName { get; set; }
    } 
    public class PositionAndProgram
    {
        public int EEOJobCategoryNumber { get; set; } 
        public List<string> ProgramOfficeName { get; set; }
        public string PositionTitle { get; set; }
    }

}
