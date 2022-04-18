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
    public class LastPerformanceRatingService : ILastPerformanceRating
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public LastPerformanceRatingService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<LastPerformanceRatingModel>> GetLastPerformanceRatingModel()
        {
            try
            {
                var _LastPerformanceRating = await _context.LastPerformanceRatings.ToListAsync();
                List<LastPerformanceRatingModel> _lstModel = new List<LastPerformanceRatingModel>();
                _lstModel.AddRange(_LastPerformanceRating.Select(g => new LastPerformanceRatingModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    LastPerformanceRatingId = g.LastPerformanceRatingId,
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
                AppUtility.LogMessage(ex, "GetLastPerformanceRatingModel", "LastPerformanceRatingService.cs");
                throw;
            }


        }
        public async Task<ResponseModel> CreateLastPerformanceRating(LastPerformanceRatingModel _model)
        {
            try
            {
                var LastPerformanceRating = await _repository.FindAsync<LastPerformanceRating>(x => x.Name == _model.Name);

                if (LastPerformanceRating != null)
                {
                    return new ResponseModel { Message = "Salary Range Name is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                LastPerformanceRating LastPerformanceRatingToInsert = new LastPerformanceRating
                {
                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    Number = _model.Number,
                    Active = _model.Active,
                    MaxValue = _model.MaxValue,
                    MinValue = _model.MinValue,
                    LastPerformanceRatingKey = "LPR" + _model.Number,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int LastPerformanceRatingId = await _repository.Insert<LastPerformanceRating>(LastPerformanceRatingToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = LastPerformanceRatingToInsert.LastPerformanceRatingId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateLastPerformanceRating", "LastPerformanceRatingService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateLastPerformanceRating(LastPerformanceRatingModel _model)
        {
            try
            {
                var _LastPerformanceRating = await _repository.FindAsync<LastPerformanceRating>(x => x.LastPerformanceRatingId == _model.LastPerformanceRatingId);
                if (_LastPerformanceRating != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _LastPerformanceRating.Name = _model.Name;
                    _LastPerformanceRating.Description = _model.Description;
                    _LastPerformanceRating.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _LastPerformanceRating.DisplayColorCode = _model.DisplayColorCode;
                    _LastPerformanceRating.Number = _model.Number;
                    _LastPerformanceRating.Active = _model.Active;
                    _LastPerformanceRating.MaxValue = _model.MaxValue;
                    _LastPerformanceRating.MinValue = _model.MinValue;
                    _LastPerformanceRating.UpdateDateTime = DateTime.Now;
                    _LastPerformanceRating.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.LastPerformanceRatingId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateLastPerformanceRating", "LastPerformanceRatingService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindLastPerformanceRatingDropDown(int? organizationID)
        {
            try
            {
                var _LastPerformanceRating = await _context.LastPerformanceRatings.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.Number).ToListAsync();
                var _ListLastPerformanceRating = new List<SelectListItem>();
                _ListLastPerformanceRating.AddRange(_LastPerformanceRating.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.LastPerformanceRatingId.ToString() }).ToList());
                return _ListLastPerformanceRating;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindLastPerformanceRatingDropDown", "LastPerformanceRatingService.cs");
                throw;
            }
        }
    }
}
