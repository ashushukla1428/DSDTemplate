using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class CustomPrincipalSerializeModel
    {
        public Int64 UserId  { get; set; }
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
        public Int64 OwnerID { get; set; }
        public Boolean IsMainUser { get; set; }
    }
}