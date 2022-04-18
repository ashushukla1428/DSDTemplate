using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class LinkMode
    {
        public string Link { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Logo { get; set; }
        public string CompanyName { get; set; }


    }


    public class PasswordChangedByOther
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ByName { get; set; }
        public string ByRole { get; set; }
        public string NewPassword { get; set; }



    }
}