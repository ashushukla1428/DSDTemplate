using System;
using System.Linq;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;
using System.Net;

namespace Wempe.Controllers
{
    public class TrackRepairStatusController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();
        public ActionResult TrackRepair()
        {
            return View();
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
        public JsonResult getRepair(Int64 id, Int64 LogId)
        {
            try
            {
                var x = db.wmpRepairStatusLogs.Where(s => s.LogId == LogId).FirstOrDefault();
                x.Type = "Viewed";
                db.SaveChanges();
                return Json(db.wmpRepairs.Where(c => c.repairNumber == id).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        public ActionResult RepairDetails()
        {
            ViewBag.NameTitles = new SelectList(db.wmpTitles.Where(c => c.IsActive == true).Select(c => new { c.ID, c.title }), "Id", "title");
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            ViewBag.ContactPrefrences = new SelectList(db.wmpContactPreferences.Where(c => c.IsActive == true).OrderBy(c => c.contactPreference).Select(c => new { c.Id, c.contactPreference }), "Id", "contactPreference");
            ViewBag.CustomerType = new SelectList(db.wmpCustomerTypes.Where(c => c.IsActive == true).OrderBy(c => c.customerType).Select(c => new { c.Id, c.customerType }), "Id", "customerType");
            ViewBag.Employee = new SelectList(db.wmpEmployees.Where(c => c.IsActive == true && c.OwnerID == SessionMaster.Current.OwnerID).OrderBy(c => c.employee).Select(c => new { c.Id, c.employee }), "Id", "employee");

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

            ViewBag.SampleTask = new SelectList(db.wmpRepairSampleTasks.Where(c => c.IsActive == true && c.description != null).OrderBy(c => c.description).Select(c => new { c.Id, c.description }), "Id", "description");

            return View();
        }
        public JsonResult searchRepairWithoutSession(GetRepairWithoutSession model)
        {
            try
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                wmpRepairStatusLog log = new wmpRepairStatusLog { Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber, RepairNumber = model.RepairNumber, Zipcode = model.ZipCode, TimeStamp = DateTime.Now, Type = "Search", IPAddress = myIP };
                db.wmpRepairStatusLogs.Add(log);
                db.SaveChanges();
                var _items = db.Database.SqlQuery<RepairWithoutSession>("USP_SearchRepairWithoutSessionOnAllFields @p0, @p1, @p2, @p3, @p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11", model.Email, model.RepairNumber, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, SessionMaster.Current.OwnerID, model.FirstName, model.LastName, model.PhoneNumber, model.ZipCode, log.LogId.ToString());
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
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
    }
}
