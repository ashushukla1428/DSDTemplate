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
    public class AgencyYearsOfServiceService : IAgencyYearsOfService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public AgencyYearsOfServiceService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<AgencyYearsOfServiceModel>> GetAgencyYearsOfServiceModel()
        {
            try
            {
                var _AgencyYears = await _context.AgencyYearsOfServices.ToListAsync();
                List<AgencyYearsOfServiceModel> _lstModel = new List<AgencyYearsOfServiceModel>();
                _lstModel.AddRange(_AgencyYears.Select(g => new AgencyYearsOfServiceModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    AgencyYearsOfServiceId = g.AgencyYearsOfServiceId,
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
                AppUtility.LogMessage(ex, "GetAgencyYearsOfServiceModel", "AgencyYearsOfServiceService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreateAgencyYearsOfService(AgencyYearsOfServiceModel _model)
        {
            try
            {
                var AgencyYearsOfService = await _repository.FindAsync<AgencyYearsOfService>(x => x.Name == _model.Name);

                if (AgencyYearsOfService != null)
                {
                    return new ResponseModel { Message = "Agency Years Of Service Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                AgencyYearsOfService AgencyYearsOfServiceToInsert = new AgencyYearsOfService
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    AgencyYearsOfServiceKey = "AYOS" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int AgencyYearsOfServiceId = await _repository.Insert<AgencyYearsOfService>(AgencyYearsOfServiceToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = AgencyYearsOfServiceToInsert.AgencyYearsOfServiceId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateAgencyYearsOfService", "AgencyYearsOfServiceService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateAgencyYearsOfService(AgencyYearsOfServiceModel _model)
        {
            try
            {
                var _AgencyYears = await _repository.FindAsync<AgencyYearsOfService>(x => x.AgencyYearsOfServiceId == _model.AgencyYearsOfServiceId);
                if (_AgencyYears != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _AgencyYears.Name = _model.Name;
                    _AgencyYears.Description = _model.Description;
                    _AgencyYears.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _AgencyYears.DisplayColorCode = _model.DisplayColorCode;
                    _AgencyYears.Number = _model.Number;
                    _AgencyYears.Active = _model.Active;
                    _AgencyYears.MaxValue = _model.MaxValue;
                    _AgencyYears.MinValue = _model.MinValue;
                    _AgencyYears.UpdateDateTime = DateTime.Now;
                    _AgencyYears.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.AgencyYearsOfServiceId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateAgencyYearsOfService", "AgencyYearsOfServiceService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindAgencyYearsOfServiceDropDown(int? organizationID)
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _AgencyYears = await _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListAgencyYears = new List<SelectListItem>();
                _ListAgencyYears.AddRange(_AgencyYears.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.AgencyYearsOfServiceId.ToString() }).ToList());
                return _ListAgencyYears;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindAgencyYearsOfServiceDropDown", "AgencyYearsOfServiceService.cs");
                throw;
            }
        }
    }
}
