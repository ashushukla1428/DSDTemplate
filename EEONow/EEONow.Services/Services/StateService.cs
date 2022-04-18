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
    public class StateService : IStatesService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public StateService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<ResponseModel> CreateState(StateModel model)
        {
            try
            {
                var State = await _repository.FindAsync<State>(x => x.Name.ToLower() == model.Name.ToLower());
                if (State != null)
                {
                    return new ResponseModel { Message = "State is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                State StateToInsert = new State
                {
                    Name = model.Name,
                    Active = model.Active,
                    Description = model.Description,
                    CreateUserId = _user,
                    UpdateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now

                };
                //save in database
                int StateId = await _repository.Insert<State>(StateToInsert);
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = StateToInsert.StateId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateState", "StateService.cs");
                throw;
            }
        }
        public async Task<List<StateModel>> GetStates()
        {
            try
            {
                var _State = await _repository.GetAllAsync<State>();
                List<StateModel> _lstModel = new List<StateModel>();
                _lstModel.AddRange(_State.Select(g => new StateModel { Name = g.Name.ToString(), StateId = g.StateId, Active = g.Active, Description = g.Description }).ToList());
                return _lstModel;

            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetStates", "StateService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindStateDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _State = await _repository.GetAllAsync<State>();
                var _ListState = new List<SelectListItem>();
                _ListState.AddRange(_State.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Description.ToString(), Value = g.StateId.ToString() }).OrderBy(e=>e.Text).ToList());
                return _ListState;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindStateDropDown", "StateService.cs");
                throw;
            }
        }

        public async Task<StateModel> GetStatesById(int Id)
        {
            try
            {
                var _State = await _repository.FindAsync<State>(x => x.StateId == Id);
                StateModel _Model = new StateModel
                { Name = _State.Name.ToString(), StateId = _State.StateId, Active = _State.Active, Description = _State.Description };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetStatesById", "StateService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateState(StateModel model)
        {
            try
            {
                var State = await _repository.FindAsync<State>(x => x.StateId == model.StateId);
                if (State != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    State.Name = model.Name;
                    State.Active = model.Active;
                    State.Description = model.Description;
                    State.UpdateUserId = _user;
                    State.UpdateDateTime = DateTime.Now;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.StateId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateState", "StateService.cs");
                throw;
            }
        }
    }
}
