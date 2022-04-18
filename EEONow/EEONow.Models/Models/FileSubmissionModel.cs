using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models
{
    public class FileSubmissionModel
    {

        public Int32 FileSubmissionId { get; set; }

        [UIHint("OrganisationList")]
        [Required]
        [Display(Name = "Organization")]
        public Int32 OrganizationId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public String OrganizationName { get; set; }

        [UIHint("FileSubmissionStatusList")]
        [Required]
        [Display(Name = "File Submission Status")]
        public Int32 FileSubmissionStatusId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "File Submission Status")]
        public String FileSubmissionStatus { get; set; }

        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; }

        [Display(Name = "New File Name")]
        public string NewFileName { get; set; }

        [UIHint("Integer")]
        [Required]
        [Display(Name = "File Version Number")]
        public Int32 FileVersionNumber { get; set; }
    }
}
