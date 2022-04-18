using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class ViewIndexReportModel
    {

        public List<IndexTypeModel> RaceIndexList { get; set; }
        public List<IndexTypeModel> GenderIndexList { get; set; }
        public String EmployeeName { get; set; }
        public decimal? EEORaceIndexRating { get; set; }
        public decimal? EEOGenderIndexRating { get; set; }

    }
    public class IndexTypeModel
    {
        public String TypeName { get; set; }
        public decimal? CurrentCount { get; set; }
        public decimal? ALMPercentage { get; set; }
        public decimal? CurrentPercentage { get; set; }
    }

}
