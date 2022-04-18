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
    public class EEOJobCategoryService : IEEOJobCategoryService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEOJobCategoryService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<EEOJobCategoriesModel>> GetEEOJobCategoryModel()
        {
            var _JobCategory = await _context.EEOJobCategories.ToListAsync();
            List<EEOJobCategoriesModel> _lstModel = new List<EEOJobCategoriesModel>();
            try
            {
                _lstModel.AddRange(_JobCategory.Select(g => new EEOJobCategoriesModel
                {
                    Name = g.Name,
                    Description = g.Description,
                    DisplayColorCode = g.DisplayColorCode,
                    Active = g.Active,
                    EEOJobCategoryId = g.EEOJobCategoryId,
                    EEOJobCategoryNumber = g.EEOJobCategoryNumber,
                    OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                    OrganizationName = g.Organization == null ? "" : g.Organization.Name
                }).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOJobCategoryModel", "EEOJobCategoryService.cs");
                throw;
            }

            return _lstModel;
        }
        public async Task<ResponseModel> CreateEEOJobCategory(EEOJobCategoriesModel _model)
        {
            try
            {
                var JobCategory = await _repository.FindAsync<EEOJobCategory>(x => x.Name == _model.Name);

                if (JobCategory != null)
                {
                    return new ResponseModel { Message = "JobCategory is already exists.", Succeeded = false, Id = 0 };
                }

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                EEOJobCategory EEOJobCategoryToInsert = new EEOJobCategory
                {

                    Name = _model.Name,
                    Description = _model.Description,
                    DisplayColorCode = _model.DisplayColorCode,
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId),
                    EEOJobCategoryNumber = _model.EEOJobCategoryNumber,
                    Active = _model.Active,
                    EEOJobCategoryKey = "EEOC" + _model.EEOJobCategoryNumber,
                    CreateDateTime = DateTime.Now,
                    CreateUserId = _user,
                    UpdateDateTime = DateTime.Now,
                    UpdateUserId = _user
                };

                int EEOJobCategoryId = await _repository.Insert<EEOJobCategory>(EEOJobCategoryToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = EEOJobCategoryToInsert.EEOJobCategoryId };

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateEEOJobCategory", "EEOJobCategoryService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateEEOJobCategory(EEOJobCategoriesModel _model)
        {
            try
            {
                var _JobCategory = await _repository.FindAsync<EEOJobCategory>(x => x.EEOJobCategoryId == _model.EEOJobCategoryId);
                if (_JobCategory != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _JobCategory.Name = _model.Name;
                    _JobCategory.Description = _model.Description;
                    _JobCategory.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _JobCategory.DisplayColorCode = _model.DisplayColorCode;
                    _JobCategory.EEOJobCategoryNumber = _model.EEOJobCategoryNumber;
                    _JobCategory.Active = _model.Active;
                    _JobCategory.UpdateDateTime = DateTime.Now;
                    _JobCategory.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.EEOJobCategoryId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateEEOJobCategory", "EEOJobCategoryService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindEEOJobCategoryDropDown(int? organizationID)
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _EEOJobCategory = await _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == (organizationID == null ? e.Organization.OrganizationId : organizationID) && e.Active == true).OrderBy(e => e.EEOJobCategoryNumber).ToListAsync();
                var _ListEEOJobCategory = new List<SelectListItem>();
                _ListEEOJobCategory.AddRange(_EEOJobCategory.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.EEOJobCategoryId.ToString() }).ToList());
                return _ListEEOJobCategory;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEEOJobCategoryDropDown", "EEOJobCategoryService.cs");
                throw;
            }
        }
    }
}
