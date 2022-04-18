using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class BackConditionModel : PageingModel
    {
        public long BackConditionID { get; set; }
        public string BackCondition { get; set; }
        
    }
}