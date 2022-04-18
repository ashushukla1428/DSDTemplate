using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;
namespace Wempe.Controllers
{
    [CustomAuthorize()]

    public class RepairController : Controller
    {
        //
        // GET: /Repair/
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult NewRepair(Int64? id)
        {
            try
            {
                SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "--Select---" };
            //
            ViewBag.NameTitles = new SelectList(db.wmpTitles.Where(c => c.IsActive == true).Select(c => new { c.ID, c.title }), "Id", "title");
            //ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

            ViewBag.ContactPrefrences = new SelectList(db.wmpContactPreferences.Where(c => c.IsActive == true).OrderBy(c => c.contactPreference).Select(c => new { c.Id, c.contactPreference }), "Id", "contactPreference");
            ViewBag.CustomerType = new SelectList(db.wmpCustomerTypes.Where(c => c.IsActive == true).OrderBy(c => c.customerType).Select(c => new { c.Id, c.customerType }), "Id", "customerType");

            //All Item tab-2

            var temp = db.wmpEmployees.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.UserID == SessionMaster.Current.LoginId);
            if (temp.Any())
            {
                ViewBag.Employee = new SelectList(db.wmpEmployees.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).OrderBy(c => c.employee).Select(c => new { c.Id, c.employee }), "Id", "employee", temp.FirstOrDefault().Id);
            }
            else
            {
                ViewBag.Employee = new SelectList(db.wmpEmployees.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).OrderBy(c => c.employee).Select(c => new { c.Id, c.employee }), "Id", "employee");
            }

            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            ViewBag.CrystalCondition = new SelectList(db.wmpCrystalConditions.Where(c => c.IsActive == true).OrderBy(c => c.condition).Select(c => new { c.Id, c.condition }), "Id", "condition");
            //items & Bezel
            ViewBag.Items = new SelectList(db.wmpItems.Where(c => c.IsActive == true).OrderBy(c => c.item).Select(c => new { c.Id, c.item }), "Id", "item");
            ViewBag.BezelCondition = new SelectList(db.wmpBezelConditionMasters.Where(c => c.IsActive == true).OrderBy(c => c.BezelCondition).Select(c => new { c.BezelConditionID, c.BezelCondition }), "BezelConditionID", "BezelCondition");
            //Style & BackCondition
            ViewBag.Style = new SelectList(db.wmpStyles.Where(c => c.IsActive == true).OrderBy(c => c.style).Select(c => new { c.Id, c.style }), "Id", "style");
            ViewBag.BackCondition = new SelectList(db.wmpBackConditionMasters.Where(c => c.IsActive == true).OrderBy(c => c.BackCondition).Select(c => new { c.BackConditionID, c.BackCondition }), "BackConditionID", "BackCondition");
            //Movement & LUGS CONDITION
            ViewBag.Movement = new SelectList(db.wmpMovementTypes.Where(c => c.IsActive == true).OrderBy(c => c.movementType).Select(c => new { c.Id, c.movementType }), "Id", "movementType");
            ViewBag.LugsCondition = new SelectList(db.wmpLugsConditions.Where(c => c.IsActive == true).OrderBy(c => c.condition).Select(c => new { c.Id, c.condition }), "Id", "condition");
            //Case & Case Condition
            ViewBag.Case = new SelectList(db.wmpCaseTypes.Where(c => c.IsActive == true).OrderBy(c => c.caseType).Select(c => new { c.Id, c.caseType }), "Id", "caseType");
            ViewBag.CaseCondition = new SelectList(db.wmpCaseConditions.Where(c => c.IsActive == true).OrderBy(c => c.condition).Select(c => new { c.Id, c.condition }), "Id", "condition");
            //Band & BAND CONDITION
            ViewBag.Band = new SelectList(db.wmpBandTypeMasters.Where(c => c.IsActive == true).OrderBy(c => c.BandType).Select(c => new { c.BandTypeID, c.BandType }), "BandTypeID", "BandType");
            ViewBag.BandCondition = new SelectList(db.wmpBandConditions.Where(c => c.IsActive == true).OrderBy(c => c.condition).Select(c => new { c.Id, c.condition }), "Id", "condition");
            //Buckle & Buckle Condition
            ViewBag.Buckle = new SelectList(db.wmpBuckleTypes.Where(c => c.IsActive == true).OrderBy(c => c.buckleType).Select(c => new { c.Id, c.buckleType }), "Id", "buckleType");
            ViewBag.BuckleCondition = new SelectList(db.wmpBuckleConditionMasters.Where(c => c.IsActive == true).OrderBy(c => c.BuckleCondition).Select(c => new { c.BuckleConditionID, c.BuckleCondition }), "BuckleConditionID", "BuckleCondition");
            //DIAL & DIAL Condition
            ViewBag.Dial = new SelectList(db.wmpDialTypes.Where(c => c.IsActive == true).OrderBy(c => c.dialType).Select(c => new { c.Id, c.dialType }), "Id", "dialType");
            ViewBag.DialCondition = new SelectList(db.wmpDialConditions.Where(c => c.IsActive == true).OrderBy(c => c.condition).Select(c => new { c.Id, c.condition }), "Id", "condition");


            //Location
            ViewBag.Location = new SelectList(db.wmpSuppliers.Where(c => c.IsActive == true).OrderBy(c => c.name).Select(c => new { c.Id, c.name }), "Id", "name");
            //Repair Type
            ViewBag.RepairType = new SelectList(db.wmpRepairTypes.Where(c => c.IsActive == true).OrderBy(c => c.repairType).Select(c => new { c.Id, c.repairType }), "Id", "repairType");
            // status
            ViewBag.Status = new SelectList(db.wmpStatus.Where(c => c.IsActive == true).OrderBy(c => c.status).Select(c => new { c.Id, c.status, Attr = c.AttrName }), "Id", "status");
            // Purchase Location
            ViewBag.PurchaseLocation = new SelectList(db.wmpPurchaseLocations.Where(c => c.IsActive == true).OrderBy(c => c.name).Select(c => new { c.Id, c.name }), "Id", "name");
            // warranty doc
            ViewBag.WarrantyDoc = new SelectList(db.wmpWarrantyDocuments.Where(c => c.IsActive == true).OrderBy(c => c.WarrantyDocument).Select(c => new { c.Id, c.WarrantyDocument }), "Id", "WarrantyDocument");
            // box included
            ViewBag.BoxIncluded = new SelectList(db.wmpBoxIncludeds.Where(c => c.IsActive == true).OrderBy(c => c.boxIncluded).Select(c => new { c.Id, c.boxIncluded }), "Id", "boxIncluded");
            // Id Security
            ViewBag.Security = new SelectList(db.wmpIDSecurities.Where(c => c.IsActive == true).OrderBy(c => c.idSecurity).Select(c => new { c.Id, c.idSecurity }), "Id", "idSecurity");
            //Appraisal
            ViewBag.Appraisal = new SelectList(db.wmpCustomers.Where(c => c.IsActive == true).OrderBy(c => c.firstName).Select(c => new { c.customerNumber, Name = c.firstName + " " + c.lastName }), "customerNumber", "Name");
            // Appraisal Title
            ViewBag.AppraisalTitle = new SelectList(db.wmpAppraiserTitles.Where(c => c.IsActive == true).OrderBy(c => c.appraiserTitle).Select(c => new { c.Id, c.appraiserTitle }), "Id", "appraiserTitle");
            // Engraving Font
            ViewBag.EngravingFont = new SelectList(db.wmpEngravingFonts.Where(c => c.IsActive == true).OrderBy(c => c.font).Select(c => new { c.Id, c.font }), "Id", "font");
            //Engraving Capitalization
            ViewBag.EngravingCapitalization = new SelectList(db.wmpEngravingCapitalizations.Where(c => c.IsActive == true).OrderBy(c => c.capitalization).Select(c => new { c.Id, c.capitalization }), "Id", "capitalization");

            ViewBag.ShippingCarrier = new SelectList(db.wmpCarriers.Where(c => c.IsActive == true).OrderBy(c => c.carrier).Select(c => new { c.Id, c.carrier }), "Id", "carrier");
            ViewBag.ShippingType = new SelectList(db.wmpShippingTypes.Where(c => c.IsActive == true).OrderBy(c => c.shippingType).Select(c => new { c.Id, c.shippingType }), "Id", "shippingType");


            // last Section
            ViewBag.MaterialType = new SelectList(db.wmpstrapMaterialTypes.Where(c => c.IsActive == true).OrderBy(c => c.strapMaterialType).Select(c => new { c.MaterialTypeID, c.strapMaterialType }), "MaterialTypeID", "strapMaterialType");
            ViewBag.TextureType = new SelectList(db.wmpStrapTextureTypes.Where(c => c.IsActive == true).OrderBy(c => c.strapTextureType).Select(c => new { c.Id, c.strapTextureType }), "Id", "strapTextureType");
            ViewBag.Color = new SelectList(db.wmpStrapColors.Where(c => c.IsActive == true).OrderBy(c => c.strapColor).Select(c => new { c.Id, c.strapColor }), "Id", "strapColor");
            ViewBag.LusterType = new SelectList(db.wmpStrapLusterTypes.Where(c => c.IsActive == true).OrderBy(c => c.strapLusterType).Select(c => new { c.Id, c.strapLusterType }), "Id", "strapLusterType");
            ViewBag.StitchType = new SelectList(db.wmpStrapStitchTypes.Where(c => c.IsActive == true).OrderBy(c => c.strapStitchType).Select(c => new { c.Id, c.strapStitchType }), "Id", "strapStitchType");
            ViewBag.ThicknessType = new SelectList(db.wmpStrapThicknessTypes.Where(c => c.IsActive == true).OrderBy(c => c.strapThicknessType).Select(c => new { c.Id, c.strapThicknessType }), "Id", "strapThicknessType");
            ViewBag.Accessories = new SelectList(db.wmpStrapAccessories.Where(c => c.IsActive == true).OrderBy(c => c.strapAccessories).Select(c => new { c.Id, c.strapAccessories }), "Id", "strapAccessories");

            ViewBag.SampleTask = new SelectList(db.wmpRepairSampleTasks.Where(c => c.Type == 1 && c.IsActive == true && c.description != null).OrderBy(c => c.description).Select(c => new { c.Id, c.description }), "Id", "description");

                //            ViewBag.SampleTaskOptional = new SelectList(db.wmpRepairSampleTasks.Where(c => c.Type == 2 && c.IsActive == true && c.description != null).OrderBy(c => c.description).Select(c => new { c.Id, c.description }), "Id", "description");
                TempData["SampleTaskOptional"] = new SelectList(db.wmpRepairSampleTasks.Where(c => c.Type == 2 && c.IsActive == true && c.description != null).OrderBy(c => c.description).Select(c => new { c.Id, c.description }), "Id", "description");
                //var obj = db.wmpRepairSampleTasks.Where(c => c.IsActive == true && c.description != null).OrderBy(c => c.description).ToList<SampleTask>();

                // ViewBag.SampleTask2 =
            }
            catch (Exception ex)
            {
                string errroMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errroMessage += ex.InnerException.Message;
                }
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                errroMessage += message;
                ViewData["Error"] = errroMessage;
                //string path = Server.MapPath("~/ErrorLog/ErrorLog.txt");
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    writer.WriteLine(message);
                //    writer.Close();
                //}
            }

            return View();
        }


        public ActionResult Search()
        {
            SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "--Select---" };
            //

            ViewBag.NameTitles = new SelectList(db.wmpTitles.Where(c => c.IsActive == true).Select(c => new { c.ID, c.title }), "Id", "title");
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

            //SelectListItem selListItem = new SelectListItem() { Value = "0", Text = "--Select---" };
            //
            ViewBag.NameTitles = new SelectList(db.wmpTitles.Where(c => c.IsActive == true).Select(c => new { c.ID, c.title }), "Id", "title");
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);

            ViewBag.ContactPrefrences = new SelectList(db.wmpContactPreferences.Where(c => c.IsActive == true).OrderBy(c => c.contactPreference).Select(c => new { c.Id, c.contactPreference }), "Id", "contactPreference");
            ViewBag.CustomerType = new SelectList(db.wmpCustomerTypes.Where(c => c.IsActive == true).OrderBy(c => c.customerType).Select(c => new { c.Id, c.customerType }), "Id", "customerType");

            //All Item tab-2
            ViewBag.Employee = new SelectList(db.wmpEmployees.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).OrderBy(c => c.employee).Select(c => new { c.Id, c.employee }), "Id", "employee");
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            ViewBag.Items = new SelectList(db.wmpItems.Where(c => c.IsActive == true).OrderBy(c => c.item).Select(c => new { c.Id, c.item }), "Id", "item");

            ViewBag.Movement = new SelectList(db.wmpMovementTypes.Where(c => c.IsActive == true).OrderBy(c => c.movementType).Select(c => new { c.Id, c.movementType }), "Id", "movementType");


            //Location
            ViewBag.Location = new SelectList(db.wmpSuppliers.Where(c => c.IsActive == true).OrderBy(c => c.name).Select(c => new { c.Id, c.name }), "Id", "name");
            //Repair Type
            ViewBag.RepairType = new SelectList(db.wmpRepairTypes.Where(c => c.IsActive == true).OrderBy(c => c.repairType).Select(c => new { c.Id, c.repairType }), "Id", "repairType");
            // status
            ViewBag.Status = new SelectList(db.wmpStatus.Where(c => c.IsActive == true).OrderBy(c => c.status).Select(c => new { c.Id, c.status }), "Id", "status");
            // Purchase Location
            ViewBag.PurchaseLocation = new SelectList(db.wmpPurchaseLocations.Where(c => c.IsActive == true).OrderBy(c => c.name).Select(c => new { c.Id, c.name }), "Id", "name");

            return View();
        }


        public JsonResult searchRepair(SearchRepairFilters model)
        {
            try
            {
                model.sortColumn = "LastUpdate";
                model.sortOrder = "desc";
                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpRepairforSearch>("USP_SearchRepairOnAllFields @p0, @p1, @p2, @p3, @p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13", model.SearchFields, model.SearchValues, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID, model.CustomerNumber, model.dueDateCustomerStart, model.dueDateCustomerEnd, model.dueDateSupplierStart, model.dueDateSupplierEnd, model.EntryDateStart, model.EntryDateEnd);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        //get Customer list for repairs list 
        public JsonResult searchCustomer(SearchCustomerFilters model)
        {
            try
            {
                model.sortColumn = "firstName";
                model.sortOrder = "desc";
                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomerOnAllFields @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.SearchFields, model.SearchValues, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        public JsonResult searchExitingCustomer(SearchFilters model)
        {
            try
            {
               // model.sortColumn = "firstName";
               // model.sortOrder = "desc";


                //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchExistingCustomerNew @p0, @p1, @p2, @p3, @p4,@p5", model.Name, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        //old function 
        //public JsonResult searchExitingCustomer(SearchFilter model)
        //{
        //    try
        //    {
        //        model.sortColumn = "firstName";
        //        model.sortOrder = "desc";
        //        //var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchCustomer @p0, @p1, @p2, @p3, @p4,@p5,@p6", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
        //        var _items = db.Database.SqlQuery<wmpCustomerforSearch>("USP_SearchExistingCustomerNew @p0, @p1, @p2, @p3, @p4,@p5", model.ColName, model.ColValue, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
        //        return Json(_items, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new Result { Status = false, Message = ex.Message });
        //    }
        //}
        [HttpPost]
        public JsonResult AddCustomer(wmpCustomer model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.IsActive = true;
                if (model.customerNumber == 0)
                {
                    model.OwnerID = SessionMaster.Current.OwnerID;
                    model.entryDate = DateTime.Now;
                    db.wmpCustomers.Add(model);
                    wmpRepairLog log = new wmpRepairLog();
                    //log.repairNumber = Convert.ToInt32(model.repairNumber);
                    log.dateStamp = DateTime.Now;
                    log.OwnerID = SessionMaster.Current.OwnerID;
                    log.UpdateBy = SessionMaster.Current.LoginId;
                    log.changeComment = "Customer profile created.";
                    db.wmpRepairLogs.Add(log);
                    db.SaveChanges();
                }
                else
                {
                    var _model = db.wmpCustomers.Where(c => c.customerNumber == model.customerNumber).FirstOrDefault();
                    db.Dispose();
                    db = new dbWempeEntities();
                    model.entryDate = _model.entryDate;
                    model.OwnerID = _model.OwnerID;
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
                var x = db.wmpLastRecordSets.Where(s => s.UserId == SessionMaster.Current.LoginId && s.OwnderId == SessionMaster.Current.OwnerID).FirstOrDefault();
                if (x != null)
                {
                    x.CustomerNumber = model.customerNumber;
                    db.SaveChanges();
                }
                else
                {
                    wmpLastRecordSet lastRecord = new wmpLastRecordSet();
                    lastRecord.UserId = SessionMaster.Current.LoginId;
                    lastRecord.OwnderId = SessionMaster.Current.OwnerID;
                    lastRecord.CustomerNumber = model.customerNumber;
                    db.wmpLastRecordSets.Add(lastRecord);
                    db.SaveChanges();
                }
                return Json(new Result { Status = true, Message = model.customerNumber.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.InnerException.InnerException.Message });
            }
        }

        [HttpPost]
        public JsonResult AddItem(wmpRepair model)
        {
            string OldStatus = "";
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.IsActive = true;
                if (model.repairNumber == 0)
                {
                    model.OwnerID = SessionMaster.Current.OwnerID;
                    model.entryDate = DateTime.Now;
                    db.wmpRepairs.Add(model);
                    db.SaveChanges();
                    wmpRepairLog log = new wmpRepairLog();
                    log.repairNumber = Convert.ToInt32(model.repairNumber);
                    log.dateStamp = DateTime.Now;
                    log.OwnerID = SessionMaster.Current.OwnerID;
                    log.UpdateBy = SessionMaster.Current.LoginId;
                    log.changeComment = "Repair profile created.";
                    db.wmpRepairLogs.Add(log);
                    db.SaveChanges();
                }
                else
                {
                    var _model = db.wmpRepairs.Where(c => c.repairNumber == model.repairNumber && c.customerNumber == model.customerNumber).FirstOrDefault();
                    // set old values
                    model.dateAuthToFactoryCharge = _model.dateAuthToFactoryCharge;
                    model.dateAuthToFactoryNoCharge = _model.dateAuthToFactoryNoCharge;
                    model.dateEstimateRequest = _model.dateEstimateRequest;
                    model.dateEstimateRequestStatus = _model.dateEstimateRequestStatus;
                    model.datePickedUp = _model.datePickedUp;
                    model.datePickupNotification = _model.datePickupNotification;
                    model.dateProceedToFactory = _model.dateProceedToFactory;
                    model.dateProceedWithService = _model.dateProceedWithService;
                    model.dateReceivedByMail = _model.dateReceivedByMail;
                    model.dateReceivedByMessenger = _model.dateReceivedByMessenger;
                    model.dateReminderToFactory = _model.dateReminderToFactory;
                    model.dateRepairDelay = _model.dateRepairDelay;
                    model.dateRepairOffer = _model.dateRepairOffer;
                    model.dateRepairOfferReminder = _model.dateRepairOfferReminder;
                    model.dateRepairStatus = _model.dateRepairStatus;
                    model.dateReturnUndone = _model.dateReturnUndone;
                    model.dateServiceRequest = _model.dateServiceRequest;
                    model.dateServiceRequestStatus = _model.dateServiceRequestStatus;
                    model.dateShipped = _model.dateShipped;
                    // model.dueDateStartDate = _model.dueDateStartDate;
                    //get status Attribute and update it in current model
                    var x = db.wmpStatus.Where(c => c.Id == model.statusId).FirstOrDefault();
                    if (x != null)
                    {
                        if (x.AttrName != null)
                        {
                            db.Entry(model).Property(x.AttrName).CurrentValue = DateTime.Now;
                        }
                    }
                    // end
                    // insert Log
                    if (model.statusId != _model.statusId)
                    {
                        wmpRepairLog log = new wmpRepairLog();
                        log.repairNumber = Convert.ToInt32(model.repairNumber);
                        log.dateStamp = DateTime.Now;
                        log.OwnerID = SessionMaster.Current.OwnerID;
                        log.UpdateBy = SessionMaster.Current.LoginId;
                        log.changeComment = "Status set to " + x.status + ".";
                        db.wmpRepairLogs.Add(log);
                        db.SaveChanges();
                    }
                    if (model.dueDateTime != _model.dueDateTime)
                    {
                        wmpRepairLog log = new wmpRepairLog();
                        if (model.dueDateTime == "")
                        {
                            log.changeComment = "Due date (customer) reset.  No due date entered.";
                        }
                        else if (model.dueDateTime == "1")
                        {
                            log.changeComment = "Due date (customer) set to " + model.dueDate + " (" + model.dueDateTime + " " + model.dueDateType + ").";
                        }
                        else
                        {
                            log.changeComment = "Due date (customer) set to " + model.dueDate + " (" + model.dueDateTime + " " + model.dueDateType + "s).";
                        }

                        log.repairNumber = Convert.ToInt32(model.repairNumber);
                        log.dateStamp = log.LastUpdate = DateTime.Now;
                        log.UpdateBy = SessionMaster.Current.LoginId;
                        log.OwnerID = SessionMaster.Current.OwnerID;
                        db.wmpRepairLogs.Add(log);
                        db.SaveChanges();
                    }
                    if (model.SupplierdueDateTime != _model.SupplierdueDateTime)
                    {
                        wmpRepairLog log = new wmpRepairLog();
                        if (model.SupplierdueDateTime == "")
                        {
                            log.changeComment = "Due date (factory) reset.  No due date (factory) entered.";
                        }
                        else if (model.SupplierdueDateTime == "1")
                        {
                            log.changeComment = "Due date (factory) set to " + model.SupplierDueDate + " (" + model.SupplierdueDateTime + " " + model.dueDateType + ").";
                        }
                        else
                        {
                            log.changeComment = "Due date (factory) set to " + model.SupplierDueDate + " (" + model.SupplierdueDateTime + " " + model.dueDateFactoryType + "s).";
                        }

                        log.repairNumber = Convert.ToInt32(model.repairNumber);
                        log.dateStamp = log.LastUpdate = DateTime.Now;
                        log.UpdateBy = SessionMaster.Current.LoginId;
                        log.OwnerID = SessionMaster.Current.OwnerID;
                        db.wmpRepairLogs.Add(log);
                        db.SaveChanges();
                    }
                    //end log


                    db.Dispose();
                    db = new dbWempeEntities();
                    model.entryDate = _model.entryDate;
                    model.OwnerID = _model.OwnerID;
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
                var ItemRecordSet = db.wmpLastRecordSets.Where(s => s.UserId == SessionMaster.Current.LoginId && s.OwnderId == SessionMaster.Current.OwnerID).FirstOrDefault();
                if (ItemRecordSet != null)
                {
                    ItemRecordSet.RepairNumber = model.repairNumber;
                    db.SaveChanges();
                }
                else
                {
                    wmpLastRecordSet lastRecord = new wmpLastRecordSet();
                    lastRecord.UserId = SessionMaster.Current.LoginId;
                    lastRecord.OwnderId = SessionMaster.Current.OwnerID;
                    lastRecord.CustomerNumber = model.customerNumber;
                    lastRecord.RepairNumber = model.repairNumber;
                    db.wmpLastRecordSets.Add(lastRecord);
                    db.SaveChanges();
                }
                return Json(new Result { Status = true, Message = model.repairNumber.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        //get states
        [HttpGet]
        public JsonResult getStates(int Id)
        {
            try
            {
                //return Json(db.wmpStates.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.CountryId==Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }),JsonRequestBehavior.AllowGet);
                return Json(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        //getCities
        [HttpGet]
        public JsonResult getCities(int Id)
        {
            try
            {
                // return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
                return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.StateId == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }


        [HttpGet]
        public JsonResult getCustomer(Int64 id)
        {
            try
            {
                //  return Json(db.wmpCustomers.Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.customerNumber == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
                return Json(db.wmpCustomers.Where(c => c.customerNumber == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetTaxByStateId(Int64 id)
        {
            try
            {
                return Json(db.wmpTaxRates.Where(c => c.StateId == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        //[HttpGet]
        public string CheckDuplicateOrderNumber(string OrderId, long RepairNumber)
        {
            try
            {
                var x = db.wmpRepairs.Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.OrderNumber == OrderId && c.repairNumber != RepairNumber).FirstOrDefault();
                if (x != null)
                {
                    return "Duplicate";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public JsonResult getRepair(Int64 id)
        {
            try
            {
                //return Json(db.wmpRepairs.Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.repairNumber == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
                return Json(db.wmpRepairs.Where(c => c.repairNumber == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        // Sagar: purchase location popup
        [HttpGet]
        public JsonResult getDatabyPurchaseLocationId(Int64 id)
        {
            try
            {
                var x = db.wmpPurchaseLocations.Where(c => c.Id == id).FirstOrDefault();
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult getDatabyLocationId(Int64 id)
        {
            try
            {
                var x = db.wmpSuppliers.Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.Id == id).FirstOrDefault();
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }


        // tab 5-- get uploaded files
        //[HttpGet]
        public JsonResult getUploadedFilesList(FileAttachmentList model)
        {
            try
            {
                model.sortColumn = "dateStamp";
                model.sortOrder = "desc";
                //var _items = db.wmpFileAttachments.Where(c => c.OwnerID == SessionMaster.Current.OwnerID && c.repairNumber == model.repairNumber).FirstOrDefault();
                var _items = db.Database.SqlQuery<FileAttachmentList>("USP_GetAttachmentFileByRepairNumber @p0, @p1, @p2, @p3, @p4,@p5", model.repairNumber == null ? 0 : model.repairNumber, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(long AttachmentId)
        {
            try
            {
                var data = db.wmpFileAttachments.Find(AttachmentId);
                FileInfo info1 = new FileInfo(Server.MapPath("/UploadFiles/") + data.fullPath);
                if (info1.Exists)
                {
                    info1.Delete();
                }
                db.wmpFileAttachments.Remove(data);
                db.SaveChanges();

                wmpRepairLog log = new wmpRepairLog();
                log.changeComment = "Deleted file (" + data.fileName + ") to repair.";
                log.repairNumber = Convert.ToInt32(data.repairNumber);
                log.dateStamp = log.LastUpdate = DateTime.Now;
                log.OwnerID = SessionMaster.Current.OwnerID;
                log.UpdateBy = SessionMaster.Current.LoginId;
                //log.UpdateBy = SessionMaster.Current.LoginId;
                db.wmpRepairLogs.Add(log);
                db.SaveChanges();
                return Json(new Result { Status = true, Message = Messages.recordDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // [HttpGet]
        public string getAttrByStatusId(Int64 StatusId)
        {
            try
            {
                var x = db.wmpStatus.Where(c => c.Id == StatusId).Select(c => new { Attr = c.AttrName });
                return x.FirstOrDefault().Attr;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public JsonResult GetLastCustomerAndRepairNumber()
        {
            try
            {
                var x = db.wmpLastRecordSets.Where(c => c.OwnderId == SessionMaster.Current.OwnerID && c.UserId == SessionMaster.Current.LoginId).SingleOrDefault();
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ResetLastCustomerAndRepairNumber()
        {
            try
            {
                var x = db.wmpLastRecordSets.Where(c => c.OwnderId == SessionMaster.Current.OwnerID && c.UserId == SessionMaster.Current.LoginId).SingleOrDefault();
                x.CustomerNumber = 0;
                x.RepairNumber = 0;
                db.SaveChanges();
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmails(Int64 RepairNumber, string DocumentName)
        {
            wmpRepair ForcustomerId = db.wmpRepairs.Where(s => s.repairNumber == RepairNumber).FirstOrDefault();
            CustomerViewModel data =(CustomerViewModel) db.wmpCustomers.Where(c => c.customerNumber == ForcustomerId.customerNumber).FirstOrDefault();
            wmpDocument doc = db.wmpDocuments.Where(c => c.shortName == DocumentName).FirstOrDefault();
            //data
            data.DocumentType = doc.type;
            data.DocumentFullName = doc.fullName;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult sendEmail(Int64 RepairNumber, string HTML, string[] Emails, string DocumentName)
        {
            try
            {
                // Get Document Type Info
                wmpDocument doc = db.wmpDocuments.Where(c => c.shortName == DocumentName).FirstOrDefault();

                //if (HTML.Contains("/images/"))
                //{
                //    HTML = HTML.Replace("/images/", Server.MapPath("/images/"));
                //    HTML = HTML.Replace("590", "100%");
                //}
                // get Repair Data
                wmpRepair ForcustomerId = db.wmpRepairs.Where(s => s.repairNumber == RepairNumber).FirstOrDefault();
                // get customer data
                var x = from data in db.wmpCustomers.Where(c => c.customerNumber == ForcustomerId.customerNumber)
                        select new { data.priEMailAddress, data.secEMailAddress, data.thirdEMailAddress, data.firstName, data.lastName };
                // generate pdf
                var _fileData = GetPDF(HTML);
                string FileName = DateTime.Now.Ticks.ToString();
                FileStream fs = new FileStream(Server.MapPath("~/Upload/eMailArchive/Wempe_" + FileName + "(" + RepairNumber + "-" + ForcustomerId.ticketNumber + ")" + ".pdf"), FileMode.OpenOrCreate);
                fs.Write(_fileData, 0, _fileData.Length);
                fs.Close();

                LinkMode vm = new LinkMode()
                {
                    //Link = WebConfigurationManager.AppSettings["Websitelink"] + "Home/ResetPassword/" + _guidCode,
                    FirstName = x.FirstOrDefault().firstName,
                    LastName = x.FirstOrDefault().lastName
                };
                string html = "";
                if (doc.type == "Customer")
                {
                    string _bName = db.wmpBrandMasters.Where(s => s.BrandID == ForcustomerId.brandId).FirstOrDefault().BrandName;
                    // in case of customer type document
                    html = RazorViewToString.RenderRazorViewToString(this, "~/Views/emailTemplate/SendAttachment.cshtml", vm);
                    if (_bName != null)
                    {
                        html = html.Replace("{BrandName}", _bName);
                    }
                    else
                    {
                        html = html.Replace("{BrandName}", "");
                    }
                    html = html.Replace("{Item}", db.wmpItems.Where(s => s.Id == ForcustomerId.itemId).FirstOrDefault().item);
                    html = html.Replace("{DocumentFullName}", doc.fullName);
                }
                else
                {
                    // need to do after verify
                }

                var MailHelper = new MailHelper
                {
                    Sender = ConfigurationManager.AppSettings["EmailFromAddress"], //email.Sender,
                    RecipientCC = null,
                    Subject = "Message from Wempe Jewelers (Repair # " + RepairNumber + "-" + ForcustomerId.ticketNumber + ")",
                    Body = html
                };
                MailHelper.AttachmentFile = Server.MapPath("~/Upload/eMailArchive/Wempe_" + FileName + "(" + RepairNumber + "-" + ForcustomerId.ticketNumber + ")" + ".pdf");

                string TempEmailList = "";
                foreach (var item in Emails)
                {
                    MailHelper.Recipient = item;
                    TempEmailList += item + ", ";
                }
                TempEmailList = TempEmailList.Remove(TempEmailList.Length - 2);
                // MailHelper.Recipient = "sagark@impingeonline.com";
                MailHelper.Send();

                // Entry in log table
                wmpRepairLog log = new wmpRepairLog();
                log.repairNumber = Convert.ToInt32(RepairNumber);
                log.dateStamp = DateTime.Now;
                log.OwnerID = SessionMaster.Current.OwnerID;
                log.UpdateBy = SessionMaster.Current.LoginId;
                log.changeComment = "Document (" + doc.fullName + ") sent via e-mail to " + doc.type + " at " + TempEmailList + ".";
                db.wmpRepairLogs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                wmpRepairLog log = new wmpRepairLog();
                log.repairNumber = Convert.ToInt32(RepairNumber);
                log.dateStamp = DateTime.Now;
                log.OwnerID = SessionMaster.Current.OwnerID;
                log.UpdateBy = SessionMaster.Current.LoginId;
                log.changeComment = ex.Message;
                db.wmpRepairLogs.Add(log);
                db.SaveChanges();
                //return false;
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;
            MemoryStream ms = new MemoryStream();
            string TempHTML = Regex.Replace(pHTML, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            pHTML = Regex.Replace(pHTML, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // TempHTML=TempHTML.Replace()
            TextReader txtReader = new StringReader(pHTML);
            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);
            //   var userpass = Encoding.ASCII.GetBytes("userpass");
            //   var ownerpass = Encoding.ASCII.GetBytes("ownerpass");
            //   oPdfWriter.SetEncryption(userpass, ownerpass, 255, true);

            // XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, txtReader);
            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);
            // 4: we open document and start the worker on the document
            doc.Open();
            htmlWorker.StartDocument();
            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);
            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();
            bPDF = ms.ToArray();
            return bPDF;
        }
        [HttpPost]
        public JsonResult UploadFiles()
        {

            //dbWempeEntities _db = new dbWempeEntities();
            ////context.Response.ContentType = "text/plain";
            //HttpFileCollectionBase files = Request.Files;
            ////int repairNumber = Convert.ToInt32(context.Request["repairNumber"]);
            ////int repairNumber = Convert.ToInt32(Request.Files["repairNumber"]);


            //var TempFetchRecord = _db.wmpRepairs.Where(x => x.repairNumber == repairNumber);
            //string FileNameFull = "C" + TempFetchRecord.FirstOrDefault().customerNumber + "_R" + TempFetchRecord.FirstOrDefault().repairNumber + "_" + DateTime.Now.Ticks + "_" + uploadFiles.FileName;
            //string FilePathToStore = +TempFetchRecord.FirstOrDefault().OwnerID + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
            //bool exists = System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore));
            //if (!exists)
            //    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore));

            //string pathToSave = HttpContext.Current.Server.MapPath("~/UploadFiles/" + FilePathToStore + "/") + FileNameFull;
            //uploadFiles.SaveAs(pathToSave);

            //wmpFileAttachment obj = new wmpFileAttachment
            //{
            //    repairNumber = repairNumber,
            //    dateStamp = DateTime.Now.ToString(),
            //    fileName = FileNameFull,
            //    fullPath = FilePathToStore + "/" + FileNameFull,
            //    OwnerID = SessionMaster.Current.OwnerID,
            //    UpdateBy = SessionMaster.Current.LoginId
            //};
            //_db.wmpFileAttachments.Add(obj);
            //_db.SaveChanges();
            //wmpRepairLog log = new wmpRepairLog();
            //log.changeComment = "Attached file (" + obj.fileName + ") to repair.";
            //log.repairNumber = Convert.ToInt32(repairNumber);
            //log.dateStamp = log.LastUpdate = DateTime.Now;

            //log.OwnerID = SessionMaster.Current.OwnerID;
            //log.UpdateBy = SessionMaster.Current.LoginId;

            //_db.wmpRepairLogs.Add(log);
            //_db.SaveChanges();

            // Checking no of files injected in Request object  
            //dbWempeEntities _db = new dbWempeEntities();

            wmpRepairLog log = new wmpRepairLog();
            log.changeComment = "Attached file  to repair.";
            log.repairNumber = 0;
            log.dateStamp = log.LastUpdate = DateTime.Now;

            log.OwnerID = SessionMaster.Current.OwnerID;
            log.UpdateBy = SessionMaster.Current.LoginId;

            db.wmpRepairLogs.Add(log);
            db.SaveChanges();


            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        // fname = Path.Combine(Server.MapPath("~/UploadFiles/"), fname);
                       
                        int repairNumber = Convert.ToInt32(Request.Files["repairNumber"]);
                        var TempFetchRecord = db.wmpRepairs.Where(x => x.repairNumber == repairNumber);
                        string FileNameFull = "C" + TempFetchRecord.FirstOrDefault().customerNumber + "_R" + TempFetchRecord.FirstOrDefault().repairNumber + "_" + DateTime.Now.Ticks + "_" + file.FileName;
                        string FilePathToStore = +TempFetchRecord.FirstOrDefault().OwnerID + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                        bool exists = System.IO.Directory.Exists(Server.MapPath("~/UploadFiles/" + FilePathToStore));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/UploadFiles/" + FilePathToStore));

                        string pathToSave = Server.MapPath("~/UploadFiles/" + FilePathToStore + "/") + FileNameFull;

                        file.SaveAs(pathToSave);

                        wmpFileAttachment obj = new wmpFileAttachment
                        {
                            repairNumber = repairNumber,
                            dateStamp = DateTime.Now.ToString(),
                            fileName = FileNameFull,
                            fullPath = FilePathToStore + "/" + FileNameFull,
                            OwnerID = SessionMaster.Current.OwnerID,
                            UpdateBy = SessionMaster.Current.LoginId
                        };
                        db.wmpFileAttachments.Add(obj);
                        db.SaveChanges();

                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        //  return Json("File Uploaded Successfully!");
        // new functinality 05 Dec 2016
        [HttpGet]
        public JsonResult SaveNewState(string CountryId, string StateName)
        {
            Int64 x = Convert.ToInt64(CountryId);
            wmpState state = db.wmpStates.Where(s => s.CountryId == x && s.stateFullName==StateName).FirstOrDefault();
            if (state != null)
            {
                // return Json("State name is already exists", JsonRequestBehavior.AllowGet);
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                wmpState obj = new wmpState { CountryId = Convert.ToInt64(CountryId), LastUpdate = DateTime.Now, OwnerID = SessionMaster.Current.OwnerID, stateFullName = StateName, UpdateBy = SessionMaster.Current.LoginId, IsActive=true };
                db.wmpStates.Add(obj);
                db.SaveChanges();
                //return Json("Saved successfully", JsonRequestBehavior.AllowGet);
                return Json("1", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult SaveNewCity(string StateId, string County, string CityName, string ZipCode)
        {
            Int64 tempCityId = 0;
            Int64 tempCountyId = 0;
            Int64 xStateId = Convert.ToInt64(StateId);
            wmpCounty obj = db.wmpCounties.Where(s => s.StateId == xStateId && s.County == County).FirstOrDefault();
            if (obj != null)
            {
                tempCountyId = obj.Id;
            }
            else
            {
                wmpCounty objCountySave = new wmpCounty { County = County, IsActive = 1, StateId = xStateId };
                db.wmpCounties.Add(objCountySave);
                db.SaveChanges();
                tempCountyId = objCountySave.Id;
            }
            wmpSampleCity city = db.wmpSampleCities.Where(s => s.StateId == xStateId && s.city == CityName && s.CountyID== tempCountyId).FirstOrDefault();
            if (city != null)
            {
                tempCityId = city.Id;
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                wmpZipCode objFetchZipCode = db.wmpZipCodes.Where(s => s.PostalCode == ZipCode).FirstOrDefault();
                if (objFetchZipCode != null)
                {
                    return Json("02", JsonRequestBehavior.AllowGet);
                }
                else
                {                
                    wmpSampleCity objCity = new wmpSampleCity { city = CityName, IsActive = true, LastUpdate = DateTime.Now, OwnerID = SessionMaster.Current.OwnerID, UpdateBy = SessionMaster.Current.LoginId, StateId = xStateId, CountyID = tempCountyId };
                    db.wmpSampleCities.Add(objCity);
                    db.SaveChanges();
                    wmpZipCode objZipcode = new wmpZipCode { PostalCode = ZipCode, IsActive = true, CityId = objCity.Id };
                    db.wmpZipCodes.Add(objZipcode);
                    db.SaveChanges();
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult GetDataByZipcode(string ZipCode)
        {
            wmpZipCode zipCode = db.wmpZipCodes.Where(s => s.PostalCode == ZipCode || s.PostalCode ==  ZipCode.Remove(0,1)).FirstOrDefault();
            if (zipCode == null)
            {
                // return Json("State name is already exists", JsonRequestBehavior.AllowGet);
                ZipcodeCountryStateCityModel objTemp = new ZipcodeCountryStateCityModel();
                return Json(objTemp, JsonRequestBehavior.AllowGet);
            }
            else
            {
                wmpSampleCity obj = db.wmpSampleCities.Where(s => s.Id == zipCode.CityId).FirstOrDefault();
                wmpState objState = db.wmpStates.Where(s => s.Id == obj.StateId).FirstOrDefault();
                ZipcodeCountryStateCityModel objTemp = new ZipcodeCountryStateCityModel { CityId = obj.Id.ToString(), StateId = obj.StateId.ToString(), CountryId = objState.CountryId.ToString() };
                return Json(objTemp, JsonRequestBehavior.AllowGet);
            }
        }
    }


    

    public class SearchFilter : SortingFields
    {
        public string ColName { get; set; }
        public string ColValue { get; set; }
    }
    public class wmpCustomerforSearch : PageingModel
    {
        public long customerNumber { get; set; }
        public string FullName { get; set; }
        public string PrimaryAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PrimaryPhone { get; set; }
        public string Email { get; set; }
    }
    public class wmpRepairforSearch : PageingModel
    {
        public long customerNumber { get; set; }
        public Int64 repairNumber { get; set; }
        public string repairNumberComplete { get; set; }
        public string EntryDate { get; set; }
        public string brandName { get; set; }
        public string supplierRepairNumber { get; set; }
        public string RepairStatus { get; set; }
        // public string State { get; set; }
        public string status { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
    }
    public class FileAttachmentList : PageingModel
    {
        public long fileNumber { get; set; }
        public Nullable<int> repairNumber { get; set; }
        public string dateStamp { get; set; }
        public string fileName { get; set; }
        public string fullPath { get; set; }
        public string Extension { get; set; }
        public string sortColumn { get; set; }
        public string sortOrder { get; set; }
        public int pageNo { get; set; }
    }
    public class searchModel
    {
        public long Id { get; set; }
        public long customerNumber { get; set; }
        public Nullable<System.DateTime> entryDate { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public string asstTitle { get; set; }
        public string asstFirstName { get; set; }
        public string asstMiddleInitial { get; set; }
        public string asstLastName { get; set; }
        public string priCompany { get; set; }
        public string priAddressLine1 { get; set; }
        public string priAddressLine2 { get; set; }
        public string priCity { get; set; }
        public string priState { get; set; }
        public string priZipCode { get; set; }
        public string priZipCodePlusFour { get; set; }
        public string priCountry { get; set; }
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
        public string priEMailAddress { get; set; }
        public string secCompany { get; set; }
        public string secAddressLine1 { get; set; }
        public string secAddressLine2 { get; set; }
        public string secCity { get; set; }
        public string secState { get; set; }
        public string secZipCode { get; set; }
        public string secZipCodePlusFour { get; set; }
        public string secCountry { get; set; }
        public string secTelephoneSegment1 { get; set; }
        public string secTelephoneSegment2 { get; set; }
        public string secTelephoneSegment3 { get; set; }
        public string secTelephoneExtension { get; set; }
        public string secCellphoneSegment1 { get; set; }
        public string secCellphoneSegment2 { get; set; }
        public string secCellphoneSegment3 { get; set; }
        public string secFaxSegment1 { get; set; }
        public string secFaxSegment2 { get; set; }
        public string secFaxSegment3 { get; set; }
        public string secEMailAddress { get; set; }
        public string priCreditCardSegment1 { get; set; }
        public string priCreditCardSegment2 { get; set; }
        public string priCreditCardSegment3 { get; set; }
        public string priCreditCardSegment4 { get; set; }
        public string priCreditCardExpMonth { get; set; }
        public string priCreditCardExpYear { get; set; }
        public string secCreditCardSegment1 { get; set; }
        public string secCreditCardSegment2 { get; set; }
        public string secCreditCardSegment3 { get; set; }
        public string secCreditCardSegment4 { get; set; }
        public string secCreditCardExpMonth { get; set; }
        public string secCreditCardExpYear { get; set; }
        public string contactPreference { get; set; }
        public string customerType { get; set; }
        public string notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<long> UpdateBy { get; set; }
        public Nullable<long> OwnerID { get; set; }
    }
}