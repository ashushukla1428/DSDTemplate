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

namespace EEONow.Services
{
    public class CountyService : ICountiesService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public CountyService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public async Task<ResponseModel> CreateCounty(CountyModel model)
        {
            try
            {
                var County = await _repository.FindAsync<County>(x => x.Name.ToLower() == model.Name.ToLower());
                if (County != null)
                {
                    return new ResponseModel { Message = "County is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                County CountyToInsert = new County
                {
                    Code = model.Code,
                    Name = model.Name,
                    Active = model.Active,
                    Description = model.Description,
                    CreateUserId = _user,
                    UpdateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now

                };
                //save in database
                int CountyId = await _repository.Insert<County>(CountyToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = CountyToInsert.CountyId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateCounty", "CountyService.cs");
                throw;
            }
        }
        public async Task<List<CountyModel>> GetCounties()
        {
            try
            {
                var _County = await _repository.GetAllAsync<County>();
                List<CountyModel> _lstModel = new List<CountyModel>();
                _lstModel.AddRange(_County.Select(g => new CountyModel { Code = g.Code, Name = g.Name.ToString(), CountyId = g.CountyId, Active = g.Active, Description = g.Description }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetCounties", "CountyService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindCountyDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _County = await _repository.GetAllAsync<County>();
                var _ListCounty = new List<SelectListItem>();
                _ListCounty.AddRange(_County.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.CountyId.ToString() }).ToList());
                return _ListCounty;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindCountyDropDown", "CountyService.cs");
                throw;
            }
        }

        public async Task<CountyModel> GetCountiesById(int Id)
        {
            try
            {
                var _County = await _repository.FindAsync<County>(x => x.CountyId == Id);
                CountyModel _Model = new CountyModel
                { Name = _County.Name.ToString(), CountyId = _County.CountyId, Active = _County.Active, Description = _County.Description };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetCountiesById", "CountyService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateCounty(CountyModel model)
        {
            try
            {
                var County = await _repository.FindAsync<County>(x => x.CountyId == model.CountyId);
                if (County != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    County.Name = model.Name;
                    County.Active = model.Active;
                    County.Description = model.Description;
                    County.UpdateUserId = _user;
                    County.UpdateDateTime = DateTime.Now;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.CountyId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateCounty", "CountyService.cs");
                throw;
            }
        }
    }
}
