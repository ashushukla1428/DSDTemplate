using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class CompanyModel : PageingModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        public string PackageStartDate { get; set; }
        public string PackageRenewalDate { get; set; }

        public string ContactPerson { get; set; }



    }
}