using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{

    public class TaskModel : PageingModel
    {
        public long Id { get; set; }
        public string description { get; set; }

        public string Type { get; set; }

    }

    public class StyleModel : PageingModel
    {
        public long Id { get; set; }
        public string style { get; set; }

    }
    public class MovementModel : PageingModel
    {
        public long Id { get; set; }
        public string movementType { get; set; }

    }
    public class ItemModel : PageingModel
    {
        public long Id { get; set; }
        public string item { get; set; }

        public string BrandName { get; set; }
    }
    public class CaseModel : PageingModel
    {
        public long Id { get; set; }
        public string caseType { get; set; }
    }
    public class BuckleModel : PageingModel
    {
        public long Id { get; set; }
        public string buckleType { get; set; }
    }
    public class DialModel : PageingModel
    {
        public long Id { get; set; }
        public string dialType { get; set; }
    }
    public class ConditionModel : PageingModel
    {
        public long Id { get; set; }
        public string condition { get; set; }
        public string Type { get; set; }
    }
    public class PurchaseLocationModel : PageingModel
    {
        public long Id { get; set; }
        public string name { get; set; }
    }
    public class PackagingIncludedModel : PageingModel
    {
        public int BoxIncludedID { get; set; }
        public string BoxIncluded { get; set; }
    }
    public class SecurityModel : PageingModel
    {
        public long Id { get; set; }
        public string idSecurity { get; set; }
    }
    public class RepairTypeModel : PageingModel
    {
        public long Id { get; set; }
        public string repairType { get; set; }
    }
    public class StatusModel : PageingModel
    {
        public long Id { get; set; }
        public string statusName { get; set; }
    }
    public class WarrantyDocModel : PageingModel
    {
        public long Id { get; set; }
        public string WarrantyDocument { get; set; }
    }
    public class AppraiserTitleModel : PageingModel
    {
        public long Id { get; set; }
        public string appraiserTitle { get; set; }
    }
    public class CapitalizationModel : PageingModel
    {
        public long Id { get; set; }
        public string capitalization { get; set; }
    }
    public class FontModel : PageingModel
    {
        public long Id { get; set; }
        public string font { get; set; }
    }
    public class CarrierModel : PageingModel
    {
        public long Id { get; set; }
        public string Carrier { get; set; }
    }
    public class CountryModel : PageingModel
    {
        public long Id { get; set; }
        public string country { get; set; }
    }
    public class StateModel : PageingModel
    {
        public long Id { get; set; }
        public string state { get; set; }
        public string stateFullName { get; set; }

    }
    public class CityModel : PageingModel
    {
        public long Id { get; set; }
        public string city { get; set; }
        public long stateId { get; set; }
        public long countryId { get; set; }

        public string ZipCode { get; set; }

    }


    public class AllMasterType
    {
        public long Id { get; set; }
        //public string item { get; set; }
        //public string style { get; set; }
        //public string movementType { get; set; }
        //public string caseType { get; set; }
        //public string BandType { get; set; }
        //public string buckleType { get; set; }
        //public string dialType { get; set; }
        //public string condition { get; set; }
        //public string BezelCondition { get; set; }
        //public string BackCondition { get; set; }
        //public string BuckleCondition { get; set; }

        public string Type { get; set; }
        public string Value { get; set; }

    }
    public class AllMasterTypeList
    {
        public List<AllMasterType> obj { get; set; }

    }


    public class MasterController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();

        #region Item Block
        public ActionResult Item()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getItemList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<ItemModel>("USP_GetItemList @p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddItem(wmpItem model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpItems.Any(c => c.item == model.item && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpItems.Add(model);
                    }
                    else
                    {
                        if (db.wmpItems.Any(c => c.item == model.item && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditItem(int id)
        {
            var _fetch = db.wmpItems.Find(id);
            wmpItem _model = new wmpItem() { Id = _fetch.Id, item = _fetch.item, IsActive = _fetch.IsActive, brandId = _fetch.brandId };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Style Block
        public ActionResult Style()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getStyleList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<StyleModel>("USP_GetStyleList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddStyle(wmpStyle model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }

                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpStyles.Any(c => c.style == model.style && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpStyles.Add(model);
                    }
                    else
                    {
                        if (db.wmpStyles.Any(c => c.style == model.style && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditStyle(int id)
        {
            var _fetch = db.wmpStyles.Find(id);
            wmpStyle _model = new wmpStyle() { Id = _fetch.Id, brandId=_fetch.brandId, style = _fetch.style, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Style Movement
        public ActionResult Movement()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getMovementList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<MovementModel>("USP_GetMovementList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddMovement(wmpMovementType model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }


                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpMovementTypes.Any(c => c.movementType == model.movementType && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpMovementTypes.Add(model);
                    }
                    else
                    {
                        if (db.wmpMovementTypes.Any(c => c.movementType == model.movementType && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditMovement(int id)
        {
            var _fetch = db.wmpMovementTypes.Find(id);
            wmpMovementType _model = new wmpMovementType() { Id = _fetch.Id, brandId=_fetch.brandId, movementType = _fetch.movementType, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Case Type Block
        public ActionResult Case()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getCaseList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CaseModel>("USP_GetCaseList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddCase(wmpCaseType model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }

                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpCaseTypes.Any(c => c.caseType == model.caseType && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpCaseTypes.Add(model);
                    }
                    else
                    {
                        if (db.wmpCaseTypes.Any(c => c.caseType == model.caseType && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCase(int id)
        {
            var _fetch = db.wmpCaseTypes.Find(id);
            wmpCaseType _model = new wmpCaseType() { Id = _fetch.Id, caseType = _fetch.caseType, brandId=_fetch.brandId, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Buckle Type Block
        public ActionResult Buckle()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getBuckleList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<BuckleModel>("USP_GetBuckleList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddBuckle(wmpBuckleType model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }


                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpBuckleTypes.Any(c => c.buckleType == model.buckleType && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpBuckleTypes.Add(model);
                    }
                    else
                    {
                        if (db.wmpBuckleTypes.Any(c => c.buckleType == model.buckleType && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditBuckle(int id)
        {
            var _fetch = db.wmpBuckleTypes.Find(id);
            wmpBuckleType _model = new wmpBuckleType() { Id = _fetch.Id, buckleType = _fetch.buckleType, IsActive = _fetch.IsActive,brandId=_fetch.brandId };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Dial Type Block
        public ActionResult Dial()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getDialList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<DialModel>("USP_GetDialList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddDial(wmpDialType model)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }

                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpDialTypes.Any(c => c.dialType == model.dialType && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpDialTypes.Add(model);
                    }
                    else
                    {
                        if (db.wmpDialTypes.Any(c => c.dialType == model.dialType && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditDial(int id)
        {
            var _fetch = db.wmpDialTypes.Find(id);
            wmpDialType _model = new wmpDialType() { Id = _fetch.Id, brandId=_fetch.brandId, dialType = _fetch.dialType, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Conditions: Crystal, Lugs, Case, Band, Dial
        public ActionResult Condition()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getConditionList(SearchFilters model,string Type)
        {
            try
            {
                var _items = db.Database.SqlQuery<ConditionModel>("USP_GetConditionList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8", model.Name == null ? "" : model.Name, Type, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddCondition(wmpCrystalCondition model,string Type)
        {
            try
            {
                if (model.brandId == null)
                {
                    model.brandId = 0;
                }

                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        switch (Type)
                        {
                            case "Crystal":
                                if (db.wmpCrystalConditions.Any(c => c.condition == model.condition && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }

                                db.wmpCrystalConditions.Add(model);
                                break;
                            case "Lugs":
                                if (db.wmpLugsConditions.Any(c => c.condition == model.condition && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpLugsCondition tem = new wmpLugsCondition() { LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive,brandId=model.brandId };
                                db.wmpLugsConditions.Add(tem);
                                break;
                            case "Case":
                                  if (db.wmpCaseConditions.Any(c => c.condition == model.condition && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                  wmpCaseCondition tem2 = new wmpCaseCondition() { LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.wmpCaseConditions.Add(tem2);
                                break;
                            case "Band":
                                if (db.wmpBandConditions.Any(c => c.condition == model.condition && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpBandCondition tempBand = new wmpBandCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.wmpBandConditions.Add(tempBand);
                                break;
                            case "Dial":
                                if (db.wmpDialConditions.Any(c => c.condition == model.condition && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpDialCondition tempDial = new wmpDialCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.wmpDialConditions.Add(tempDial);
                                break;
                        }
                    }
                    else
                    {
                        switch (Type)
                        {
                            case "Crystal":
                                if (db.wmpCrystalConditions.Any(c => c.condition == model.condition && c.Id != model.Id && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                db.Entry(model).State = EntityState.Modified;
                                break;
                            case "Lugs":
                                if (db.wmpLugsConditions.Any(c => c.condition == model.condition && c.Id != model.Id && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpLugsCondition tem = new wmpLugsCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.Entry(tem).State = EntityState.Modified;
                                break;
                            case "Case":
                                if (db.wmpCaseConditions.Any(c => c.condition == model.condition && c.Id != model.Id && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpCaseCondition tem2 = new wmpCaseCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.Entry(tem2).State = EntityState.Modified;
                                break;
                            case "Band":
                                if (db.wmpBandConditions.Any(c => c.condition == model.condition && c.Id != model.Id && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpBandCondition tempBand = new wmpBandCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.Entry(tempBand).State = EntityState.Modified;
                                break;
                            case "Dial":
                                if (db.wmpDialConditions.Any(c => c.condition == model.condition && c.Id != model.Id && c.OwnerID == model.OwnerID))
                                {
                                    return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                                }
                                wmpDialCondition tempDial = new wmpDialCondition() { Id = model.Id, LastUpdate = DateTime.Now, UpdateBy = SessionMaster.Current.LoginId, OwnerID = SessionMaster.Current.OwnerID, condition = model.condition, IsActive = model.IsActive, brandId = model.brandId };
                                db.Entry(tempDial).State = EntityState.Modified;
                                break;
                        }
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCondition(int id, string Type)
        {
            switch (Type)
            {
                case "Crystal":
                    var _fetch = db.wmpCrystalConditions.Find(id);
                    wmpCrystalCondition _model = new wmpCrystalCondition() { Id = _fetch.Id, condition = _fetch.condition, IsActive = _fetch.IsActive,brandId=_fetch.brandId };
                    return Json(_model, JsonRequestBehavior.AllowGet);

                case "Lugs":
                    var _fetchLugs = db.wmpLugsConditions.Find(id);
                    wmpLugsCondition _modelLugs = new wmpLugsCondition() { Id = _fetchLugs.Id, condition = _fetchLugs.condition, IsActive = _fetchLugs.IsActive, brandId = _fetchLugs.brandId };
                    return Json(_modelLugs, JsonRequestBehavior.AllowGet);

                case "Case":
                    var _fetchCase = db.wmpCaseConditions.Find(id);
                    wmpCaseCondition _modelCase = new wmpCaseCondition() { Id = _fetchCase.Id, condition = _fetchCase.condition, IsActive = _fetchCase.IsActive, brandId = _fetchCase.brandId };
                    return Json(_modelCase, JsonRequestBehavior.AllowGet);

                case "Band":
                    var _fetchBand = db.wmpBandConditions.Find(id);
                    wmpBandCondition _modelBand = new wmpBandCondition() { Id = _fetchBand.Id, condition = _fetchBand.condition, IsActive = _fetchBand.IsActive, brandId = _fetchBand.brandId };
                    return Json(_modelBand, JsonRequestBehavior.AllowGet);

                case "Dial":
                    var _fetchDial = db.wmpDialConditions.Find(id);
                    wmpDialCondition _modelDial = new wmpDialCondition() { Id = _fetchDial.Id, condition = _fetchDial.condition, IsActive = _fetchDial.IsActive, brandId = _fetchDial.brandId };
                    return Json(_modelDial, JsonRequestBehavior.AllowGet);
            }
          return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Purchase Location Block
        public ActionResult PurchaseLocation()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getPurchaseLocationList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<PurchaseLocationModel>("USP_GetPurchaseLocationList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddPurchaseLocation(wmpPurchaseLocation model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpPurchaseLocations.Any(c => c.name == model.name && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpPurchaseLocations.Add(model);
                    }
                    else
                    {
                        if (db.wmpPurchaseLocations.Any(c => c.name == model.name && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditPurchaseLocation(int id)
        {
            var _fetch = db.wmpPurchaseLocations.Find(id);
            wmpPurchaseLocation _model = new wmpPurchaseLocation() { brandId=_fetch.brandId, Id = _fetch.Id, name = _fetch.name, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Packaging Included Block
        public ActionResult PackagingIncluded()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getPackagingIncludedList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<PackagingIncludedModel>("USP_GetPackagingIncludedList @p0, @p1, @p2, @p3, @p4, @p5,@p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddPackagingIncluded(wmpBoxIncludedMaster model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.BoxIncludedID == 0)
                    {
                        if (db.wmpBoxIncludedMasters.Any(c => c.BoxIncluded == model.BoxIncluded && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpBoxIncludedMasters.Add(model);
                    }
                    else
                    {
                        if (db.wmpBoxIncludedMasters.Any(c => c.BoxIncluded == model.BoxIncluded && c.BoxIncludedID != model.BoxIncludedID && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditPackagingIncluded(int id)
        {
            var _fetch = db.wmpBoxIncludedMasters.Find(id);
            wmpBoxIncludedMaster _model = new wmpBoxIncludedMaster() { brandId = _fetch.brandId, BoxIncludedID = _fetch.BoxIncludedID, BoxIncluded = _fetch.BoxIncluded, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Security Block
        public ActionResult Security()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getSecurityList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<SecurityModel>("USP_GetSecurityList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddSecurityList(wmpIDSecurity model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpIDSecurities.Any(c => c.idSecurity == model.idSecurity && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpIDSecurities.Add(model);
                    }
                    else
                    {
                        if (db.wmpIDSecurities.Any(c => c.idSecurity == model.idSecurity && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditSecurity(int id)
        {
            var _fetch = db.wmpIDSecurities.Find(id);
            wmpIDSecurity _model = new wmpIDSecurity() { brandId=_fetch.brandId,   Id = _fetch.Id, idSecurity = _fetch.idSecurity, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Repair Type Block
        public ActionResult RepairType()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getRepairTypeList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<RepairTypeModel>("USP_GetRepairTypeList @p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddRepairType(wmpRepairType model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpRepairTypes.Any(c => c.repairType == model.repairType && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpRepairTypes.Add(model);
                    }
                    else
                    {
                        if (db.wmpRepairTypes.Any(c => c.repairType == model.repairType && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditRepairType(int id)
        {
            var _fetch = db.wmpRepairTypes.Find(id);
            wmpRepairType _model = new wmpRepairType() {  brandId=_fetch.brandId, Id = _fetch.Id, repairType = _fetch.repairType, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Status Block
        public ActionResult Status()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getStatusList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<StatusModel>("USP_GetStatusList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddStatus(wmpStatu model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpStatus.Any(c => c.status == model.status && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result {      Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpStatus.Add(model);
                    }
                    else
                    {
                        if (db.wmpStatus.Any(c => c.status == model.status && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Editstatus(int id)
        {
            var _fetch = db.wmpStatus.Find(id);
            wmpStatu _model = new wmpStatu() {  brandId=_fetch.brandId, Id = _fetch.Id, status = _fetch.status, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Warranty Doc Block
        public ActionResult WarrantyDoc()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getWarrantyDocList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<WarrantyDocModel>("USP_GetWarrantyDocList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddWarrantyDoc(wmpWarrantyDocument model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpWarrantyDocuments.Any(c => c.WarrantyDocument == model.WarrantyDocument && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpWarrantyDocuments.Add(model);
                    }
                    else
                    {
                        if (db.wmpWarrantyDocuments.Any(c => c.WarrantyDocument == model.WarrantyDocument && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditWarrantyDoc(int id)
        {
            var _fetch = db.wmpWarrantyDocuments.Find(id);
            wmpWarrantyDocument _model = new wmpWarrantyDocument() {  brandId=_fetch.brandId, Id = _fetch.Id, WarrantyDocument = _fetch.WarrantyDocument, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Appraiser Title Block
        public ActionResult AppraiserTitle()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getAppraiserTitleList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<AppraiserTitleModel>("USP_GetAppraiserTitleList @p0, @p1, @p2, @p3, @p4, @p5, @p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddAppraiserTitle(wmpAppraiserTitle model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpAppraiserTitles.Any(c => c.appraiserTitle == model.appraiserTitle && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpAppraiserTitles.Add(model);
                    }
                    else
                    {
                        if (db.wmpAppraiserTitles.Any(c => c.appraiserTitle == model.appraiserTitle && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditAppraiserTitle(int id)
        {
            var _fetch = db.wmpAppraiserTitles.Find(id);
            wmpAppraiserTitle _model = new wmpAppraiserTitle() { Id = _fetch.Id, appraiserTitle = _fetch.appraiserTitle, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Capitalization Block
        public ActionResult Capitalization()
        {
            ViewBag.Brand = new SelectList(db.wmpBrandMasters.Where(c => c.IsActive == true).OrderBy(c => c.BrandName).Select(c => new { c.BrandID, c.BrandName }), "BrandID", "BrandName");
            return View();
        }
        [HttpPost]
        public JsonResult getCapitalizationList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CapitalizationModel>("USP_GetCapitalizationList @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.BrandId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddCapitalization(wmpEngravingCapitalization model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpEngravingCapitalizations.Any(c => c.capitalization == model.capitalization && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpEngravingCapitalizations.Add(model);
                    }
                    else
                    {
                        if (db.wmpEngravingCapitalizations.Any(c => c.capitalization == model.capitalization && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCapitalization(int id)
        {
            var _fetch = db.wmpEngravingCapitalizations.Find(id);
            wmpEngravingCapitalization _model = new wmpEngravingCapitalization() { brandId=_fetch.brandId, Id = _fetch.Id, capitalization = _fetch.capitalization, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Font Block
        public ActionResult Font()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getFontList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<FontModel>("USP_GetFontList @p0, @p1, @p2, @p3, @p4, @p5,@p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddFont(wmpEngravingFont model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpEngravingFonts.Any(c => c.font == model.font && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpEngravingFonts.Add(model);
                    }
                    else
                    {
                        if (db.wmpEngravingFonts.Any(c => c.font == model.font && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditFont(int id)
        {
            var _fetch = db.wmpEngravingFonts.Find(id);
            wmpEngravingFont _model = new wmpEngravingFont() { Id = _fetch.Id, font = _fetch.font, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Carrier
        public ActionResult Carrier()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getCarrierList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CarrierModel>("USP_GetCarrierList @p0, @p1, @p2, @p3, @p4, @p5, @p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddCarrier(wmpCarrier model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpCarriers.Any(c => c.carrier == model.carrier && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpCarriers.Add(model);
                    }
                    else
                    {
                        if (db.wmpCarriers.Any(c => c.carrier == model.carrier && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCarrier(int id)
        {
            var _fetch = db.wmpCarriers.Find(id);
            wmpCarrier _model = new wmpCarrier() { Id = _fetch.Id, carrier = _fetch.carrier, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Country
        public ActionResult Countries()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getCountryList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CountryModel>("USP_GetCountryList @p0, @p1, @p2, @p3, @p4, @p5, @p6", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddCountry(wmpCountry model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpCountries.Any(c => c.country == model.country && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpCountries.Add(model);
                    }
                    else
                    {
                        if (db.wmpCountries.Any(c => c.country == model.country && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCountry(int id)
        {
            var _fetch = db.wmpCountries.Find(id);
            wmpCountry _model = new wmpCountry() { Id = _fetch.Id, country = _fetch.country, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region States
        public ActionResult States()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            return View();
        }
        [HttpPost]
        public JsonResult getStateList(StateFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<StateModel>("USP_GetStateList @p0, @p1, @p2, @p3, @p4, @p5,@p6, @p7", model.Name == null ? "" : model.Name,model.CountryId, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddState(wmpState model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpStates.Any(c => c.state == model.state && c.CountryId==model.CountryId && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpStates.Add(model);
                    }
                    else
                    {
                        if (db.wmpStates.Any(c => c.state == model.state && c.Id != model.Id && c.CountryId == model.CountryId  && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditState(int id)
        {
            var _fetch = db.wmpStates.Find(id);
            wmpState _model = new wmpState() { Id = _fetch.Id, state = _fetch.state,stateFullName=_fetch.stateFullName,CountryId=_fetch.CountryId, IsActive = _fetch.IsActive };
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region cities
        public ActionResult cities()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            return View();
        }
        [HttpPost]
        public JsonResult getCityList(CityFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CityModel>("USP_GetCityList @p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7,@p8", model.Name == null ? "" : model.Name, model.StateId, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder,model.CountryId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            } 
        }
        [HttpPost]
        public JsonResult AddCity(CityModelNew model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpSampleCities.Any(c => c.city == model.city && c.StateId==model.StateId && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        //if (db.wmpZipCodes.Any(s => s.PostalCode == model.ZipCode))
                        //{
                        //    return Json(new Result { Status = false, Message = "Zip-code already exists" }, JsonRequestBehavior.AllowGet);
                        //}


                        wmpSampleCity obj = new wmpSampleCity() { city = model.city, CountyID = model.CountyID, IsActive = true, LastUpdate = DateTime.Now, OwnerID = SessionMaster.Current.OwnerID, StateId = model.StateId, UpdateBy = SessionMaster.Current.LoginId };

                        db.wmpSampleCities.Add(obj);
                    }
                    else
                    {
                        if (db.wmpSampleCities.Any(c => c.city == model.city && c.StateId==model.StateId  && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        //if (db.wmpZipCodes.Any(s => s.PostalCode == model.ZipCode && s.CityId!=model.Id))
                        //{
                        //    return Json(new Result { Status = false, Message = "Zip-code already exists with other city name" }, JsonRequestBehavior.AllowGet);
                        //}


                        wmpSampleCity obj = new wmpSampleCity() { Id=model.Id, city = model.city, CountyID = model.CountyID, IsActive = (bool)model.IsActive, LastUpdate = DateTime.Now, OwnerID = SessionMaster.Current.OwnerID, StateId = model.StateId, UpdateBy = SessionMaster.Current.LoginId };


                        db.Entry(obj).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelCity in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelCity.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditCity(int id)
        {
            var _fetch = db.wmpSampleCities.Find(id);
            wmpSampleCity _model = new wmpSampleCity() { Id = _fetch.Id, CountyID=_fetch.CountyID, city = _fetch.city, StateId = _fetch.StateId, IsActive = _fetch.IsActive };

            // wmpZipCode zipObj = db.wmpZipCodes.Where(s => s.CityId == _model.Id).FirstOrDefault();
            //, ZipCode=zipObj.PostalCode
            CityModelNew modelObject = new CityModelNew { Id = _model.Id, CountyID = _model.CountyID, city = _model.city, StateId = _model.StateId, IsActive = _model.IsActive };


            return Json(modelObject, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult getStates(int Id)
        {
            try
            {
                return Json(db.wmpStates.Where(c => c.IsActive == true && c.CountryId == Id).OrderBy(c => c.stateFullName).Select(c => new { c.Id, c.stateFullName }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }
        #endregion

        [HttpGet]
        public JsonResult getCounty(int Id)
        {
            try
            {
                return Json(db.wmpCounties.Where(c => c.IsActive == 1 && c.StateId == Id).OrderBy(c => c.County).Select(c => new { c.Id, c.County }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }


        [HttpGet]
        public JsonResult getCityByCountyId(int Id)
        {
            try
            {
                return Json(db.wmpSampleCities.Where(c => c.IsActive == true && c.CountyID == Id).OrderBy(c => c.city).Select(c => new { c.Id, c.city}), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new Result { Status = false, Message = ex.Message });
            }
        }

        #region Task Block
        public ActionResult Tasks()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getTaskList(SearchFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<TaskModel>("USP_GetTaskList @p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7", model.Name == null ? "" : model.Name, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.ActiveOrAllCheck, model.Type);

                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public JsonResult AddTask(wmpRepairSampleTask model)
        {
            try
            {
                model.LastUpdate = DateTime.Now;
                model.UpdateBy = SessionMaster.Current.LoginId;
                model.OwnerID = SessionMaster.Current.OwnerID;
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpRepairSampleTasks.Any(c => c.description == model.description && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.wmpRepairSampleTasks.Add(model);
                    }
                    else
                    {
                        if (db.wmpRepairSampleTasks.Any(c => c.description == model.description && c.Id != model.Id && c.OwnerID == model.OwnerID))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }

                        db.Entry(model).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new Result { Status = true, Message = Messages.recordSaved }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _error = string.Empty;
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            _error = _error + error;
                        }
                    }
                    return Json(new Result { Status = false, Message = _error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EditTask(int id)
        {
            var _fetch = db.wmpRepairSampleTasks.Find(id);
            wmpRepairSampleTask _model = new wmpRepairSampleTask() { Id = _fetch.Id, Type=_fetch.Type,  description = _fetch.description, IsActive = _fetch.IsActive };
            //, Status = true
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpGet]
        public JsonResult getItemByBrand(int id)
        {
            try
            {
                var _items = db.Database.SqlQuery<AllMasterType>("USP_ChangeBrandTypeValue @p0", id);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }


            //if (id == 0)
            //{
            //    var x = db.wmpItems.OrderBy(c => c.item).Select(c => new { c.Id, c.item });
            //    return Json(x, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    var x = db.wmpItems.Where(c => c.brandId == id).OrderBy(c => c.item).Select(c => new { c.Id, c.item });
            //    return Json(x, JsonRequestBehavior.AllowGet);
            //}
        }

    }

}