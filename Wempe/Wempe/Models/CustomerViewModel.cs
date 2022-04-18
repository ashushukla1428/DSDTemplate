using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class CustomerViewModel: wmpCustomer
    {
        public string DocumentType { get; set; }
        public string DocumentFullName { get; set; }
    }
}