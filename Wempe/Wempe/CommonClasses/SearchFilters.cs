using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.CommonClasses
{
    public class SearchFilters : SortingFields
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string UserType { get; set; }
        public int? BrandId { get; set; }
        public string ActiveOrAllCheck { get; set; }

        public int? CompanyId { get; set; }
    }
    public class ManageRightsFilters : SearchFilters
    {
        public int RoleID { get; set; }
    }
    public class SearchCustomerFilters : SortingFields
    {
        public string SearchFields { get; set; }
        public string SearchValues { get; set; }
    }
    public class StateFilters : SearchFilters
    {
        public int CountryId { get; set; }
    }
    public class CityFilters : SearchFilters
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
    }


    public class ZipcodeFilters : SearchFilters
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }

        public int CountyId { get; set; }

        public int CityId { get; set; }
    }


    public class SearchRepairFilters : SortingFields
    {
        public string SearchFields { get; set; }
        public string SearchValues { get; set; }
        public long CustomerNumber { get; set; }


        public string dueDateCustomerStart { get; set; }
        public string dueDateCustomerEnd { get; set; }
        public string dueDateSupplierStart { get; set; }
        public string dueDateSupplierEnd { get; set; }
        public string EntryDateStart { get; set; }
        public string EntryDateEnd { get; set; }
    }

    public class PaymentHistory : SearchFilters
    {
        public int CompanyId { get; set; }

    }

    public class PaymentHistoryModel 
    {
        public long CompanyId { get; set; }
        public string PackageStartDate { get; set; }
        public string PackageRenewalDate { get; set; }
        public long UserLimit { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public long OwnerId { get; set; }
        public string Payment { get; set; }
        public string PackagePeriod { get; set; }

        public long Id { get; set; }
        public bool IsEditPaymentHistoryRecord { get; set; }

    }
}