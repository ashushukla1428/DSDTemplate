using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{

    public class UserModel : PageingModel
    {
        public Int64 Id { get; set; }
        public string employee { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        public string Role { get; set; }
    }

    public class UserTrackModel : PageingModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public DateTime? LoginTime { get; set; }


        public string LoginTimeString { get; set; }

        public DateTime? LogoutTime { get; set; }

        public string LogoutTimeString { get; set; }

        public string Type { get; set; }

        public string TotalTime { get; set; }
    }


    public class ErrorTrackModel : PageingModel
    {
        public Int64 ErrorId { get; set; }
        public string PageName { get; set; }
        public string TimeStamp { get; set; }
        public string ErrorMessage { get; set; }
    }


    public class SuggestionModel : PageingModel
    {
        public Int64 SuggestionId { get; set; }
        public string Suggestion { get; set; }
        public string TimeStamp { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
    }
}