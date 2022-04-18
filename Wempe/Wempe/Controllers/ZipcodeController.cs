using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wempe.CommonClasses;
using Wempe.Models;

namespace Wempe.Controllers
{
    public class ZipcodeController : Controller
    {
        dbWempeEntities db = new dbWempeEntities();

        //
        // GET: /Zipcode/

        public ActionResult index()
        {
            ViewBag.Countries = new SelectList(db.wmpCountries.Where(c => c.IsActive == true).OrderBy(c => c.country).Select(c => new { c.Id, c.country }), "Id", "country", 253);
            return View();
        }


        [HttpPost]
        public JsonResult AddZipcode(wmpZipCode model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        if (db.wmpZipCodes.Any(s => s.PostalCode == model.PostalCode))
                        {
                            return Json(new Result { Status = false, Message = Messages.recordAlreadyExists }, JsonRequestBehavior.AllowGet);
                        }
                        db.wmpZipCodes.Add(model);
                    }
                    else
                    {
                        db.Entry(model).State = EntityState.Modified;
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
        public JsonResult EditZipcode(int id)
        {
            var _fetch = db.wmpZipCodes.Find(id);

            wmpSampleCity cityObj = db.wmpSampleCities.Where(s => s.Id == _fetch.CityId).FirstOrDefault();
            //wmpSampleCity _model = new wmpSampleCity() { Id = _fetch.Id, CountyID = _fetch.CountyID, city = _fetch.city, StateId = _fetch.StateId, IsActive = _fetch.IsActive };

            // wmpZipCode zipObj = db.wmpZipCodes.Where(s => s.CityId == _model.Id).FirstOrDefault();
            //, ZipCode=zipObj.PostalCode

            wmpState objState = db.wmpStates.Where(s => s.Id == cityObj.StateId).FirstOrDefault();

            CityModelNew modelObject = new CityModelNew { CountryId= objState.CountryId, Id = _fetch.Id, CountyID = cityObj.CountyID, city = cityObj.Id.ToString(), StateId = cityObj.StateId, IsActive = _fetch.IsActive, ZipCode=_fetch.PostalCode };


            return Json(modelObject, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult getZipcodeList(ZipcodeFilters model)
        {
            try
            {
                var _items = db.Database.SqlQuery<CityModel>("USP_GetZipcodeList @p0, @p1, @p2, @p3, @p4, @p5,@p6,@p7,@p8,@p9,@p10", model.Name == null ? "" : model.Name, model.StateId, SessionMaster.Current.OwnerID, model.pageNo, Convert.ToInt32(MainSetting.pageSize), model.sortColumn, model.sortOrder, model.CountryId,model.CountyId,model.CityId, model.ActiveOrAllCheck);
                return Json(_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    }
}
