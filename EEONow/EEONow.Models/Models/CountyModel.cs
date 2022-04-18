using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class CountyModel
    {
        [ScaffoldColumn(false)]
        public Int32 CountyId { get; set; }
        [Required]
        [Display(Name = "County Code")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "County Name")]
        public string Name { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
        public bool Active { get; set; }
       // [ScaffoldColumn(false)]
        //public Int32 CreateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime CreateDateTime { get; set; }
        //[ScaffoldColumn(false)]
        //public Int32 UpdateUserId { get; set; }
        //[ScaffoldColumn(false)]
        //public DateTime UpdateDateTime { get; set; }



    }

}
