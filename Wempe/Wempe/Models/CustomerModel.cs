using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class CustomerModel
    {
        public long CustomerNumber { get; set; }
        
        public string title { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string middleInitial { get; set; }
        [Required]
        public string lastName { get; set; }

        public string asstTitle { get; set; }
        [Required]
        public string asstFirstName { get; set; }
        [Required]
        public string asstMiddleInitial { get; set; }
        [Required]
        public string asstLastName { get; set; }

    }
}