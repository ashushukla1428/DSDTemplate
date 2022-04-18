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
    public class EEORatingService : IEEORating
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEORatingService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public async Task<List<EEORatingModel>> GetEEORatingModel()
        {
            try
            {
                var _EEORating = await _context.EEORatings.ToListAsync();
                List<EEORatingModel> _lstModel = new List<EEORatingModel>();
                _lstModel.AddRange(_EEORating.Select(g => new EEORatingModel
                {
                    EEORatingTypeId = g.EEORatingType.EEORatingTypeId,
                    EEORatingTypeName = g.EEORatingType.Name,
                    GenderAndRaceColorCode = g.GenderAndRaceColorCode,
                    GenderColorCode = g.GenderColorCode,
                    NonSupervisorColorCode = g.NonSupervisorColorCode,
                    NonSupervisorLabelDisplay = g.NonSupervisorLabelDisplay,
                    Active = g.Active,
                    EEORatingId = g.EEORatingId,
                    GenderValueIndicator = g.GenderValueIndicator,
                    RaceColorCode = g.RaceColorCode,
                    RaceLabelDisplay = g.RaceLabelDisplay,
                    RaceValueIndicator = g.RaceValueIndicator,
                    GenderLabelDisplay = g.GenderLabelDisplay,
                    GenderAndRaceLabelDisplay=g.GenderAndRaceLabelDisplay,
                    Remarks=g.Remarks,
                    OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                    OrganizationName = g.Organization == null ? "" : g.Organization.Name
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEORatingModel", "EEORatingService.cs");
                throw;
            }


        }

        public async Task<ResponseModel> CreateEEORating(EEORatingModel _model)
        {
            try
            {
                var EEORating = await _repository.FindAsync<EEORating>(x => x.Organization.OrganizationId == _model.OrganizationId && x.Active == true);

                if (EEORating != null)
                {
                    return new ResponseModel { Message = "EEO Rating Range for this Organization is already exists and active,.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                EEORating EEORatingToInsert = new EEORating
                {
                    EEORatingType = await _repository.FindAsync<EEORatingType>(x => x.EEORatingTypeId == _model.EEORatingTypeId),
                    GenderAndRaceColorCode = _model.GenderAndRaceColorCode,
                    GenderLabelDisplay = _model.GenderLabelDisplay,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    GenderColorCode = _model.GenderColorCode,
                    GenderValueIndicator = _model.GenderValueIndicator,
                    NonSupervisorLabelDisplay = _model.NonSupervisorLabelDisplay,
                    Active = _model.Active,
                    RaceValueIndicator = _model.RaceValueIndicator,
                    RaceColorCode = _model.RaceColorCode,
                    RaceLabelDisplay = _model.RaceLabelDisplay,
                    NonSupervisorColorCode = _model.NonSupervisorColorCode,
                    GenderAndRaceLabelDisplay=_model.GenderAndRaceLabelDisplay,
                    Remarks=_model.Remarks,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int EEORatingId = await _repository.Insert<EEORating>(EEORatingToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = EEORatingToInsert.EEORatingId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateEEORating", "EEORatingService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateEEORating(EEORatingModel _model)
        {
            try
            {
                var _EEORating = await _repository.FindAsync<EEORating>(x => x.EEORatingId == _model.EEORatingId);
                if (_EEORating != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _EEORating.EEORatingType = await _repository.FindAsync<EEORatingType>(x => x.EEORatingTypeId == _model.EEORatingTypeId);
                    _EEORating.GenderAndRaceColorCode = _model.GenderAndRaceColorCode;
                    _EEORating.GenderLabelDisplay = _model.GenderLabelDisplay;
                    _EEORating.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _EEORating.GenderColorCode = _model.GenderColorCode;
                    _EEORating.GenderValueIndicator = _model.GenderValueIndicator;
                    _EEORating.NonSupervisorLabelDisplay = _model.NonSupervisorLabelDisplay;
                    _EEORating.Active = _model.Active;
                    _EEORating.RaceValueIndicator = _model.RaceValueIndicator;
                    _EEORating.RaceColorCode = _model.RaceColorCode;
                    _EEORating.RaceLabelDisplay = _model.RaceLabelDisplay;
                    _EEORating.NonSupervisorColorCode = _model.NonSupervisorColorCode;
                    _EEORating.GenderAndRaceLabelDisplay = _model.GenderAndRaceLabelDisplay;
                    _EEORating.Remarks = _model.Remarks;
                    _EEORating.UpdateDateTime = DateTime.Now;
                    _EEORating.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.EEORatingId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateEEORating", "EEORatingService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindEEORatingDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _EEORating = await _repository.GetAllAsync<EEORating>();
                var _ListEEORating = new List<SelectListItem>();
                _ListEEORating.AddRange(_EEORating.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Organization.Name.ToString(), Value = g.EEORatingId.ToString() }).ToList());
                return _ListEEORating;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEEORatingDropDown", "EEORatingService.cs");
                throw;
            }
        }

        public decimal GetBenchMarkValue(int OrganizationId)
        {
            try
            {
                if (OrganizationId == 0)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    OrganizationId = _context.Users.Where(e => e.UserId == _user).Select(e => e.Organization.OrganizationId).FirstOrDefault();
                }
                var _EEORatingValue = _context.EEORatings.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).FirstOrDefault();

                decimal ResultValue =0;
                switch (_EEORatingValue.EEORatingType.EEORatingTypeId)
                {
                    case 1:
                        ResultValue = Convert.ToDecimal(_EEORatingValue.RaceValueIndicator);
                        break;
                    case 2:
                        ResultValue = Convert.ToDecimal(_EEORatingValue.GenderValueIndicator);
                        break;
                    case 3:
                        ResultValue = Convert.ToDecimal(_EEORatingValue.RaceValueIndicator);
                        break;
                    default:
                        break;
                }
                return ResultValue;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetBenchMarkValue", "EEORatingService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindEEORatingTypeDropDown()
        {
            try
            { 
                var _EEORatingType = await _repository.GetAllAsync<EEORatingType>();
                var _ListEEORatingType = new List<SelectListItem>();
                _ListEEORatingType.AddRange(_EEORatingType.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.EEORatingTypeId.ToString() }).ToList());
                return _ListEEORatingType;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEEORatingTypeDropDown", "EEORatingService.cs");
                throw;
            }
        }
    }
}
