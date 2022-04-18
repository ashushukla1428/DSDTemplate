using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class PublicURLModel
    {
        [ScaffoldColumn(false)]
        public Int32 PublicURLId { get; set; }
        [ScaffoldColumn(false)]
        public Int32 OrgainisationId { get; set; }
        [Display(Name = "Name")]
        public string OrgainisationName { get; set; }
        [ScaffoldColumn(false)]
        public string Token { get; set; }
        [Display(Name = "Public URL")]
        public string PublicLink { get; set; } 
        public bool Active { get; set; }
    }

}
