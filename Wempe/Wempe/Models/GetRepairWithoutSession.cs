using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wempe.CommonClasses;

namespace Wempe.Models
{
    public class GetRepairWithoutSession : SortingFields
    {
        public string Email { get; set; }
        public string RepairNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string ZipCode { get; set; }
    }

    public class RepairWithoutSession : PageingModel
    {
        public string entryDate { get; set; }
        public long customerNumber { get; set; }

        public string brandName { get; set; }

        public string RepairStatus { get; set; }

        public long repairNumber { get; set; }

        public string Phone { get; set; }

        public string CustomerName { get; set; }

        public string EmailAddress { get; set; }


        public string LogId { get; set; }
    }
}