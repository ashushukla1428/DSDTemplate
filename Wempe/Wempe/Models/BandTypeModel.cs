using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class BandTypeModel : PageingModel
    {
        public long BandTypeID { get; set; }
        public string BandType { get; set; }

    }
}