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
    public class OrganizationService : IOrganizationsService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public OrganizationService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public async Task<ResponseModel> CreateOrganization(OrganizationModel model)
        {
            try
            {
                var Organization = await _repository.FindAsync<Organization>(x => x.Name.ToLower() == model.Name.ToLower());
                if (Organization != null)
                {
                    return new ResponseModel { Message = "Organization is already exists.", Succeeded = false, Id = 0 };
                }
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                int? ParentOrganizationID = null;
                if (model.ParentOrganizationId > 0)
                {
                    ParentOrganizationID = model.ParentOrganizationId;
                }
                Organization OrganizationToInsert = new Organization
                {
                    Name = model.Name,
                    Address = model.Address,
                    City = model.City,
                    OrgCode = model.OrgCode,
                    ZipCode = model.ZipCode,
                    State = await _repository.FindAsync<State>(x => x.StateId == model.StateId),
                    Active = model.Active,
                    StatesALMDefault = await _repository.FindAsync<State>(x => x.StateId == model.DefaultStateId),
                    USCensus_EEO_OccupationCodes = await _repository.FindAsync<USCensus_EEO_OccupationCodes>(x => x.EEOOccupationCodeID == model.DefaultEEOOccupationalCodeID),
                    ParentOrganizationID = ParentOrganizationID,
                    EnableTwoFactorAuthentication = model.EnableTwoFactorAuthentication,
                    //VacanciesDisplayColorCode = model.VacanciesDisplayColorCode,
                    Description = model.Description,
                    CreateUserId = _user,
                    UpdateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now

                };
                //save in database
                await _repository.Insert<Organization>(OrganizationToInsert);

                // Start insert PublicURL Data
                PublicURL PublicURLInsert = new PublicURL
                {
                    Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == OrganizationToInsert.OrganizationId),
                    Token = Guid.NewGuid().ToString(),
                    Active = false,
                    CreateUserId = _user,
                    UpdateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now
                };
                int PublicURLId = await _repository.Insert<PublicURL>(PublicURLInsert);
                // End insert PublicURL Data

                //Start GraphOrganizationView
                /// <summary>
                /// This is the method to add "Dashboard Views" in the dropdown menu control for "View Organization Data:"
                /// See UI screen "/GraphOrganizationView/AssignGraphOrganizationView" for more details on how to setup
                /// for each user role.
                /// </summary>
                /// <param name="OrganizationId"></param>
                /// <returns></returns>
                await AddGraphOrganizationView(Organization.OrganizationId);
                //End Add Graph OrganizationView

                // Start insert OrganizationLabelField data 
                /// <summary>
                /// This is the method to add "Dashboard Data" control for users to see based on the User Role.
                /// We can then set "Active" or "Inactive" via the application the fields and panels that we don't want
                /// to show to certain user roles.
                /// See UI screen "/OrganizationLabelField/Index" for more details on how to setup for each user role.
                /// </summary>
                /// <param name="OrganizationId"></param>
                /// <returns></returns>
                await AddOrganizationLabelField(Organization.OrganizationId);
                // End insert OrganizationLabelField data

                // Start insert EmployeeActiveFields data 
                /// <summary>
                /// This is the method to add "Employee Data Fields" to view or download. This method will add the default values for each user role.
                /// We can then set "Active" or "Inactive" via the application the fields that we want to allow user roles to see or download.
                /// See UI screen "/EmployeeActiveField/index" for more details on how to setup for each user role.
                /// </summary>
                /// <param name="OrganizationId"></param>
                /// <returns></returns>
                await AddEmployeeActiveFields(Organization.OrganizationId);
                // End insert EmployeeActiveFields data


                return new ResponseModel { Message = "Data successfully saved", Succeeded = true, Id = OrganizationToInsert.OrganizationId };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CreateOrganization", "OrganizationService.cs");
                throw;
            }
        }
        public async Task<List<OrganizationModel>> GetOrganization()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var users = await _context.Users.Where(e => e.UserId == _user).FirstOrDefaultAsync();


                var _Organization = await _context.Organizations.ToListAsync();

                if (users.UserRole != null)
                {
                    _Organization = _Organization.Where(e => e.ParentOrganizationID == users.UserRole.Organization.OrganizationId || e.OrganizationId == users.UserRole.Organization.OrganizationId).ToList();
                }

                List<OrganizationModel> _lstModel = new List<OrganizationModel>();
                //var _States = await _repository.GetAllAsync<State>();
                //var _Liststate = new List<SelectListItem>();
                //_Liststate.AddRange(_Organization.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList()); 
                _lstModel.AddRange(_Organization.Select(g => new OrganizationModel
                {
                    Name = g.Name.ToString(),
                    OrganizationId = g.OrganizationId,
                    Active = g.Active,
                    ParentOrganizationId = g.ParentOrganizationID == null ? 0 : Convert.ToInt32(g.ParentOrganizationID),
                    //VacanciesDisplayColorCode = g.VacanciesDisplayColorCode,
                    Description = g.Description,
                    Address = g.Address,
                    City = g.City,
                    OrgCode = g.OrgCode,
                    StateId = g.State.StateId,
                    DefaultEEOOccupationalCodeID = g.USCensus_EEO_OccupationCodes.EEOOccupationCodeID,
                    EnableTwoFactorAuthentication = g.EnableTwoFactorAuthentication,
                    DefaultStateId = g.StatesALMDefault.StateId,
                    ZipCode = g.ZipCode
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOrganization", "OrganizationService.cs");
                throw;
            }
        }

        public async Task<OrganizationModel> GetOrganizationsById(int Id)
        {
            try
            {
                var _Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == Id);
                OrganizationModel _Model = new OrganizationModel
                { Name = _Organization.Name.ToString(), OrganizationId = _Organization.OrganizationId, Active = _Organization.Active, Description = _Organization.Description };
                return _Model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOrganizationsById", "OrganizationService.cs");
                throw;
            }
        }
        public Int32 GetFirstOrganizationsId()
        {
            try
            {
                var _Organization = _context.Organizations.FirstOrDefault();
                return _Organization.OrganizationId;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFirstOrganizationsId", "OrganizationService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateOrganization(OrganizationModel model)
        {
            try
            {
                var Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == model.OrganizationId);
                if (Organization != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    if (await _repository.FindAsync<PublicURL>(x => x.Organization.OrganizationId == Organization.OrganizationId) == null)
                    {
                        // Start insert PublicURL Data
                        PublicURL PublicURLInsert = new PublicURL
                        {
                            Organization = Organization,
                            Token = Guid.NewGuid().ToString(),
                            Active = false,
                            CreateUserId = _user,
                            UpdateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateDateTime = DateTime.Now
                        };
                        int PublicURLId = await _repository.Insert<PublicURL>(PublicURLInsert);
                        // End insert PublicURL Data
                    }
                    Organization.ParentOrganizationID = null;
                    if (model.ParentOrganizationId > 0)
                    {
                        Organization.ParentOrganizationID = model.ParentOrganizationId;
                    }
                    Organization.Name = model.Name;
                    Organization.Address = model.Address;
                    Organization.City = model.City;
                    Organization.OrgCode = model.OrgCode;
                    Organization.ZipCode = model.ZipCode;
                    Organization.State = await _repository.FindAsync<State>(x => x.StateId == model.StateId);

                    Organization.StatesALMDefault = await _repository.FindAsync<State>(x => x.StateId == model.DefaultStateId);
                    Organization.USCensus_EEO_OccupationCodes = await _repository.FindAsync<USCensus_EEO_OccupationCodes>(x => x.EEOOccupationCodeID == model.DefaultEEOOccupationalCodeID);
                    Organization.Active = model.Active;
                    Organization.EnableTwoFactorAuthentication = model.EnableTwoFactorAuthentication;
                    //Organization.VacanciesDisplayColorCode = model.VacanciesDisplayColorCode;
                    Organization.Description = model.Description;
                    Organization.UpdateUserId = _user;
                    await _repository.SaveChangesAsync();

                    //Start GraphOrganizationView
                    /// <summary>
                    /// This is the method to add "Dashboard Views" in the dropdown menu control for "View Organization Data:"
                    /// See UI screen "/GraphOrganizationView/AssignGraphOrganizationView" for more details on how to setup
                    /// for each user role.
                    /// </summary>
                    /// <param name="OrganizationId"></param>
                    /// <returns></returns>
                    await AddGraphOrganizationView(Organization.OrganizationId);
                    //End Add Graph OrganizationView

                    // Start insert OrganizationLabelField data 
                    /// <summary>
                    /// This is the method to add "Dashboard Data" control for users to see based on the User Role.
                    /// We can then set "Active" or "Inactive" via the application the fields and panels that we don't want
                    /// to show to certain user roles.
                    /// See UI screen "/OrganizationLabelField/Index" for more details on how to setup for each user role.
                    /// </summary>
                    /// <param name="OrganizationId"></param>
                    /// <returns></returns>
                    await AddOrganizationLabelField(Organization.OrganizationId);
                    // End insert OrganizationLabelField data

                    // Start insert EmployeeActiveFields data 
                    /// <summary>
                    /// This is the method to add "Employee Data Fields" to view or download. This method will add the default values for each user role.
                    /// We can then set "Active" or "Inactive" via the application the fields that we want to allow user roles to see or download.
                    /// See UI screen "/EmployeeActiveField/index" for more details on how to setup for each user role.
                    /// </summary>
                    /// <param name="OrganizationId"></param>
                    /// <returns></returns>
                    await AddEmployeeActiveFields(Organization.OrganizationId);
                    // End insert EmployeeActiveFields data

                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.OrganizationId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateOrganization", "OrganizationService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindOrganizationDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _Organization = await _repository.GetAllAsync<Organization>();
                var _ListOrganization = new List<SelectListItem>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    _ListOrganization.AddRange(_Organization.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                }
                else
                {
                    var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                    if (user != null)
                    {
                        _ListOrganization.AddRange(_Organization.Where(e => e.Active == true && e.OrganizationId == user.UserRole.Organization.OrganizationId).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                    }
                    else
                    {
                        _ListOrganization.AddRange(_Organization.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                    }
                }
                return _ListOrganization;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindOrganizationDropDown", "OrganizationService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> BindParentOrganizationDropDown()
        {
            try
            {
                RegisterModel model = new RegisterModel();
                var _Organization = await _repository.GetAllAsync<Organization>();
                var _ListOrganization = new List<SelectListItem>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                _ListOrganization.Add(new SelectListItem { Text = "Select Organization", Value = "0" });

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    _ListOrganization.AddRange(_Organization.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                }
                else
                {
                    var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                    if (user != null)
                    {
                        _ListOrganization.AddRange(_Organization.Where(e => e.Active == true && e.OrganizationId == user.UserRole.Organization.OrganizationId).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                    }
                    else
                    {
                        _ListOrganization.AddRange(_Organization.Where(e => e.Active == true).Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.OrganizationId.ToString() }).ToList());
                    }
                }

                return _ListOrganization.OrderBy(e => e.Value).ToList();
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindOrganizationDropDown", "OrganizationService.cs");
                throw;
            }
        }
        //

        public async Task<List<VacancyPositionColorModel>> GetVacancyPositionColor()
        {
            try
            {
                var _Organization = await _context.Organizations.ToListAsync();
                List<VacancyPositionColorModel> _lstModel = new List<VacancyPositionColorModel>();

                _lstModel.AddRange(_Organization.Select(g => new VacancyPositionColorModel
                {
                    Name = g.Name.ToString(),
                    OrganizationId = g.OrganizationId,
                    VacanciesDisplayColorCode = g.VacanciesDisplayColorCode,
                    NonVacanciesDisplayColorCode = g.NonVacanciesDisplayColorCode,
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetVacancyPositionColor", "OrganizationService.cs");
                throw;
            }
        }

        public async Task<ResponseModel> UpdateVacancyPositionColor(VacancyPositionColorModel model)
        {
            try
            {
                var Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == model.OrganizationId);
                if (Organization != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);
                    Organization.VacanciesDisplayColorCode = model.VacanciesDisplayColorCode;
                    Organization.NonVacanciesDisplayColorCode = model.NonVacanciesDisplayColorCode;

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = model.OrganizationId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateVacancyPositionColor", "OrganizationService.cs");
                throw;
            }
        }

        /// <summary>
        /// This is the method to add "Dashboard Views" in the dropdown menu control for "View Organization Data:"
        /// See UI screen "/GraphOrganizationView/AssignGraphOrganizationView" for more details on how to setup
        /// for each user role.
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>
        private async Task<bool> AddGraphOrganizationView(int OrganizationId)
        {
            try
            {
                var ExistingGraphOrganizationView = await _context.GraphOrganizationViews.Where(e => e.Organization.OrganizationId == OrganizationId).ToListAsync();
                if (ExistingGraphOrganizationView.Count() == 0)
                {
                    List<GraphOrganizationView> listGraphOrganizationView = new List<GraphOrganizationView>();
                    var listGraphOrganizationMaster = await _context.GraphOrganizationMasters.Where(e => e.Active).ToListAsync();
                    foreach (var itemGraphOrganization in listGraphOrganizationMaster)
                    {
                        GraphOrganizationView _InsertGraphOrganizationView = new GraphOrganizationView
                        {
                            Organization = await _context.Organizations.Where(e => e.OrganizationId == OrganizationId).FirstOrDefaultAsync(),
                            GraphOrganizationMaster = itemGraphOrganization,
                            Name = itemGraphOrganization.DefaultName,
                            ColorKey = itemGraphOrganization.ColorKey,
                            OrderNo = itemGraphOrganization.OrderNo,
                            Active = itemGraphOrganization.Active,
                            CreateUserId = itemGraphOrganization.CreateUserId,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = itemGraphOrganization.UpdateUserId,
                            UpdateDateTime = DateTime.Now
                        };
                        listGraphOrganizationView.Add(_InsertGraphOrganizationView);
                    }
                    _context.GraphOrganizationViews.AddRange(listGraphOrganizationView);
                    await _context.SaveChangesAsync();
                }
                return true;

            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// This is the method to add "Dashboard Data" control for users to see based on the User Role.
        /// We can then set "Active" or "Inactive" via the application the fields and panels that we don't want
        /// to show to certain user roles.
        /// See UI screen "/OrganizationLabelField/Index" for more details on how to setup for each user role.
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>
        private async Task<bool> AddOrganizationLabelField(int OrganizationId)
        {
            try
            {
                var ListOfRoles = await _context.UserRoles.Where(e => e.Organization.OrganizationId == OrganizationId).ToListAsync();
                var _ExistOrgLabel = await _context.OrganizationLabelFields.Where(e => e.Organization.OrganizationId == OrganizationId).ToListAsync();
                foreach (var ExistingRoles in ListOfRoles)
                {
                    if (_ExistOrgLabel.Where(e => e.UserRole.RoleId == ExistingRoles.RoleId).Count() == 0)
                    {
                        var _DefaultLabelField = await _context.DefaultLabelFields.ToListAsync();
                        List<OrganizationLabelField> listOrganizationLabelField = new List<OrganizationLabelField>();
                        foreach (var itemLabelField in _DefaultLabelField)
                        {
                            OrganizationLabelField insertOrganizationLabelField = new OrganizationLabelField
                            {
                                DefaultLabelField = itemLabelField,
                                DisplayLabelData = itemLabelField.DisplayLabelData,
                                Organization = await _context.Organizations.Where(e => e.OrganizationId == OrganizationId).FirstOrDefaultAsync(),
                                UserRole = ExistingRoles,
                                SectionId = 1,
                                PageId = 1,
                                Active = true,
                                CreateUserId = itemLabelField.CreateUserId,
                                UpdateUserId = itemLabelField.UpdateUserId,
                                CreateDateTime = DateTime.Now,
                                UpdateDateTime = DateTime.Now
                            };

                            listOrganizationLabelField.Add(insertOrganizationLabelField);
                        }
                        _context.OrganizationLabelFields.AddRange(listOrganizationLabelField);
                        await _context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// This is the method to add "Employee Data Fields" to view or download. This method will add the default values for each user role.
        /// We can then set "Active" or "Inactive" via the application the fields that we want to allow user roles to see or download.
        /// See UI screen "/EmployeeActiveField/index" for more details on how to setup for each user role.
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>
        private async Task<bool> AddEmployeeActiveFields(int OrganizationId)
        {
            try
            {
                var ListOfRoles = await _context.UserRoles.Where(e => e.Organization.OrganizationId == OrganizationId).ToListAsync();
                var _EmployeeActiveFields = await _context.EmployeeActiveFields.Where(e => e.Organization.OrganizationId == OrganizationId).ToListAsync();
                foreach (var ExistingRoles in ListOfRoles)
                {
                    if (_EmployeeActiveFields.Where(e => e.UserRole.RoleId == ExistingRoles.RoleId).Count() == 0)
                    {
                        var _DefaultEmployeeField = await _context.DefaultEmployeeFields.ToListAsync();
                        List<EmployeeActiveField> listEmployeeActiveField = new List<EmployeeActiveField>();
                        foreach (var itemEmployeeField in _DefaultEmployeeField)
                        {
                            EmployeeActiveField insertEmployeeActiveField = new EmployeeActiveField
                            {
                                DefaultEmployeeField = itemEmployeeField,
                                DisplayLabelData = itemEmployeeField.ColumnName,
                                Organization = await _context.Organizations.Where(e => e.OrganizationId == OrganizationId).FirstOrDefaultAsync(),
                                UserRole = ExistingRoles,
                                Active = true,
                                Sorting = itemEmployeeField.Sorting,
                                CreateUserId = itemEmployeeField.CreateUserId,
                                UpdateUserId = itemEmployeeField.UpdateUserId,
                                CreateDateTime = DateTime.Now,
                                UpdateDateTime = DateTime.Now
                            };

                            listEmployeeActiveField.Add(insertEmployeeActiveField);
                        }
                        _context.EmployeeActiveFields.AddRange(listEmployeeActiveField);
                        await _context.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<DefaultALMValue> GetDefaultALMValue()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                if (_Loginmodel.Roles == "DefinedSoftwareAdministrator")
                {
                    return new DefaultALMValue();
                }
                var result = await _context.Users.Where(e => e.UserId == _user && e.UserRole != null)
                            .Select(e => new DefaultALMValue { DefaultALMState = e.UserRole.Organization.StatesALMDefault.StateId, DefaultALMOccupationalCode = e.UserRole.Organization.USCensus_EEO_OccupationCodes.EEOOccupationCodeID }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDefaultALMValue", "OrganizationService.cs");
                throw;
            }

        }
    }
}
