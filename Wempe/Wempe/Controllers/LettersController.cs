using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class LettersController : Controller
    {
        //
        // GET: /Letters/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult Index(int callType, Int64 repairNumber, string priAddress, string withLetterHead)
        {
           // string _id = Request.Cookies["someCookie"];
            CustomerReceiptEnvelope model = new CustomerReceiptEnvelope();
            wmpPrintSetting _model = db.wmpPrintSettings.Where(c => c.OwnerID == SessionMaster.Current.OwnerID).FirstOrDefault();
            wmpRepair repair = db.wmpRepairs.Where(c => c.repairNumber == repairNumber).FirstOrDefault();
          
            switch (callType)
            {
                #region _customerReceiptEnvelope
                case 1://_customerReceiptEnvelope

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                    return View("_customerReceiptEnvelope", model);
                #endregion
                #region _customerReceiptEnvelope
                case 2://_customerReceiptEnvelope
                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = true;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_customerReceiptEnvelope", model);
                #endregion
                #region _letterToCustomer
                case 3://_letterToCustomer
                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = true;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                  
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_letterToCustomer", model);
                #endregion
                #region _repairSummary
                case 4://_repairSummary

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_repairSummary", model);
                #endregion
                #region _repairSummary
                case 5://_repairSummary

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = true;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     //Supplier Account detail 

                     ViewBag.wempeAccount = db.wmpSuppliers.Where(c => c.Id == repair.supplierId).FirstOrDefault().mainWempeAccount;
                     Int64 purchaseLocationId = Convert.ToInt64(repair.purchaseLocationId);
                     if (purchaseLocationId != 0)
                     {
                         ViewBag.purchaseLocation = db.wmpPurchaseLocations.Where(c => c.Id == purchaseLocationId).FirstOrDefault().name;
                     }

                    if (repair.warrantyDocumentId != null)
                    {
                        ViewBag.warrantyDocument = db.wmpWarrantyDocuments.Where(c => c.Id == unchecked((Int64)repair.warrantyDocumentId)).FirstOrDefault().WarrantyDocument;
                    }

                    return View("_repairSummary", model);
                #endregion
                #region _envelopeToCustomer
                case 6://_envelopeToCustomer

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_envelopeToCustomer", model);
                #endregion
                #region _envelopeToSupplier
                case 7://_envelopeToSupplier

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     //SupplierDetail
                     var supplierDetail = db.wmpSuppliers.Where(c => c.Id == repair.supplierId).FirstOrDefault();
                     if (supplierDetail != null)
                     {
                         SupplierDetail _supplierDetail = new SupplierDetail()
                         {
                             name = supplierDetail.name,
                             mainAddressLine1 = supplierDetail.mainAddressLine1,
                             mainAddressLine2 = supplierDetail.mainAddressLine2,
                             mainCity = supplierDetail.mainCity,
                             mainCountry = supplierDetail.mainCountry,
                             mainFaxSegment1 = supplierDetail.mainFaxSegment1,
                             mainFaxSegment2 = supplierDetail.mainFaxSegment2,
                             mainFaxSegment3 = supplierDetail.mainFaxSegment3,
                             mainState = supplierDetail.mainState,
                             mainWempeAccount = supplierDetail.mainWempeAccount,
                             mainZipCode = supplierDetail.mainZipCode,
                             mainTelephoneExtension = supplierDetail.mainTelephoneExtension,
                             mainTelephoneSegment1 = supplierDetail.mainTelephoneSegment1,
                             mainTelephoneSegment2 = supplierDetail.mainTelephoneSegment2,
                             mainTelephoneSegment3 = supplierDetail.mainTelephoneSegment3
                         };
                         model.supplierDetail = _supplierDetail;
                     }
                     else
                     {
                         model.supplierDetail = new SupplierDetail ();
                     }
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_envelopeToSupplier", model);
                #endregion
                #region _returnEnvelopeFromCustomer
                case 8://_returnEnvelopeFromCustomer

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }

                     StoreNameAndAddress _storeNameAndAddress = new StoreNameAndAddress();
                     var storeNameAndAddess = db.wmpStoreInformations.Where(c => c.storeNumber == "1").FirstOrDefault();
                     if (storeNameAndAddess != null)
                     {
                         _storeNameAndAddress = new StoreNameAndAddress() { 
                             city=storeNameAndAddess.city,
                             country=storeNameAndAddess.country,
                             addressLine1=storeNameAndAddess.addressLine1,
                             addressLine2=storeNameAndAddess.addressLine2,
                             name=storeNameAndAddess.name,
                             state=storeNameAndAddess.state,
                             zipCode=storeNameAndAddess.zipCode
                         };
                     }
                     model.storeNameAndAddress = _storeNameAndAddress;
                     return View("_returnEnvelopeFromCustomer", model);
                #endregion
                #region _returnEnvelopeFromSupplier
                case 9://_returnEnvelopeFromSupplier

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }

                      var supplierDetail1 = db.wmpSuppliers.Where(c => c.Id == repair.supplierId).FirstOrDefault();
                      if (supplierDetail1 != null)
                     {
                         SupplierDetail _supplierDetail = new SupplierDetail()
                         {
                             name = supplierDetail1.name,
                             mainAddressLine1 = supplierDetail1.mainAddressLine1,
                             mainAddressLine2 = supplierDetail1.mainAddressLine2,
                             mainCity = supplierDetail1.mainCity,
                             mainCountry = supplierDetail1.mainCountry,
                             mainFaxSegment1 = supplierDetail1.mainFaxSegment1,
                             mainFaxSegment2 = supplierDetail1.mainFaxSegment2,
                             mainFaxSegment3 = supplierDetail1.mainFaxSegment3,
                             mainState = supplierDetail1.mainState,
                             mainWempeAccount = supplierDetail1.mainWempeAccount,
                             mainZipCode = supplierDetail1.mainZipCode,

                             mainTelephoneExtension=supplierDetail1.mainTelephoneExtension,
                             mainTelephoneSegment1 = supplierDetail1.mainTelephoneSegment1,
                             mainTelephoneSegment2 = supplierDetail1.mainTelephoneSegment2,
                             mainTelephoneSegment3 = supplierDetail1.mainTelephoneSegment3
                         };
                         model.supplierDetail = _supplierDetail;
                     }
                     else
                     {
                         model.supplierDetail = new SupplierDetail ();
                     }

                      StoreNameAndAddress _storeNameAndAddress1 = new StoreNameAndAddress();
                     var storeNameAndAddess1 = db.wmpStoreInformations.Where(c => c.storeNumber == "1").FirstOrDefault();
                     if (storeNameAndAddess1 != null)
                     {
                         _storeNameAndAddress1 = new StoreNameAndAddress() { 
                             city=storeNameAndAddess1.city,
                             country=storeNameAndAddess1.country,
                             addressLine1=storeNameAndAddess1.addressLine1,
                             addressLine2=storeNameAndAddess1.addressLine2,
                             name=storeNameAndAddess1.name,
                             state=storeNameAndAddess1.state,
                             zipCode=storeNameAndAddess1.zipCode
                         };
                     }
                     model.storeNameAndAddress = _storeNameAndAddress1;

                     return View("_returnEnvelopeFromSupplier", model);
                #endregion
                #region _shippingSlipToCustomer
                case 10://_shippingSlipToCustomer
                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = true;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_shippingSlipToCustomer", model);
                #endregion
                #region _shippingSlipToCustomer
                case 11://_shippingSlipToCustomer
                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = true;
                     model.withLetterHead = withLetterHead;
                      var supplierDetail2 = db.wmpSuppliers.Where(c => c.Id == repair.supplierId).FirstOrDefault();
                      if (supplierDetail2 != null)
                     {
                         SupplierDetail _supplierDetail = new SupplierDetail()
                         {
                             name = supplierDetail2.name,
                             mainAddressLine1 = supplierDetail2.mainAddressLine1,
                             mainAddressLine2 = supplierDetail2.mainAddressLine2,
                             mainCity = supplierDetail2.mainCity,
                             mainCountry = supplierDetail2.mainCountry,
                             mainFaxSegment1 = supplierDetail2.mainFaxSegment1,
                             mainFaxSegment2 = supplierDetail2.mainFaxSegment2,
                             mainFaxSegment3 = supplierDetail2.mainFaxSegment3,
                             mainState = supplierDetail2.mainState,
                             mainWempeAccount = supplierDetail2.mainWempeAccount,
                             mainZipCode = supplierDetail2.mainZipCode,
                             
                             mainTelephoneExtension=supplierDetail2.mainTelephoneExtension,
                             mainTelephoneSegment1=supplierDetail2.mainTelephoneSegment1,
                             mainTelephoneSegment2=supplierDetail2.mainTelephoneSegment2,
                             mainTelephoneSegment3=supplierDetail2.mainTelephoneSegment3
                         };
                         model.supplierDetail = _supplierDetail;
                     }
                     else
                     {
                         model.supplierDetail = new SupplierDetail ();
                     }


                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_shippingSlipToSupplier", model);
                #endregion
                #region _envelopeToCustomer
                case 12://_envelopeToCustomer

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     return View("_appraisal", model);
                #endregion
                #region _envelopeToCustomer
                case 13://_envelopeToCustomer

                     model = M_receiptToClientEnvelope(repairNumber, priAddress);
                     model.fullReport = false;
                     model.withLetterHead = withLetterHead;
                     if (_model != null)
                     {
                         model.PaperTopMargin = _model.PaperTopMargin;
                         model.PaperBottomMargin = _model.PaperBottomMargin;
                         model.PaperLeftMargin = _model.PaperLeftMargin;
                         model.PaperRightMargin = _model.PaperRightMargin;
                     }
                     List<wmpRepairLog> _repairLog = db.wmpRepairLogs.Where(c => c.repairNumber == repairNumber).ToList();
                     model.RepairLog = _repairLog;
                     return View("_repairLog", model);
                #endregion
                default:
                 return View("_customerReceiptEnvelope");
            }
            //return View("_customerReceiptEnvelope");
        }

        public ActionResult IndexAdmin(int callType, Int64 repairNumber, string priAddress)
        {
            CustomerReceiptEnvelope model = new CustomerReceiptEnvelope();
            switch (callType)
            {
                case 1:

                    // model = M_receiptToClientEnvelope(repairNumber, priAddress);
                    var _items = new GetToalAndPendingRepairList
              {
                  TotalRepairs = db.Database.SqlQuery<ReportSummary>("GetNoOfRepairs").ToList(),
                  PendingRepairs = db.Database.SqlQuery<PendingReportSummary>("GetNoOfPendingRepairs").ToList()
              };
                    ViewBag.SystemOverViewNumerofTotalRepairs = _items.TotalRepairs.Sum(s => s.NumberofRepairs);
                    ViewBag.SystemOverViewNumerofPendingRepairs = _items.PendingRepairs.Count();
                    ViewBag.Years = Enumerable.Range(2003, (DateTime.Now.AddYears(1).Year - 2003));
                    //return View(_items);
                    model.fullReport = false;
                    return View("_systemOverview", _items);
               


              
                default:
                    return View("_customerReceiptEnvelope");
            }
        }

        
        private CustomerReceiptEnvelope M_receiptToClientEnvelope(Int64 repairNumber, string priAddress)
        {
            CustomerReceiptEnvelope model = new CustomerReceiptEnvelope();
            ResportMainHeadrs resportMainHeadrs = new ResportMainHeadrs();
            resportMainHeadrs.RepairNumber = repairNumber.ToString();


            model.resportMainHeadrs = resportMainHeadrs;
            vwRepair item = db.vwRepairs.Where(c => c.repairNumber == repairNumber).FirstOrDefault();
            resportMainHeadrs.TicketNumber = item.ticketNumber;
            resportMainHeadrs.OrderNumber = item.OrderNumber;
            if (priAddress == "1")
            {
                //var _customer=db.wmpsta
                var _customer = (
                    from customer in db.wmpCustomers
                    where (customer.customerNumber == item.customerNumber)
                    from countries in db.wmpCountries.Where(countries => countries.Id == customer.priCountry).DefaultIfEmpty()
                    from states in db.wmpStates.Where(states => states.Id == customer.priState).DefaultIfEmpty()
                    select new
                    {
                        title = customer.title,
                        firstName = customer.firstName,
                        middleInitial = customer.middleInitial,
                        lastName = customer.lastName,
                        priCompany = customer.priCompany,
                        priAddressLine1 = customer.priAddressLine1,
                        priAddressLine2 = customer.priAddressLine2,
                        priCountry = countries.country,
                        priState = states.stateFullName,
                        priCity = customer.priCity,
                        priZipCode = customer.priZipCode,
                        priZipCodePlusFour = customer.priZipCodePlusFour,
                        priCellphoneSegment1 = customer.priCellphoneSegment1,
                        priCellphoneSegment2 = customer.priCellphoneSegment2,
                        priCellphoneSegment3 = customer.priCellphoneSegment3,
                        priTelephoneExtension = customer.priTelephoneExtension,

                        priFaxSegment1 = customer.priFaxSegment1,
                        priFaxSegment2 = customer.priFaxSegment2,
                        priFaxSegment3 = customer.priFaxSegment3,
                        entryDate=customer.entryDate
                    }).FirstOrDefault();

                CustomerDetail _detail = new CustomerDetail
                {
                    title = _customer.title,
                    firstName = _customer.firstName,
                    lastName = _customer.lastName,
                    middleInitial = _customer.middleInitial,
                    priAddressLine1 = _customer.priAddressLine1,
                    priAddressLine2 = _customer.priAddressLine2,
                    priCountry = _customer.priCountry,
                    priCompany = _customer.priCompany,
                    priState = _customer.priState,
                    priCity = _customer.priCity,
                    priZipCode = _customer.priZipCode,
                    priZipCodePlusFour = _customer.priZipCodePlusFour,
                    priCellphoneSegment1 = _customer.priCellphoneSegment1,
                    priCellphoneSegment2 = _customer.priCellphoneSegment2,
                    priCellphoneSegment3 = _customer.priCellphoneSegment3,
                    priTelephoneExtension = _customer.priTelephoneExtension,
                    entryDate=Convert.ToDateTime( _customer.entryDate)
                };
                model.customerDetail = _detail;
            }
            else
            {
                var _customer = (
                   from customer in db.wmpCustomers
                   where (customer.customerNumber == item.customerNumber)
                   from countries in db.wmpCountries.Where(countries => countries.Id == customer.secCountry).DefaultIfEmpty()
                   from states in db.wmpStates.Where(states => states.Id == customer.secState).DefaultIfEmpty()
                   select new
                   {
                       title = customer.title,
                       firstName = customer.firstName,
                       middleInitial = customer.middleInitial,
                       lastName = customer.lastName,
                       priCompany = customer.secCompany,
                       priAddressLine1 = customer.secAddressLine1,
                       priAddressLine2 = customer.secAddressLine2,
                       priCountry = countries.country,
                       priState = states.stateFullName,
                       priCity = customer.secCity,
                       priZipCode = customer.secZipCode,
                       priZipCodePlusFour = customer.secZipCodePlusFour,
                       priCellphoneSegment1 = customer.secCellphoneSegment1,
                       priCellphoneSegment2 = customer.secCellphoneSegment2,
                       priCellphoneSegment3 = customer.secCellphoneSegment3,
                       priTelephoneExtension = customer.secTelephoneExtension,

                       priFaxSegment1 = customer.secFaxSegment1,
                       priFaxSegment2 = customer.secFaxSegment2,
                       priFaxSegment3 = customer.secFaxSegment3,
                       entryDate = customer.entryDate
                   }).FirstOrDefault();

                CustomerDetail _detail = new CustomerDetail
                {
                    title = _customer.title,
                    firstName = _customer.firstName,
                    lastName = _customer.lastName,
                    middleInitial = _customer.middleInitial,
                    priAddressLine1 = _customer.priAddressLine1,
                    priAddressLine2 = _customer.priAddressLine2,
                    priCountry = _customer.priCountry,
                    priCompany = _customer.priCompany,
                    priState = _customer.priState,
                    priCity = _customer.priCity,
                    priZipCode = _customer.priZipCode,
                    priZipCodePlusFour = _customer.priZipCodePlusFour,
                    priCellphoneSegment1 = _customer.priCellphoneSegment1,
                    priCellphoneSegment2 = _customer.priCellphoneSegment2,
                    priCellphoneSegment3 = _customer.priCellphoneSegment3,
                    priTelephoneExtension = _customer.priTelephoneExtension,
                    entryDate = Convert.ToDateTime(_customer.entryDate)

                };
                model.customerDetail = _detail;
            }



            model.itemDetail = item;

            return model;
        }



        public ActionResult LetterTemplate()
        {
            return View();
        }

        
    }
   
}
