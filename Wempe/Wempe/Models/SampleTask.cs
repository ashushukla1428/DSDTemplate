using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class SampleTask
    {

     
        public long Id { get; set; }
        public string description { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<long> OwnerID { get; set; }
  
    }
}