using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class AvailableLaborMarketFileVersionModel
    {
        public Int32 AvailableLaborMarketFileVersionId { get; set; }
        public Int32 OrganizationId { get; set; }
        public DateTime SubmissionDateTime { get; set; }
        public Int32 FileVersionNumber { get; set; }
        public String Notes { get; set; }
        public Boolean Active { get; set; }
        public List<AvailableLaborMarketEEOJobCategoryModel> ListAvailableLaborMarketEEOJobCategory { get; set; }
    }
    public class AvailableLaborMarketEEOJobCategoryModel
    {
        public Int32 EEOJobCategoryId { get; set; }
        public String EEOJobCategoryName { get; set; }
        public Int32 EEOJobCategoryNumber { get; set; }
        public List<AvailableLaborMarketDataModel> ListAvailableLaborMarketData { get; set; }
    }
    public class AvailableLaborMarketDataModel
    {
        public Int32 AvailableLaborMarketDataId { get; set; }
        public Int32 EEOJobCategoryId { get; set; }
        public Int32 RaceId { get; set; }
        public String RaceName { get; set; }
        public Int32? MaleValue { get; set; }
        public Int32? FemaleValue { get; set; }
    }
}
