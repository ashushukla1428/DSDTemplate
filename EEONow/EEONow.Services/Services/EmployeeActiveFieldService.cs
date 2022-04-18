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
    public class EmployeeActiveFieldService : IEmployeeActiveFieldService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EmployeeActiveFieldService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<EmployeeActiveFieldModel>> GetEmployeeActiveFieldModel(int? organization, int? roleid)
        {
            var _EmployeeActiveField = await _context.EmployeeActiveFields.Where(e=>e.Organization.OrganizationId== organization && e.UserRole.RoleId== roleid).OrderBy(e=>e.Sorting).ToListAsync();
            List<EmployeeActiveFieldModel> _lstModel = new List<EmployeeActiveFieldModel>();
            try
            {
                _lstModel.AddRange(_EmployeeActiveField.Select(g => new EmployeeActiveFieldModel
                { 
                    EmployeeActiveFieldId = g.EmployeeActiveFieldId, 
                    Active=g.Active,
                    ColumnKey=g.DefaultEmployeeField.ColumnCode,
                    RoleId=g.UserRole.RoleId,
                    RoleName=g.UserRole.Name,
                    DisplayLabelData=g.DisplayLabelData,
                    OrganizationId=g.Organization.OrganizationId,
                    OrganizationName=g.Organization.Name
                }).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEmployeeActiveFieldModel", "EmployeeActiveFieldService.cs");
                throw;
            }

            return _lstModel;
        } 
        public async Task<ResponseModel> UpdateEmployeeActiveField(EmployeeActiveFieldModel _model)
        {
            try
            {
                var _EmployeeActiveField = await _repository.FindAsync<EmployeeActiveField>(x => x.EmployeeActiveFieldId == _model.EmployeeActiveFieldId);
                if (_EmployeeActiveField != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    _EmployeeActiveField.Active = _model.Active;
                     
                    _EmployeeActiveField.UpdateDateTime = DateTime.Now;
                    _EmployeeActiveField.UpdateUserId = _user;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.EmployeeActiveFieldId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateEmployeeActiveField", "EmployeeActiveFieldService.cs");
                throw;
            }
        }
    
    }
}
