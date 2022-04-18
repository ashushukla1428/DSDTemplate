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
    public class PositionYearsOfServiceService : IPositionYearsOfService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public PositionYearsOfServiceService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<PositionYearsOfServiceModel>> GetPositionYearsOfServiceModel()
        {
            try
            {
                var _PositionYears = await _context.PositionYearsOfServices.ToListAsync();
                List<PositionYearsOfServiceModel> _lstModel = new List<PositionYearsOfServiceModel>();
                _lstModel.AddRange(_PositionYears.Select(g => new PositionYearsOfServiceModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    PositionYearsOfServiceId = g.PositionYearsOfServiceId,
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
                AppUtility.LogMessage(ex, "GetPositionYearsOfServiceModel", "PositionYearsOfServiceService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreatePositionYearsOfService(PositionYearsOfServiceModel _model)
        {
            try
            {
                var PositionYearsOfService = await _repository.FindAsync<PositionYearsOfService>(x => x.Name == _model.Name);

                if (PositionYearsOfService != null)
                {
                    return new ResponseModel { Message = "Position Years Of Service Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                PositionYearsOfService PositionYearsOfServiceToInsert = new PositionYearsOfService
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    PositionYearsOfServiceKey = "PYOS" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int PositionYearsOfServiceId = await _repository.Insert<PositionYearsOfService>(PositionYearsOfServiceToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = PositionYearsOfServiceToInsert.PositionYearsOfServiceId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreatePositionYearsOfService", "PositionYearsOfServiceService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdatePositionYearsOfService(PositionYearsOfServiceModel _model)
        {
            try
            {
                var _PositionYears = await _repository.FindAsync<PositionYearsOfService>(x => x.PositionYearsOfServiceId == _model.PositionYearsOfServiceId);
                if (_PositionYears != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _PositionYears.Name = _model.Name;
                    _PositionYears.Description = _model.Description;
                    _PositionYears.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _PositionYears.DisplayColorCode = _model.DisplayColorCode;
                    _PositionYears.Number = _model.Number;
                    _PositionYears.Active = _model.Active;
                    _PositionYears.MaxValue = _model.MaxValue;
                    _PositionYears.MinValue = _model.MinValue;
                    _PositionYears.UpdateDateTime = DateTime.Now;
                    _PositionYears.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.PositionYearsOfServiceId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdatePositionYearsOfService", "PositionYearsOfServiceService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindPositionYearsOfServiceDropDown(int? organizationID)
        {
            try
            {
                var _PositionYears = await _context.PositionYearsOfServices.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListPositionYears = new List<SelectListItem>();
                _ListPositionYears.AddRange(_PositionYears.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.PositionYearsOfServiceId.ToString() }).ToList());
                return _ListPositionYears;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindPositionYearsOfServiceDropDown", "PositionYearsOfServiceService.cs");
                throw;
            }
        }
    }
}
