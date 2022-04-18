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
    public class RaceService : IRaceService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public RaceService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<RaceModel>> GetRaceModel()
        {
            var _Race = await _context.Races.ToListAsync();
            List<RaceModel> _lstModel = new List<RaceModel>();
            try
            {
                _lstModel.AddRange(_Race.Select(g => new RaceModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    RaceId = g.RaceId,

                    RaceNumber = g.RaceNumber,
                    OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                    OrganizationName = g.Organization == null ? "" : g.Organization.Name
                }).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetRaceModel", "RaceService.cs");
                throw;
            }

            return _lstModel;
        }
        public async Task<ResponseModel> CreateRace(RaceModel _model)
        {
            try
            {
                var Race = await _repository.FindAsync<Race>(x => x.Name == _model.Name);

                if (Race != null)
                {
                    return new ResponseModel { Message = "Race is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                Race RaceToInsert = new Race
                {

                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    RaceNumber = _model.RaceNumber,
                    Active = _model.Active,
                    RaceKey = "R" + _model.RaceNumber,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int RaceId = await _repository.Insert<Race>(RaceToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = RaceToInsert.RaceId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateRace", "RaceService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateRace(RaceModel _model)
        {
            try
            {
                var _Race = await _repository.FindAsync<Race>(x => x.RaceId == _model.RaceId);
                if (_Race != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _Race.Name = _model.Name;
                    _Race.Description = _model.Description;
                    _Race.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _Race.DisplayColorCode = _model.DisplayColorCode;
                    _Race.RaceNumber = _model.RaceNumber;
                    _Race.Active = _model.Active;
                    _Race.UpdateDateTime = DateTime.Now;
                    _Race.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.RaceId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateRace", "RaceService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindRaceDropDown(int? organizationID)
        {
            try
            {
                var _Race = await _context.Races.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.RaceNumber).ToListAsync();
                var _ListRace = new List<SelectListItem>();
                _ListRace.AddRange(_Race.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.RaceId.ToString() }).ToList());
                return _ListRace;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindRaceDropDown", "RaceService.cs");
                throw;
            }
        }
    }
}
