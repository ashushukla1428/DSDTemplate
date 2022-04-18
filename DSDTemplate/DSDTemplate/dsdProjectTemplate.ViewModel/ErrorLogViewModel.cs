using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel
{
    public class ErrorLogViewModel
    {
        public long Id { get; set; }
        public string LogType { get; set; }
        public string LogTitle { get; set; }
        public string LogMessage { get; set; }
        public string ErrorSource { get; set; }        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> LogDate { get; set; }
    }
}
