using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.CommonClasses
{
    public static class MainSetting
    {
        public static int pageSize = 55;
    }
    public class SortingFields
    {
        public string sortColumn { get; set; }
        public string sortOrder { get; set; }
        public int pageNo { get; set; }
    }
    public class Result
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public static class Messages
    {
        public static string accessDenied = "Access Denied!";
        public static string recordSaved = "record saved successfully";
        public static string recordDeleted = "record deleted successfully";
        public static string recordAlreadyExists = "record already exists!";
    }
}