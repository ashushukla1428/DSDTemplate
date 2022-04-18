using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models
{
    public class FileSubmissionErrorsModel
    {
        public Int32 FileSubmissionErrorId { get; set; }
        public Int32 FileSubmissionRecordNumber { get; set; }
        public Int32 FileSubmissionId { get; set; }
        public string ErrorDescription { get; set; }
    }
}
