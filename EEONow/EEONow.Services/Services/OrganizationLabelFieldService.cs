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
    public class OrganizationLabelFieldService : IOrganizationLabelFieldService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public OrganizationLabelFieldService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<OrganizationLabelFieldModel>> GetOrganizationLabelFieldModel(int? organization, int? roleid)
        {
            var _OrganizationLabelField = await _context.OrganizationLabelFields.Where(e=>e.Organization.OrganizationId== organization && e.UserRole.RoleId== roleid).ToListAsync();
            List<OrganizationLabelFieldModel> _lstModel = new List<OrganizationLabelFieldModel>();
            try
            {
                _lstModel.AddRange(_OrganizationLabelField.Select(g => new OrganizationLabelFieldModel
                { 
                    OrganizationLabelFieldId = g.OrganizationLabelFieldId, 
                    Active=g.Active,
                    LabelKey=g.DefaultLabelField.LabelKey,
                    RoleId=g.UserRole.RoleId,
                    RoleName=g.UserRole.Name,
                    LabelName=g.DisplayLabelData,
                    OrganizationId=g.Organization.OrganizationId,
                    OrganizationName=g.Organization.Name
                }).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOrganizationLabelFieldModel", "OrganizationLabelFieldService.cs");
                throw;
            }

            return _lstModel;
        } 
        public async Task<ResponseModel> UpdateOrganizationLabelField(OrganizationLabelFieldModel _model)
        {
            try
            {
                var _OrganizationLabelField = await _repository.FindAsync<OrganizationLabelField>(x => x.OrganizationLabelFieldId == _model.OrganizationLabelFieldId);
                if (_OrganizationLabelField != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    _OrganizationLabelField.Active = _model.Active;
                    _OrganizationLabelField.DisplayLabelData = _model.LabelName;
                    _OrganizationLabelField.UpdateDateTime = DateTime.Now;
                    _OrganizationLabelField.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.OrganizationLabelFieldId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateOrganizationLabelField", "OrganizationLabelFieldService.cs");
                throw;
            }
        }
    
    }
}
