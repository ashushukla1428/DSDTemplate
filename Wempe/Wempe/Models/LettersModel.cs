using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wempe.Models
{
    public class LettersModel
    {
    }
    public class ResportMainHeadrs
    {
        public string RepairNumber { get; set; }
        public string TicketNumber { get; set; }
        public string OrderNumber { get; set; }
    }
    public class CustomerReceiptEnvelope:wmpPrintSetting{
        public ResportMainHeadrs resportMainHeadrs { get; set; }
        public CustomerDetail customerDetail { get; set; }
        public vwRepair itemDetail { get; set; }
        public SupplierDetail supplierDetail { get; set; }
        public StoreNameAndAddress storeNameAndAddress { get; set; }
        public bool fullReport { get; set; }
        public string withLetterHead { get; set; }
        public List<wmpRepairLog> RepairLog { get; set; }
        
    }
    public class CustomerDetail 
    {
        public string title { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }

        public string priCompany { get; set; }
        public string priAddressLine1 { get; set; }
        public string priAddressLine2 { get; set; }

        public string priCountry { get; set; }
        public string priState { get; set; }
        public string priCity { get; set; }
        public string priZipCode { get; set; }
        public string priZipCodePlusFour { get; set; }
        public string priTelephoneSegment1 { get; set; }
        public string priTelephoneSegment2 { get; set; }
        public string priTelephoneSegment3 { get; set; }
        public string priTelephoneExtension { get; set; }

        public string priCellphoneSegment1 { get; set; }
        public string priCellphoneSegment2 { get; set; }
        public string priCellphoneSegment3 { get; set; }

        public string priFaxSegment1 { get; set; }
        public string priFaxSegment2 { get; set; }
        public string priFaxSegment3 { get; set; }

        public DateTime entryDate { get; set; }

        
    }

    public class ItemDetail {
        public string  brandName { get; set; }
        public string item { get; set; }
        public string style { get; set; }
        public string movementType { get; set; }
        public string caseType { get; set; }
        public string bandType { get; set; }
        public string buckleType { get; set; }
        public string dialType { get; set; }
        public string crystalCondition { get; set; }
        public string bezelCondition { get; set; }
        public string backCondition { get; set; }
        public string lugsCondition { get; set; }
        public string caseCondition { get; set; }
        public string movementSerialNumber { get; set; }
        public string caliber { get; set; }
        public string numberOfJewels { get; set; }
        public string bandCondition { get; set; }
        public string buckleCondition { get; set; }
        public string dialCondition { get; set; }
        public string serialNumber { get; set; }
    }
    public class SupplierDetail
    {
        public string name { get; set; }
        public string mainWempeAccount { get; set; }
        public string mainAddressLine1 { get; set; }
        public string mainAddressLine2 { get; set; }
        public string mainCity { get; set; }
        public string mainState { get; set; }
        public string mainZipCode { get; set; }
        public string mainCountry { get; set; }
        public string mainFaxSegment1 { get; set; }
        public string mainFaxSegment2 { get; set; }
        public string mainFaxSegment3 { get; set; }
        public string mainTelephoneSegment1 { get; set; }
        public string mainTelephoneSegment2 { get; set; }
        public string mainTelephoneSegment3 { get; set; }
        public string mainTelephoneExtension { get; set; }

    }
    public class StoreNameAndAddress
    {
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public string country { get; set; }
    }
}