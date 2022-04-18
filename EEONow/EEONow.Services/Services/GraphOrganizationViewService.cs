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
    public class GraphOrganizationViewService : IGraphOrganizationViewService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public GraphOrganizationViewService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public async Task<List<GraphOrganizationViewModel>> GetGraphOrganizationViewModel()
        {
            var _GraphOrganizationView = await _context.GraphOrganizationViews.ToListAsync();
            List<GraphOrganizationViewModel> _lstModel = new List<GraphOrganizationViewModel>();
            try
            {
                _lstModel.AddRange(_GraphOrganizationView.Select(g => new GraphOrganizationViewModel
                {
                    Name = g.Name,
                    Active = g.Active,
                    Organization = g.Organization.Name,
                    OrganizationId = g.Organization.OrganizationId,
                    GraphOrganizationViewId = g.GraphOrganizationViewId,
                    Order = g.OrderNo
                }).ToList());
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetGraphOrganizationViewModel", "GraphOrganizationViewService.cs");
                throw;
            }

            return _lstModel;
        }
        public async Task<ResponseModel> UpdateGraphOrganizationView(GraphOrganizationViewModel _model)
        {
            try
            {
                var _GraphOrganizationView = await _repository.FindAsync<GraphOrganizationView>(x => x.GraphOrganizationViewId == _model.GraphOrganizationViewId && x.Organization.OrganizationId == _model.OrganizationId);
                if (_GraphOrganizationView != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    _GraphOrganizationView.Name = _model.Name;
                    _GraphOrganizationView.Active = _model.Active;
                    _GraphOrganizationView.OrderNo = _model.Order;
                    _GraphOrganizationView.UpdateDateTime = DateTime.Now;
                    _GraphOrganizationView.UpdateUserId = _user;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.GraphOrganizationViewId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateGraphOrganizationView", "GraphOrganizationViewService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindGraphOrganizationViewDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _GraphOrganizationView = await _context.GraphOrganizationViews.ToListAsync();
                AccountService _AccountService = new AccountService();
                var OrganisationId = _AccountService.GetOrganisationID();
                if (OrganisationId > 0)
                {
                    _GraphOrganizationView = _GraphOrganizationView.Where(e => e.Organization.OrganizationId == OrganisationId).ToList();
                }
                var _ListGraphOrganizationView = new List<SelectListItem>();
                _ListGraphOrganizationView.AddRange(_GraphOrganizationView.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.GraphOrganizationViewId.ToString() }).ToList());
                return _ListGraphOrganizationView;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindGraphOrganizationViewDropDown", "GraphOrganizationViewService.cs");
                throw;
            }
        }
        public List<SelectListItem> BindGraphOrganizationViewViaOrganisationIdDropDown(int OrganisationId)
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _GraphOrganizationView = _context.GraphOrganizationViews.ToList();
                AccountService _AccountService = new AccountService();
                if (OrganisationId > 0)
                {
                    _GraphOrganizationView = _GraphOrganizationView.Where(e => e.Organization.OrganizationId == OrganisationId).ToList();
                }
                else
                {
                    var _OrganisationId = _AccountService.GetOrganisationID();
                    if (_OrganisationId > 0)
                    {
                        _GraphOrganizationView = _GraphOrganizationView.Where(e => e.Organization.OrganizationId == _OrganisationId).ToList();
                    }

                }
                var _ListGraphOrganizationView = new List<SelectListItem>();
                _ListGraphOrganizationView.AddRange(_GraphOrganizationView.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.GraphOrganizationViewId.ToString() }).ToList());
                return _ListGraphOrganizationView;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindGraphOrganizationViewDropDown", "GraphOrganizationViewService.cs");
                throw;
            }
        }
        public String GraphOrganizationViewList(Int32 organisationId = 0, Int32 roleId = 0)
        {
            try
            {
                Int32 _organisationId = organisationId;
                Int32 _roleId = roleId;
                if (organisationId == 0)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    var _UserRole = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole).FirstOrDefault();
                    _organisationId = _UserRole.Organization.OrganizationId;
                    _roleId = _UserRole.RoleId;
                }

                // var _GetGraphLevelId = _context.GraphLevels.Where(e => e.Code == GraphLevel).Select(e => e.GraphLevelId).FirstOrDefault();
                //var _List = _context.AssignedGraphOrganizationViews
                //            .Where(e => e.GraphLevel.GraphLevelId == _GetGraphLevelId && e.Organization.OrganizationId == _organisationId && e.UserRole.RoleId == _roleId && e.GraphOrganizationView.Active == true).OrderBy(e => e.GraphOrganizationView.OrderNo).Select(e => e.GraphOrganizationView.Name).ToList();
                var _List = _context.AssignedGraphOrganizationViews
                           .Where(e => e.Organization.OrganizationId == _organisationId && e.UserRole.RoleId == _roleId && e.GraphOrganizationView.Active == true).OrderBy(e => e.GraphOrganizationView.OrderNo).Select(e => e.GraphOrganizationView.Name).ToList();

                return string.Join(",", _List.Select(e => e));
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GraphOrganizationViewList", "GraphOrganizationViewService.cs");
                throw;
            }
        }
        public async Task<List<AssigGraphOrganizationViewModel>> GetAssignedGraphOrganizationViewModel(int? organization, int? roleid)
        {
            try
            {
                //var _GraphLevels = await _context.GraphLevels.ToListAsync();
                var _AssignedGraphOrganizationViews = await _context.AssignedGraphOrganizationViews.ToListAsync();
                var _GraphOrganizationViews = _context.GraphOrganizationViews.Where(e => e.Active == true);
                List<AssigGraphOrganizationViewModel> _lstmodel = new List<AssigGraphOrganizationViewModel>();
                //var _roles = await _context.UserRoles.Where(e=>e.RoleId== roleid).ToListAsync();
                var _organisation = await _context.Organizations.ToListAsync();

                if (organization != null && organization > 0)
                {
                    _organisation = await _context.Organizations.Where(e => e.OrganizationId == organization).ToListAsync();
                }
                foreach (var _itemOrg in _organisation)
                {
                    if (roleid != null && roleid > 0)
                    {
                        _itemOrg.UserRoles = _itemOrg.UserRoles.Where(e => e.RoleId == roleid).ToList();
                    }
                    foreach (var _itemRoles in _itemOrg.UserRoles)
                    {
                        // foreach (var item in _GraphLevels)
                        //{
                        AssigGraphOrganizationViewModel _model = new AssigGraphOrganizationViewModel
                        {
                            GraphLevelId = 1,//item.GraphLevelId,
                            GraphLevelName = "Dashboard",//item.Name,
                            OrganizationId = _itemOrg.OrganizationId,
                            Organization = _itemOrg.Name,
                            RoleId = _itemRoles.RoleId,
                            RoleName = _itemRoles.Name,
                            GraphOrganizationViewId = _AssignedGraphOrganizationViews.Where(e => e.Organization.OrganizationId == _itemOrg.OrganizationId && e.UserRole.RoleId == _itemRoles.RoleId).Select(e => e.GraphOrganizationView.GraphOrganizationViewId).ToList(),
                            ListGraphOrganizationView = _AssignedGraphOrganizationViews.Where(e => e.Organization.OrganizationId == _itemOrg.OrganizationId && e.UserRole.RoleId == _itemRoles.RoleId).Select(e => new SelectListItem { Text = e.GraphOrganizationView.Name, Value = e.GraphOrganizationView.GraphOrganizationViewId.ToString() }).ToList()
                            //GraphOrganizationViewId = _AssignedGraphOrganizationViews.Where(e => e.GraphLevel.GraphLevelId == item.GraphLevelId && e.Organization.OrganizationId == _itemOrg.OrganizationId && e.UserRole.RoleId == _itemRoles.RoleId).Select(e => e.GraphOrganizationView.GraphOrganizationViewId).ToList(),
                            //ListGraphOrganizationView = _AssignedGraphOrganizationViews.Where(e => e.GraphLevel.GraphLevelId == item.GraphLevelId && e.Organization.OrganizationId == _itemOrg.OrganizationId && e.UserRole.RoleId == _itemRoles.RoleId).Select(e => new SelectListItem { Text = e.GraphOrganizationView.Name, Value = e.GraphOrganizationView.GraphOrganizationViewId.ToString() }).ToList()
                        };
                        _lstmodel.Add(_model);
                        //}
                    }
                }
                return _lstmodel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAssignedGraphOrganizationViewModel", "GraphOrganizationViewService.cs");
                throw;
            }
        }
        public ResponseModel UpdateAssignedGraphOrganizationView(AssigGraphOrganizationViewModel _model)
        {
            try
            {
                // var ExistAssignedGraphOrganizationView = _context.AssignedGraphOrganizationViews.Where(x => x.GraphLevel.GraphLevelId == _model.GraphLevelId && x.Organization.OrganizationId == _model.OrganizationId && x.UserRole.RoleId == _model.RoleId).ToList();

                var ExistAssignedGraphOrganizationView = _context.AssignedGraphOrganizationViews.Where(x =>x.Organization.OrganizationId == _model.OrganizationId && x.UserRole.RoleId == _model.RoleId).ToList();

                if (ExistAssignedGraphOrganizationView != null && ExistAssignedGraphOrganizationView.Count() > 0)
                {
                    _context.AssignedGraphOrganizationViews.RemoveRange(ExistAssignedGraphOrganizationView);
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                List<AssignedGraphOrganizationView> _lstAssignedGraphOrganizationView = new List<AssignedGraphOrganizationView>();
                foreach (var item in _model.ListGraphOrganizationView)
                {
                    int GraphOrganizationViewId = Convert.ToInt32(item.Value);
                    AssignedGraphOrganizationView AssignedGraphOrganizationViewToInsert = new AssignedGraphOrganizationView
                    {
                        GraphOrganizationView = _context.GraphOrganizationViews.Where(e => e.GraphOrganizationViewId == GraphOrganizationViewId && e.Organization.OrganizationId == _model.OrganizationId).FirstOrDefault(),
                        //GraphLevel = _context.GraphLevels.Where(e => e.GraphLevelId == _model.GraphLevelId).FirstOrDefault(),
                        UserRole = _context.UserRoles.Where(e => e.RoleId == _model.RoleId).FirstOrDefault(),
                        CreateUserId = _user,
                        Organization = _context.Organizations.Where(e => e.OrganizationId == _model.OrganizationId).FirstOrDefault(),
                        UpdateUserId = _user,
                        CreateDateTime = DateTime.Now,
                        UpdateDateTime = DateTime.Now

                    };
                    _lstAssignedGraphOrganizationView.Add(AssignedGraphOrganizationViewToInsert);
                }
                //save in database
                var AssignedGraphOrganizationViewId = _context.AssignedGraphOrganizationViews.AddRange(_lstAssignedGraphOrganizationView);
                _context.SaveChanges();
                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = 1 };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateAssignedGraphOrganizationView", "GraphOrganizationViewService.cs");
                throw;
            }
        }
    }
}
