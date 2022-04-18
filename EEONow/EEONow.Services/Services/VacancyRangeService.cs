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
    public class VacancyRangeService : IVacancyRange
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public VacancyRangeService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<VacancyRangeModel>> GetVacancyRangeModel()
        {
            try
            {
                var _VacancyRange = await _context.VacancyRanges.ToListAsync();
                List<VacancyRangeModel> _lstModel = new List<VacancyRangeModel>();
                _lstModel.AddRange(_VacancyRange.Select(g => new VacancyRangeModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    VacancyRangeId = g.VacancyRangeId,
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
                AppUtility.LogMessage(ex, "GetVacancyRangeModel", "VacancyRangeService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreateVacancyRange(VacancyRangeModel _model)
        {
            try
            {
                var VacancyRange = await _repository.FindAsync<VacancyRange>(x => x.Name == _model.Name);

                if (VacancyRange != null)
                {
                    return new ResponseModel { Message = "VacancyRange Range Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                VacancyRange VacancyRangeToInsert = new VacancyRange
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    VacancyRangeKey = "VR" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int VacancyRangeId = await _repository.Insert<VacancyRange>(VacancyRangeToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = VacancyRangeToInsert.VacancyRangeId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateVacancyRange", "VacancyRangeService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateVacancyRange(VacancyRangeModel _model)
        {
            try
            {
                var _VacancyRange = await _repository.FindAsync<VacancyRange>(x => x.VacancyRangeId == _model.VacancyRangeId);
                if (_VacancyRange != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _VacancyRange.Name = _model.Name;
                    _VacancyRange.Description = _model.Description;
                    _VacancyRange.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _VacancyRange.DisplayColorCode = _model.DisplayColorCode;
                    _VacancyRange.Number = _model.Number;
                    _VacancyRange.Active = _model.Active;
                    _VacancyRange.MaxValue = _model.MaxValue;
                    _VacancyRange.MinValue = _model.MinValue;
                    _VacancyRange.UpdateDateTime = DateTime.Now;
                    _VacancyRange.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.VacancyRangeId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateVacancyRange", "VacancyRangeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindVacancyRangeDropDown(int? organizationID)
        {
            try
            {
                var _VacancyRange = await _context.VacancyRanges.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListVacancyRange = new List<SelectListItem>();
                _ListVacancyRange.AddRange(_VacancyRange.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.VacancyRangeId.ToString() }).ToList());
                return _ListVacancyRange;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindVacancyRangeDropDown", "VacancyRangeService.cs");
                throw;
            }
        }
    }
}
