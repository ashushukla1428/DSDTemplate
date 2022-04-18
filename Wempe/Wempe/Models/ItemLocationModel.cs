using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class ItemLocationModel : PageingModel
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string mainCompany { get; set; }
    }
}