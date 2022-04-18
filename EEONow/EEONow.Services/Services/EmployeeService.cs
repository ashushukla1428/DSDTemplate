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
    public class EmployeeService : IEmployeeService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EmployeeService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public EmployeeSearchModel BindEmployeeFilterModel()
        {
            try
            {
                EmployeeSearchModel model = new EmployeeSearchModel();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);

                var UserDetail = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                if (UserDetail != null)
                {
                    var FileSubmission = UserDetail.UserRole.Organization.FileSubmissions;
                    model.ListFileSubmissionFilter = new List<SelectListItem>();
                    //model.ListFileSubmissionFilter.Add(new SelectListItem { Text = "Please select", Value = "0", Selected = true });
                    model.ListFileSubmissionFilter.AddRange(FileSubmission.OrderByDescending(e => e.FileVersionNumber).OrderByDescending(e => e.SubmissionDateTime).Where(e => e.FileSubmissionStatu.Status != "Uploaded").Select(g => new SelectListItem { Text = "Version : " + g.FileVersionNumber.ToString() + " (" + g.SubmissionDateTime.Value.ToString("MM/dd/yyyy") + ")" + " (" + g.FileSubmissionStatu.Description + ") ", Value = g.FileSubmissionId.ToString() }).ToList());
                    // model.FileSubmissionFilterId = 0;

                }
                return model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEmployeeFilterModel", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<EmployeesModel>> GetEmployeeModel()
        {
            EmployeeSearchModel model = new EmployeeSearchModel();
            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            int _user = Convert.ToInt32(_Loginmodel.UserId);
            List<EmployeesModel> _lstModel = new List<EmployeesModel>();

            var _OrganizationId = await _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefaultAsync();
            if (_OrganizationId > 0)
            {
                var _Employee = await _context.Employees.Where(e => e.Organization.OrganizationId == _OrganizationId).ToListAsync();

                try
                {
                    _lstModel.AddRange(_Employee.Select(g => new EmployeesModel
                    {
                        EmployeeId = g.EmployeeId,
                        FirstName = g.FirstName,
                        MiddleName = g.MiddleName,
                        LastName = g.LastName,
                        Email = g.Email,
                        LastPerformanceRatingValue = g.LastPerformanceRatingValue == null ? -1 : Convert.ToDecimal(g.LastPerformanceRatingValue),
                        LastPerformanceRatingId = g.LastPerformanceRating == null ? 0 : g.LastPerformanceRating.LastPerformanceRatingId,
                        PhoneNumber = g.PhoneNumber,
                        //  PicturePath = g.PicturePath,
                        PositionTitle = g.PositionTitle,
                        ProgramOfficeName = g.ProgramOfficeName,
                        // StateDateOfHire = g.StateDateOfHire,
                        SupervisorFlag = g.SupervisorFlag,
                        SupervisorPositionNumber = g.SupervisorPositionNumber,


                        Salary = g.Salary,
                        PositionNumber = g.PositionNumber,
                        //   Vacant = g.Vacant,
                        AgencyDateOfHire = g.AgencyDateOfHire,
                        CountyId = g.County == null ? 0 : g.County.CountyId,
                        CountyName = g.County == null ? "" : g.County.Name,
                        // DateOfBirth = g.DateOfBirth,
                        EEOCodeId = g.EEOJobCategory == null ? 0 : g.EEOJobCategory.EEOJobCategoryId,
                        EEOCodeName = g.EEOJobCategory == null ? "" : g.EEOJobCategory.Name,
                        RaceId = g.Race == null ? 0 : g.Race.RaceId,
                        RaceName = g.Race == null ? "" : g.Race.Name,
                        FileSubmissionId = g.FileSubmission.FileSubmissionId,
                        GenderId = g.Gender == null ? 0 : g.Gender.Name == "Male" ? 1 : 2,
                        GenderName = g.Gender == null ? "" : g.Gender.Name,
                        AgencyYearsOfServiceId = g.AgencyYearsOfService == null ? 0 : g.AgencyYearsOfService.AgencyYearsOfServiceId,
                        AgencyYearsOfServiceName = g.AgencyYearsOfService == null ? "" : g.AgencyYearsOfService.Name,

                        StateId = g.State == null ? 0 : g.State.StateId,
                        StateName = g.State == null ? "" : g.State.Name,
                        WorkAddress = g.WorkAddress,
                        WorkCity = g.WorkCity,
                        WorkZipCode = g.WorkZipCode,
                        OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                        OrganizationName = g.Organization == null ? "" : g.Organization.Name
                    }).ToList());
                }
                catch (Exception ex)
                {
                    AppUtility.LogMessage(ex, "GetEmployeeModel", "EmployeeService.cs");
                    throw;
                }
            }
            return _lstModel;
        }
        public async Task<ResponseModel> UpdateEmployee(EmployeesModel _model)
        {
            try
            {
                var _Employee = await _repository.FindAsync<Employee>(x => x.EmployeeId == _model.EmployeeId);
                if (_Employee != null)
                {
                    LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    int _user = Convert.ToInt32(_Loginmodel.UserId);

                    _Employee.FirstName = _model.FirstName;
                    _Employee.MiddleName = _model.MiddleName;
                    _Employee.LastName = _model.LastName;
                    _Employee.Filled = _model.Filled;
                    _Employee.PositionNumber = _model.PositionNumber;
                    _Employee.SupervisorPositionNumber = _model.SupervisorPositionNumber;
                    _Employee.PhoneNumber = _model.PhoneNumber;
                    _Employee.Email = _model.Email;
                    _Employee.SupervisorFlag = _model.SupervisorFlag;
                    _Employee.Age = _model.Age;
                    _Employee.PositionTitle = _model.PositionTitle;
                    _Employee.ProgramOfficeName = _model.ProgramOfficeName;
                    _Employee.Salary = _model.Salary;
                    _Employee.LastPerformanceRatingValue = _model.LastPerformanceRatingValue;
                    _Employee.LastPerformanceRating = await _repository.FindAsync<LastPerformanceRating>(x => x.LastPerformanceRatingId == _model.LastPerformanceRatingId);
                    _Employee.StateCumulativeMonthsOfService = _model.StateCumulativeMonthsOfService;
                    _Employee.OPSPosition = _model.OPSPosition;
                    _Employee.PositionDateOfHire = _model.PositionDateOfHire;
                    _Employee.NationalOrigin = _model.NationalOrigin;
                    _Employee.AgencyDateOfHire = _model.AgencyDateOfHire;
                    _Employee.WorkAddress = _model.WorkAddress;
                    _Employee.WorkCity = _model.WorkCity;
                    _Employee.WorkZipCode = _model.WorkZipCode;
                    _Employee.UpdateUserId = _user;
                    _Employee.UpdateDateTime = DateTime.Now;
                    _Employee.County = await _repository.FindAsync<County>(x => x.CountyId == _model.CountyId);
                    _Employee.Gender = await _repository.FindAsync<Gender>(x => x.GenderId == _model.GenderId);
                    _Employee.Organization = await _repository.FindAsync<Organization>(x => x.OrganizationId == _model.OrganizationId);
                    _Employee.Race = await _repository.FindAsync<Race>(x => x.RaceId == _model.RaceId);
                    _Employee.State = await _repository.FindAsync<State>(x => x.StateId == _model.StateId);
                    _Employee.AgencyYearsOfService = await _repository.FindAsync<AgencyYearsOfService>(x => x.AgencyYearsOfServiceId == _model.AgencyYearsOfServiceId); //_model.AgencyYearsOfService
                    _Employee.PositionYearsOfService = await _repository.FindAsync<PositionYearsOfService>(x => x.PositionYearsOfServiceId == _model.PositionYearsOfServiceId); //_model.AgencyYearsOfService
                    _Employee.EEOJobCategory = await _repository.FindAsync<EEOJobCategory>(x => x.EEOJobCategoryId == _model.EEOCodeId);

                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = _model.EmployeeId };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateEmployee", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<FileSubmissionModel>> GetFileSubmissionModel()
        {
            try
            {
                FileSubmissionModel model = new FileSubmissionModel();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                List<FileSubmissionModel> _lstModel = new List<FileSubmissionModel>();

                var UserDetail = await _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefaultAsync();
                if (UserDetail != null)
                {
                    var _Files = await _context.FileSubmissions.ToListAsync();

                    try
                    {
                        _lstModel.AddRange(_Files.Select(g => new FileSubmissionModel
                        {
                            OrganizationId = g.Organization.OrganizationId,
                            FileSubmissionId = g.FileSubmissionId,
                            OriginalFileName = g.OriginalFileName
                        }).ToList());
                    }
                    catch (Exception ex)
                    {
                        AppUtility.LogMessage(ex, "GetFileSubmissionModel", "EmployeeService.cs");
                        throw;
                    }
                }
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileSubmissionModel", "EmployeeService.cs");
                throw;
            }
        }
        public List<SelectListItem> GetFileSubmissionViaOrganisation(int? organization)
        {
            try
            {
                var ListFileSubmission = new List<SelectListItem>();
                if (organization > 0 && organization != null)
                {
                    var FileSubmission = _context.Organizations.Where(e => e.OrganizationId == organization).Select(e => e.FileSubmissions).FirstOrDefault();
                    if (FileSubmission != null)
                    {
                        ListFileSubmission.AddRange(FileSubmission.OrderByDescending(e => e.FileVersionNumber).OrderByDescending(e => e.SubmissionDateTime).Where(e => e.FileSubmissionStatu.Status == "Validated").Select(g => new SelectListItem { Text = "Version : " + g.FileVersionNumber.ToString() + " (" + g.SubmissionDateTime.Value.ToString("MM/dd/yyyy") + ")" + " (" + g.FileSubmissionStatu.Description + ") ", Value = g.FileSubmissionId.ToString() }).ToList());
                    }

                }
                return ListFileSubmission;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileSubmissionViaOrganisation", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<EmployeesModel>> GetEmployeeViaOrganizationsModel(int FileSubmissionId, string region)
        {
            try
            {
                EmployeeSearchModel model = new EmployeeSearchModel();

                List<EmployeesModel> _lstModel = new List<EmployeesModel>();

                var _Employee = await _context.Employees.Where(e => e.FileSubmission.FileSubmissionId == FileSubmissionId && e.RegionName == (region.ToString().Length > 0 ? region : e.RegionName)).ToListAsync();
                if (_Employee != null)
                {
                    var EmployeeLevel = await _context.EmployeeLevels.Where(e => e.FileSubmissionId == FileSubmissionId).Select(e => new { EmployeeId = e.Employee.EmployeeId, EmployeeLevelNumber = e.EmployeeLevelNumber }).ToListAsync();
                    try
                    {
                        _lstModel.AddRange(_Employee.Select(g => new EmployeesModel
                        {
                            EmployeeId = g.EmployeeId,
                            FirstName = g.FirstName,
                            MiddleName = g.MiddleName,
                            LastName = g.LastName,
                            Email = g.Email,
                            LastPerformanceRatingId = g.LastPerformanceRating == null ? 0 : g.LastPerformanceRating.LastPerformanceRatingId,
                            LastPerformanceRatingValue = g.LastPerformanceRatingValue == null ? 0 : g.LastPerformanceRatingValue.Value,
                            PhoneNumber = g.PhoneNumber,
                            Age = g.Age,
                            AgeRangeId = g.AgeRange == null ? 0 : g.AgeRange.AgeRangeId,
                            AgeRangeName = g.AgeRange == null ? "" : g.AgeRange.Name,
                            Filled = g.Filled,
                            NationalOrigin = g.NationalOrigin,
                            OPSPosition = g.OPSPosition.Value,
                            PositionDateOfHire = g.Filled == false ? null : g.PositionDateOfHire,
                            StateCumulativeMonthsOfService = g.StateCumulativeMonthsOfService,
                            // PicturePath = g.PicturePath,
                            PositionTitle = g.PositionTitle,

                            ProgramOfficeName = g.ProgramOfficeName,
                            // StateDateOfHire = g.StateDateOfHire,
                            SupervisorFlag = g.SupervisorFlag,
                            SupervisorPositionNumber = g.SupervisorPositionNumber,
                            VacancyDate = g.Filled == true ? null : g.VacancyDate,
                            VacancyDateRangeId = g.VacancyRange == null ? 0 : g.VacancyRange.VacancyRangeId,
                            Salary = g.Salary,
                            SalaryRangeId = g.SalaryRange == null ? 0 : g.SalaryRange.SalaryRangeId,
                            SalaryRangeName = g.SalaryRange == null ? "" : g.SalaryRange.Name,
                            PositionNumber = g.PositionNumber,
                            //  Vacant = g.Vacant,
                            AgencyDateOfHire = g.Filled == false ? null : g.AgencyDateOfHire,
                            CountyId = g.County == null ? 0 : g.County.CountyId,
                            CountyName = g.County == null ? "" : g.County.Name,
                            //  DateOfBirth = g.DateOfBirth,
                            EEOCodeId = g.EEOJobCategory == null ? 0 : g.EEOJobCategory.EEOJobCategoryId,
                            EEOCodeName = g.EEOJobCategory == null ? "" : g.EEOJobCategory.Name,
                            RaceId = g.Race == null ? 0 : g.Race.RaceId,
                            RaceName = g.Race == null ? "" : g.Race.Name,
                            RaceIndex = Convert.ToDecimal(g.EEORaceIndex),
                            SpanOfControl = Convert.ToInt32(g.SpanOfControl),
                            FileSubmissionId = g.FileSubmission.FileSubmissionId,
                            GenderId = g.Gender == null ? 0 : g.Gender.Name == "Male" ? 1 : 2,
                            GenderName = g.Gender == null ? "" : g.Gender.Name,
                            GenderIndex = Convert.ToDecimal(g.EEOGenderIndex),
                            Region = g.RegionName,
                            AgencyYearsOfServiceId = g.AgencyYearsOfService == null ? 0 : g.AgencyYearsOfService.AgencyYearsOfServiceId,
                            AgencyYearsOfServiceName = g.AgencyYearsOfService == null ? "" : g.AgencyYearsOfService.Name,

                            PositionYearsOfServiceId = g.PositionYearsOfService == null ? 0 : g.PositionYearsOfService.PositionYearsOfServiceId,
                            PositionYearsOfServiceName = g.PositionYearsOfService == null ? "" : g.PositionYearsOfService.Name,

                            StateId = g.State == null ? 0 : g.State.StateId,
                            StateName = g.State == null ? "" : g.State.Name,
                            WorkAddress = g.WorkAddress,
                            WorkCity = g.WorkCity,
                            WorkZipCode = g.WorkZipCode,
                            OrganizationId = g.Organization == null ? 0 : g.Organization.OrganizationId,
                            OrganizationName = g.Organization == null ? "" : g.Organization.Name,
                            EmployeeLevel = "Level - " + EmployeeLevel.Where(e => e.EmployeeId == g.EmployeeId).FirstOrDefault().EmployeeLevelNumber
                        }).ToList());
                    }
                    catch (Exception ex)
                    {
                        AppUtility.LogMessage(ex, "GetEmployeeViaOrganizationsModel", "EmployeeService.cs");
                        throw;
                    }

                    // HttpContext.Current.Session["Employee"] = _lstModel;
                }
                return _lstModel;
                // } 
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEmployeeViaOrganizationsModel", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<EmployeeModelForExport>> GetExportEmployeeViaOrganizationsModel(int FileSubmissionId, string region)
        {
            try
            {
                EmployeeSearchModel model = new EmployeeSearchModel();

                List<EmployeeModelForExport> _lstModel = new List<EmployeeModelForExport>();

                var _Employee = await _context.Employees.Where(e => e.FileSubmission.FileSubmissionId == FileSubmissionId && e.RegionName == (region.ToString().Length > 0 ? region : e.RegionName)).ToListAsync();
                if (_Employee != null)
                {
                    var EmployeeLevel = await _context.EmployeeLevels.Where(e => e.FileSubmissionId == FileSubmissionId).Select(e => new { EmployeeId = e.Employee.EmployeeId, EmployeeLevelNumber = e.EmployeeLevelNumber }).ToListAsync();
                    try
                    {
                        _lstModel.AddRange(_Employee.Select(g => new EmployeeModelForExport
                        {
                            EmployeeId = Convert.ToString(g.EmployeeId),
                            FirstName = g.FirstName,
                            MiddleName = g.MiddleName,
                            LastName = g.LastName,
                            Email = g.Email,

                            Organization = g.Organization == null ? "" : g.Organization.Name,
                            LastPerformanceRating = g.LastPerformanceRating == null ? "" : g.LastPerformanceRating.Name,
                            LastPerformanceRatingValue = Convert.ToString(g.LastPerformanceRatingValue),
                            PhoneNumber = g.PhoneNumber,
                            Age = Convert.ToString(g.Age),
                            AgeRange = g.AgeRange == null ? "" : g.AgeRange.Name,
                            Filled = Convert.ToString(g.Filled),
                            NationalOrigin = g.NationalOrigin,
                            OPSPosition = Convert.ToString(g.OPSPosition.Value),
                            PositionDateOfHire = g.Filled == false ? "" : Convert.ToString(g.PositionDateOfHire),
                            StateCumulativeMonthsOfService = Convert.ToString(g.StateCumulativeMonthsOfService),
                            PositionTitle = g.PositionTitle,
                            ProgramOfficeName = g.ProgramOfficeName,
                            SupervisorFlag = Convert.ToString(g.SupervisorFlag),
                            SupervisorPositionNumber = g.SupervisorPositionNumber,
                            VacancyDate = g.Filled == true ? "" : Convert.ToDateTime(g.VacancyDate).ToString("MM/dd/yyyy"),
                            VacancyDateRange = g.VacancyRange == null ? "" : g.VacancyRange.Name,
                            Salary = Convert.ToString(g.Salary),
                            SalaryRange = g.SalaryRange == null ? "" : g.SalaryRange.Name,
                            PositionNumber = g.PositionNumber,
                            AgencyDateOfHire = g.Filled == false ? "" : Convert.ToString(g.AgencyDateOfHire),
                            County = g.County == null ? "" : g.County.Name,
                            EEOCode = g.EEOJobCategory == null ? "" : g.EEOJobCategory.Name,
                            Race = g.Race == null ? "" : g.Race.Name,
                            GenderIndex = Convert.ToString(g.EEOGenderIndex),
                            Gender = g.Gender == null ? "" : g.Gender.Name,
                            RaceIndex = Convert.ToString(g.EEORaceIndex),
                            Region = g.RegionName,
                            SpanOfControl = Convert.ToString(g.SpanOfControl),
                            AgencyYearsOfService = g.AgencyYearsOfService == null ? "" : g.AgencyYearsOfService.Name,
                            PositionYearsOfService = g.PositionYearsOfService == null ? "" : g.PositionYearsOfService.Name,
                            State = g.State == null ? "" : g.State.Name,
                            WorkAddress = "\"" + g.WorkAddress.Replace("\"", "") + "\"",
                            WorkCity = "\"" + g.WorkCity.Replace("\"", "") + "\"",
                            WorkZipCode = g.WorkZipCode,
                            EmployeeLevel = "Level - " + EmployeeLevel.Where(e => e.EmployeeId == g.EmployeeId).FirstOrDefault().EmployeeLevelNumber


                        }).ToList());
                    }
                    catch (Exception ex)
                    {
                        AppUtility.LogMessage(ex, "GetExportEmployeeViaOrganizationsModel", "EmployeeService.cs");
                        throw;
                    }
                }
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetExportEmployeeViaOrganizationsModel", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindPositionTitleDropDown()
        {
            try
            {
                var _FinalPositionTitle = new List<SelectListItem>();
                var employeeData = await _context.Employees.Select(e => e.PositionTitle).Distinct().ToListAsync();
                var listPositionTitle = employeeData.GroupBy(e => e, (key, group) => group.FirstOrDefault()).ToList();
                if (listPositionTitle.Count() > 0)
                {
                    _FinalPositionTitle.AddRange(listPositionTitle.Select(g => new SelectListItem { Text = g.ToString(), Value = g.ToString() }).ToList());
                }
                return _FinalPositionTitle;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindPositionTitleDropDown", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindProgramOfficeDropDown()
        {
            try
            {
                var _FinalProgramOfficeName = new List<SelectListItem>();
                var employeeData = await _context.Employees.Select(e => e.ProgramOfficeName).Distinct().ToListAsync();
                var listProgramOfficeName = employeeData.GroupBy(e => e, (key, group) => group.FirstOrDefault()).ToList();
                if (listProgramOfficeName.Count() > 0)
                {
                    _FinalProgramOfficeName.AddRange(listProgramOfficeName.Select(g => new SelectListItem { Text = g.ToString(), Value = g.ToString() }).ToList());
                }
                return _FinalProgramOfficeName;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindProgramOfficeDropDown", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindEmployeeLevelDropDown(int? organizationID)
        {
            try
            {
                var _FinalPositionTitle = new List<SelectListItem>();
                var employeeData = await _context.EmployeeLevels
                                  .Where(e => e.OrganizationId == (organizationID == null ? e.OrganizationId : organizationID))
                                  .Select(e => new { EmployeeLevelNumber = e.EmployeeLevelNumber }).Distinct()
                                  .OrderBy(e => e.EmployeeLevelNumber)
                                  .Select(e => new SelectListItem { Text = "Level - " + e.EmployeeLevelNumber, Value = "Level - " + e.EmployeeLevelNumber }).ToListAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEmployeeLevelDropDown", "EmployeeService.cs");
                throw;
            }
        }
        public async Task<List<SelectListItem>> BindEmployeeActiveColumn()
        {
            try
            {
                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
                var employeeData = await _context.EmployeeActiveFields
                                   .Where(e => e.Active == true && e.UserRole.RoleId == _Loginmodel.UserRoleId).OrderBy(e=>e.Sorting)
                                  .Select(e => new SelectListItem { Text = e.DefaultEmployeeField.ColumnName, Value = e.DefaultEmployeeField.ColumnCode }).ToListAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEmployeeLevelDropDown", "EmployeeService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> GetSupervisorsViaOrganisation(int organization, int FileSubmissionId)
        {
            try
            {
                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
                if (organization == 0)
                {
                    organization = _Loginmodel.OrgId;
                }

                var employeeData = await _context.Employees
                                   .Where(e => e.SupervisorFlag == true && e.Filled==true && e.Organization.OrganizationId == organization && e.FileSubmission.FileSubmissionId==FileSubmissionId).OrderBy(e => e.LastName)
                                  .Select(e => new SelectListItem { Text = e.LastName + " "+ e.FirstName, Value = e.PositionNumber }).ToListAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetSupervisorViaOrganisation", "EmployeeService.cs");
                throw;
            }
        }

        public async Task<List<SelectListItem>> GetSupervisorsViaOrgPrgOffReg(int organization, int FileSubmissionId, string prgoffice, string region)
        {
            try
            {
                LoginResponse _Loginmodel = Utilities.AppUtility.DecryptCookie();
                if (organization == 0)
                {
                    organization = _Loginmodel.OrgId;
                }

                var employeeData = await _context.Employees
                                   .Where(e => e.Organization.OrganizationId == organization 
                                   && e.FileSubmission.FileSubmissionId == FileSubmissionId
                                   && e.ProgramOfficeName == (prgoffice.Length > 0 ? prgoffice : e.ProgramOfficeName)
                                   && e.RegionName == (region.Length > 0 ? region:e.RegionName)
                                   && e.SupervisorFlag == true
                                   && e.Filled == true
                                   ).OrderBy(e => e.LastName)
                                  .Select(e => new SelectListItem { Text = e.LastName + " " + e.FirstName, Value = e.PositionNumber }).ToListAsync();
                return employeeData;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetSupervisorsViaOrgPrgOffReg", "EmployeeService.cs");
                throw;
            }
        }
}
}
