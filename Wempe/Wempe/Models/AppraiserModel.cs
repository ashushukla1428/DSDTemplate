using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wempe.CommonClasses;

namespace Wempe.Models
{
    
    public class AppraiserModel : PageingModel
    {
        public long AppraiserID { get; set; }
        public string AppraiserTitle { get; set; }
        
    }
    public partial class MenuModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public Nullable<long> PageID { get; set; }
        public string PageName { get; set; }
        public Nullable<int> parentID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> MenuIndex { get; set; }
        public string ParentName { get; set; }
    }
}