using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class ZipcodeCountryStateCityModel
    {

        public string CountryId { get; set; }
        public string StateId { get; set; }

        public string CityId { get; set; }

    }


    public class CityModelNew
    {

        public long Id { get; set; }
        public string city { get; set; }
        public bool? IsActive { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<long> OwnerID { get; set; }
        public Nullable<long> StateId { get; set; }
        public Nullable<long> CountyID { get; set; }

        public string ZipCode { get; set; }


        public Nullable<long> CountryId { get; set; }

    }
}