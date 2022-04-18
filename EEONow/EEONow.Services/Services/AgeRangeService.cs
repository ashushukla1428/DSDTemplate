using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EEONow.Context.EntityContext;
using EEONow.Utilities;
using System.Data.Entity;

namespace EEONow.Services
{
    public class AgeRangeService : IAgeRange
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public AgeRangeService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<AgeRangeModel>> GetAgeRangeModel()
        {
            try
            {
                var _AgeRange = await _context.AgeRanges.ToListAsync();
                List<AgeRangeModel> _lstModel = new List<AgeRangeModel>();
                _lstModel.AddRange(_AgeRange.Select(g => new AgeRangeModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    AgeRangeId = g.AgeRangeId,
                    Number = g.Number,
                    MaxValue = g.MaxValue,
                    MinValue = g.MinValue,
                    OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                    OrganizationName = g.Organization == null ? "" : g.Organization.Name
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAgeRangeModel", "AgeRangeService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreateAgeRange(AgeRangeModel _model)
        {
            try
            {
                var AgeRange = await _repository.FindAsync<AgeRange>(x => x.Name == _model.Name);

                if (AgeRange != null)
                {
                    return new ResponseModel { Message = "Age Range Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                AgeRange AgeRangeToInsert = new AgeRange
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    AgeRangeKey = "AR" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int AgeRangeId = await _repository.Insert<AgeRange>(AgeRangeToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = AgeRangeToInsert.AgeRangeId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateAgeRange", "AgeRangeService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateAgeRange(AgeRangeModel _model)
        {
            try
            {
                var _AgeRange = await _repository.FindAsync<AgeRange>(x => x.AgeRangeId == _model.AgeRangeId);
                if (_AgeRange != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _AgeRange.Name = _model.Name;
                    _AgeRange.Description = _model.Description;
                    _AgeRange.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _AgeRange.DisplayColorCode = _model.DisplayColorCode;
                    _AgeRange.Number = _model.Number;
                    _AgeRange.Active = _model.Active;
                    _AgeRange.MaxValue = _model.MaxValue;
                    _AgeRange.MinValue = _model.MinValue;
                    _AgeRange.UpdateDateTime = DateTime.Now;
                    _AgeRange.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.AgeRangeId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateAgeRange", "AgeRangeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindAgeRangeDropDown(int? organizationID)
        {
            try
            {
                var _AgeRange = await _context.AgeRanges.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListAgeRange = new List<SelectListItem>();
                _ListAgeRange.AddRange(_AgeRange.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.AgeRangeId.ToString() }).ToList());
                return _ListAgeRange;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindAgeRangeDropDown", "AgeRangeService.cs");
                throw;
            }
        }
    }
}
