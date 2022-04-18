using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel.Organization
{
    public class OrganizationYearsViewModel: OrganizationBaseModel
    {
        [Display(Name = "Begin Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Long Description")]
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }
        
    }
}
