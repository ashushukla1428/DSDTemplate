using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class GetRepairStatusLogModel:PageingModel
    {
       
            public long LogId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string RepairNumber { get; set; }
            public string Zipcode { get; set; }
            public string TimeStamp { get; set; }
            public string Type { get; set; }
            public string IPAddress { get; set; }
       
    }
}