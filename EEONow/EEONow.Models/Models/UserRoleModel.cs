using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class UserRoleModel
    {
        [ScaffoldColumn(false)]
        public Int32 RoleId { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public bool Active { get; set; }
        //[Display(Name = "Is Filter")]
        public bool IsFilter { get; set; }
        //[Display(Name = "Is Add")]
        public bool IsAdd { get; set; }
        //[Display(Name = "Is Edit")]
        public bool IsEdit { get; set; }

        [UIHint("OrganisationList")]
        [Display(Name = "Organization")]
        public Int32 OrganizationId { get; set; }
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
