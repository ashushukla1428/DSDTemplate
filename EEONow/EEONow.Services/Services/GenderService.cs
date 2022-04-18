using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEONow.Context;
using System.Configuration;
using System.Web.Mvc;
using EEONow.Context.EntityContext;
using System.Web;
using EEONow.Utilities;
using System.Data.Entity;

namespace EEONow.Services
{
    public class GenderService : IGendersService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public GenderService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<GenderModel>> GetGenders()
        {
            try
            {
                var _ListOrganization = await _context.Organizations.Where(e => e.Active == true).Select(e => new { e.OrganizationId, e.Name, e.Genders }).ToListAsync();
                List<GenderModel> _LstModel = new List<GenderModel>();
                foreach (var item in _ListOrganization)
                {

                    if (item.Genders != null && item.Genders.Count() > 0)
                    {
                        GenderModel _model = new GenderModel();
                        _model.OrganizationId = item.OrganizationId;
                        _model.Organization = item.Name;
                        foreach (var itemGender in item.Genders)
                        {
                            if (itemGender.Name == "Male")
                            {
                                _model.MaleDisplayColorCode = itemGender.DisplayColorCode;
                                _model.Active = itemGender.Active;
                            }
                            else
                            {
                                _model.FemaleDisplayColorCode = itemGender.DisplayColorCode;
                            }
                        }
                        _LstModel.Add(_model);
                    }
                    else
                    {
                        GenderModel _model = new GenderModel();
                        _model.OrganizationId = item.OrganizationId;
                        _model.Organization = item.Name;
                        _model.MaleDisplayColorCode = "";
                        _model.FemaleDisplayColorCode = "";
                        _model.Active = false;
                        _LstModel.Add(_model);
                    }

                }

                return _LstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetGenders", "GenderService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateGender(GenderModel model)
        {
            try
            {

                var IsExistGender = await _context.Genders.Where(e => e.Organization.OrganizationId == model.OrganizationId).ToListAsync();

                if (IsExistGender != null && IsExistGender.Count() > 0)
                {
                    foreach (var item in IsExistGender)
                    {
                        if (item.Name == "Male")
                        {
                            item.Active = model.Active;
                            item.DisplayColorCode = model.MaleDisplayColorCode;
                        }
                        else
                        {
                            item.Active = model.Active;
                            item.DisplayColorCode = model.FemaleDisplayColorCode;
                        }

                    }
                }
                else
                {
                    List<Gender> _lstGender = new List<Gender>();
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    Gender _MaleGenderToInsert = new Gender();
                    _MaleGenderToInsert.Organization = await _context.Organizations.Where(e => e.OrganizationId == model.OrganizationId).FirstOrDefaultAsync();
                    _MaleGenderToInsert.Name = "Male";
                    _MaleGenderToInsert.Description = "Male";
                    _MaleGenderToInsert.Active = model.Active;
                    _MaleGenderToInsert.DisplayColorCode = model.MaleDisplayColorCode;
                    _MaleGenderToInsert.CreateUserId = _user;
                    _MaleGenderToInsert.CreateDateTime = DateTime.Now;
                    _MaleGenderToInsert.UpdateUserId = _user;
                    _MaleGenderToInsert.UpdateDateTime = DateTime.Now;

                    _lstGender.Add(_MaleGenderToInsert);

                    Gender _FemaleGenderToInsert = new Gender();
                    _FemaleGenderToInsert.Organization = await _context.Organizations.Where(e => e.OrganizationId == model.OrganizationId).FirstOrDefaultAsync();
                    _FemaleGenderToInsert.Name = "Female";
                    _FemaleGenderToInsert.Description = "Female";
                    _FemaleGenderToInsert.Active = model.Active;
                    _FemaleGenderToInsert.DisplayColorCode = model.MaleDisplayColorCode;
                    _FemaleGenderToInsert.CreateUserId = _user;
                    _FemaleGenderToInsert.CreateDateTime = DateTime.Now;
                    _FemaleGenderToInsert.UpdateUserId = _user;
                    _FemaleGenderToInsert.UpdateDateTime = DateTime.Now;
                    _lstGender.Add(_FemaleGenderToInsert);
                    var result = _context.Genders.AddRange(_lstGender);
                }
                _context.SaveChanges();

                return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = 1 };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateGender", "GenderService.cs");
                throw;
            }
        }
    }
}
