using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
    }
    public class PageingModel
    {
        public Nullable<bool> IsActive { get; set; }
        public string LastUpdate { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public string UpdateByUserName { get; set; }
        public int TotalCount { get; set; }

        public bool Status { get; set; }
        public string Message { get; set; }
        public Int64? brandId { get; set; }
    }
}