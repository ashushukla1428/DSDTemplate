using System;
using System.ComponentModel.DataAnnotations;

namespace dsdProjectTemplate.ViewModel
{
    public class ActionsHistoryViewModel:BaseModel
    {
        public string TableName { get; set; }
        public string Data { get; set; }
        public long OrganizationId { get; set; }
        public long PrimaryKeyId { get; set; }
        public string ActionType { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime ActionDate { get; set; }
    }
    public class ActionsHistorySearch
    {
        public long OrganizationId { get; set; }
        public string TableName { get; set; }
    }
}
