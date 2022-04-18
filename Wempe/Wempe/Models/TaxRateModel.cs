using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class TaxRateModel : PageingModel
    {
        public Int64 Id { get; set; }
        public string taxRate { get; set; }
        public Int64? StateId { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }

    }
}