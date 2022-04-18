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
    public class SalaryRangeService : ISalaryRange
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public SalaryRangeService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<SalaryRangeModel>> GetSalaryRangeModel()
        {
            try
            {
                var _SalaryRange = await _context.SalaryRanges.ToListAsync();
                List<SalaryRangeModel> _lstModel = new List<SalaryRangeModel>();
                _lstModel.AddRange(_SalaryRange.Select(g => new SalaryRangeModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    SalaryRangeId = g.SalaryRangeId,
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
                AppUtility.LogMessage(ex, "GetSalaryRangeModel", "SalaryRangeService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreateSalaryRange(SalaryRangeModel _model)
        {
            try
            {
                var SalaryRange = await _repository.FindAsync<SalaryRange>(x => x.Name == _model.Name);

                if (SalaryRange != null)
                {
                    return new ResponseModel { Message = "Salary Range Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                SalaryRange SalaryRangeToInsert = new SalaryRange
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    SalaryKey = "SR" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int SalaryRangeId = await _repository.Insert<SalaryRange>(SalaryRangeToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = SalaryRangeToInsert.SalaryRangeId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateSalaryRange", "SalaryRangeService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateSalaryRange(SalaryRangeModel _model)
        {
            try
            {
                var _SalaryRange = await _repository.FindAsync<SalaryRange>(x => x.SalaryRangeId == _model.SalaryRangeId);
                if (_SalaryRange != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _SalaryRange.Name = _model.Name;
                    _SalaryRange.Description = _model.Description;
                    _SalaryRange.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _SalaryRange.DisplayColorCode = _model.DisplayColorCode;
                    _SalaryRange.Number = _model.Number;
                    _SalaryRange.Active = _model.Active;
                    _SalaryRange.MaxValue = _model.MaxValue;
                    _SalaryRange.MinValue = _model.MinValue;
                    _SalaryRange.UpdateDateTime = DateTime.Now;
                    _SalaryRange.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.SalaryRangeId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateSalaryRange", "SalaryRangeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindSalaryRangeDropDown(int? organizationID)
        {
            try
            {
                var _SalaryRange = await _context.SalaryRanges.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListSalaryRange = new List<SelectListItem>();
                _ListSalaryRange.AddRange(_SalaryRange.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.SalaryRangeId.ToString() }).ToList());
                return _ListSalaryRange;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindSalaryRangeDropDown", "SalaryRangeService.cs");
                throw;
            }
        }
    }
}
