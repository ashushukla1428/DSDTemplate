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
    public class DashboardService : IDashboard
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public DashboardService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }

        public List<string> GetdbCsvFileList(string year)
        {
            try
            {
                List<String> _value = new List<String>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }

                _value = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.Status == true).OrderByDescending(e => e.CreatedDate).Select(e => e.FileName).ToList();
                _value = _value.Where(e => year.Contains(trimFileNameForYear(e))).ToList();


                return _value;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetdbCsvFileList", "DashboardService.cs");
                throw;
            }
        }
        public List<string> GetdbSelectedYearList()
        {
            List<String> _value = new List<String>();
            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            int _user = Convert.ToInt32(_Loginmodel.UserId);
            int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
            Int32 adminselectedOrgId = 0;
            if (AppUtility.GetOrgIdForAdminView().Length > 0)
            {
                adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                _organizationId = adminselectedOrgId;
            }
            var _FileList = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.Status == true).OrderByDescending(e => e.CreatedDate).Select(e => e.FileName).ToList();

            _value = _FileList.Select(e => trimFileNameForYear(e)).ToList();

            return _value.Distinct().ToList();

        }
        private string trimFileNameForYear(string FileName)
        {
            string result = FileName.Split('_')[4];
            return result;
        }
        public string GetdbCurrentSelectedYear()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }
                int fileSubmissionId = GetFileSubmissionID(_organizationId);
                String _CurrentUserFile = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.FileSubmission.FileSubmissionId == fileSubmissionId && e.Status == true).FirstOrDefault().FileName;

                return trimFileNameForYear(_CurrentUserFile);
            }
            catch
            {
                return "";
            }
        }
        public Dictionary<string, string> GetdbCurrentOrganisationColorCode()
        {
            try
            {
                // Dictionary<string, string> _list = new Dictionary<string, string>();

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var _UserModel = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                int _organizationId = 0;
                int _RoleId = 0;
                if (_UserModel != null)
                {
                    _organizationId = _UserModel.UserRole.Organization.OrganizationId;
                    _RoleId = _UserModel.UserRole.RoleId;
                }
                else
                {
                    Int32 adminselectedOrgId = 0;
                    if (AppUtility.GetOrgIdForAdminView().Length > 0)
                    {
                        adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                        _organizationId = adminselectedOrgId;
                    }
                    if (AppUtility.GetRoleIdForAdminView().Length > 0)
                    {
                        _RoleId = Convert.ToInt32(AppUtility.GetRoleIdForAdminView());
                    }
                }
                //GraphLevel.Code default equals to 4 for dashboard
                var GraphOrganizationViewList = _context.AssignedGraphOrganizationViews.Where(e => e.Organization.OrganizationId == _organizationId && e.UserRole.RoleId == _RoleId && e.GraphOrganizationView.Active == true).Select(e => e.GraphOrganizationView).ToList();
                var CurrentSelector = GraphOrganizationViewList.OrderBy(x => x.OrderNo).Select(e => e.Name).FirstOrDefault();
                var Result = GetdbDynamicColorChanger(CurrentSelector);

                return Result;
                //var result = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                //_list.Add("Filled", result.Select(e => e.NonVacanciesDisplayColorCode).FirstOrDefault());
                //_list.Add("Vacant", result.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                //return _list;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetdbCurrentOrganisationColorCode", "DashboardService.cs");
                throw;
            }
        }
        public string GetdbCurrentUserFile()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }
                int fileSubmissionId = GetFileSubmissionID(_organizationId);
                String _CurrentUserFile = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.FileSubmission.FileSubmissionId == fileSubmissionId && e.Status == true).FirstOrDefault().FileName;

                return _CurrentUserFile;
            }
            catch
            {
                return "";
            }
        }
        public Dictionary<string, string> GetdbDynamicColorChanger(string Selectedvalue)
        {
            try
            {
                Dictionary<string, string> _list = new Dictionary<string, string>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }

                //Get Code from selected Value
                var GraphOrganizationViewCode = _context.GraphOrganizationViews.Where(e => e.Name == Selectedvalue).Select(e => e.ColorKey).FirstOrDefault();

                if (GraphOrganizationViewCode == "CS3")
                {
                    _list = _context.Races.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "RC" + e.RaceId.ToString(), e => e.DisplayColorCode);

                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("RC0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS2")
                {
                    _list = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "EEO" + e.EEOJobCategoryId.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS4")
                {
                    var result = _context.Genders.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    _list.Add("female", result.Where(e => e.Name == "Female").FirstOrDefault().DisplayColorCode);
                    _list.Add("male", result.Where(e => e.Name == "Male").FirstOrDefault().DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("na", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS1")
                {
                    var result = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();//.ToDictionary(e => "EEO" + e.EEOJobCategoryId.ToString(), e => e.DisplayColorCode);
                    _list.Add("Filled", result.Select(e => e.NonVacanciesDisplayColorCode).FirstOrDefault());
                    _list.Add("Vacant", result.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS9")
                {

                    _list = _context.LastPerformanceRatings.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "LPR" + e.LastPerformanceRatingId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("LPR0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());

                    //var EEORaingValue = _context.EEORatings.Where(e => e.Organization.OrganizationId == _organizationId && e.Active == true).FirstOrDefault();

                    //_list.Add("ERR1", EEORaingValue.NonSupervisorDisplayColorCode);
                    //_list.Add("ERR2", EEORaingValue.BelowBenchmarkSupervisorDisplayColorCode);
                    //_list.Add("ERR3", EEORaingValue.AboveBenchmarkSupervisorDisplayColorCode);
                    //var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    //_list.Add("ERR0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS5")
                {
                    _list = _context.SalaryRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "SR" + e.SalaryRangeId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("SR0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS6")
                {
                    _list = _context.AgeRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "AR" + e.AgeRangeId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("AR0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS7")
                {
                    _list = _context.PositionYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "PSP" + e.PositionYearsOfServiceId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("PSP0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS11")
                {
                    _list = _context.VacancyRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "VR" + e.VacancyRangeId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("VR0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                //Position Service Period
                else
                {
                    _list = _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId).ToDictionary(e => "TYS" + e.AgencyYearsOfServiceId.ToString(), e => e.DisplayColorCode);
                    var VacanciesColor = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _list.Add("TYS0", VacanciesColor.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());

                }
                return _list;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetdbDynamicColorChanger", "DashboardService.cs");
                throw;
            }
        }
        public Dictionary<string, string> GetDBLegendList(string Selectedvalue, string filename)
        {
            try
            {
                Dictionary<string, string> _ResultList = new Dictionary<string, string>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }
                //Get Code from selected Value
                var GraphOrganizationViewCode = _context.GraphOrganizationViews.Where(e => e.Name == Selectedvalue).Select(e => e.ColorKey).FirstOrDefault();

                if (GraphOrganizationViewCode == "CS3")
                {
                    _ResultList = _context.Races.Where(e => e.Organization.OrganizationId == _organizationId)
                                  .ToDictionary(e => e.Name.ToString() + "^" + e.RaceKey.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS2")
                {
                    _ResultList = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == _organizationId)
                                 .ToDictionary(e => e.Name.ToString() + "^" + e.EEOJobCategoryKey.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS4")
                {
                    var resultGenders = _context.Genders.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    _ResultList.Add("Female^genderFemale", resultGenders.Where(e => e.Name == "Female").FirstOrDefault().DisplayColorCode);
                    _ResultList.Add("Male^genderMale", resultGenders.Where(e => e.Name == "Male").FirstOrDefault().DisplayColorCode);

                }
                else if (GraphOrganizationViewCode == "CS1")
                {
                    var resultvacant = _context.Organizations.Where(e => e.OrganizationId == _organizationId).ToList();
                    _ResultList.Add("Filled" + "^filled", resultvacant.Select(e => e.NonVacanciesDisplayColorCode).FirstOrDefault());
                    _ResultList.Add("Vacant" + "^vacant", resultvacant.Select(e => e.VacanciesDisplayColorCode).FirstOrDefault());
                }
                else if (GraphOrganizationViewCode == "CS9")
                {
                    _ResultList = _context.LastPerformanceRatings.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                               .ToDictionary(e => e.Name.ToString() + "^" + e.LastPerformanceRatingKey.ToString(), e => e.DisplayColorCode);
                    //var EEORaingData = _context.EEORatings.Where(e => e.Organization.OrganizationId == _organizationId && e.Active == true).FirstOrDefault();
                    //Decimal BMIndicator = EEORaingData.BenchMarkValueIndicator;
                    //Dictionary<string, string> _listEEORaing = new Dictionary<string, string>();
                    //_ResultList.Add(EEORaingData.NonSupervisorLabelDisplay + "^NSP", EEORaingData.NonSupervisorDisplayColorCode);
                    //_ResultList.Add(EEORaingData.BelowBenchmarkSupervisorLabelDisplay + "^BBS", EEORaingData.BelowBenchmarkSupervisorDisplayColorCode);
                    //_ResultList.Add(EEORaingData.AboveBenchmarkSupervisorLabelDisplay + "^ABS", EEORaingData.AboveBenchmarkSupervisorDisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS5")
                {
                    _ResultList = _context.SalaryRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                .ToDictionary(e => e.Name.ToString() + "^" + e.SalaryKey.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS11")
                {
                    _ResultList = _context.VacancyRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                .ToDictionary(e => e.Name.ToString() + "^" + e.VacancyRangeKey.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS6")
                {
                    _ResultList = _context.AgeRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                 .ToDictionary(e => e.Name.ToString() + "^" + e.AgeRangeKey.ToString(), e => e.DisplayColorCode);
                }
                else if (GraphOrganizationViewCode == "CS7")
                {
                    _ResultList = _context.PositionYearsOfServices.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                .ToDictionary(e => e.Name.ToString() + "^" + e.PositionYearsOfServiceKey.ToString(), e => e.DisplayColorCode);
                }
                else
                {
                    _ResultList = _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId)
                                .ToDictionary(e => e.Name.ToString() + "^" + e.AgencyYearsOfServiceKey.ToString(), e => e.DisplayColorCode);

                }
                _ResultList.Add("KeyValue", GraphOrganizationViewCode);
                return _ResultList;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDBLegendList", "DashboardService.cs");
                throw;
            }
        }
        public Dictionary<string, string> GetLegendCollections(string filename)
        {
            try
            {
                Dictionary<string, string> _ResultList = new Dictionary<string, string>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organizationId = adminselectedOrgId;
                }
                _ResultList.Add("Races", String.Join(",", _context.Races.Where(e => e.Organization.OrganizationId == _organizationId)
                    .Select(e => e.RaceKey.ToString())));

                _ResultList.Add("EEOJobCategories", String.Join(",", _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == _organizationId)
                             .Select(e => e.EEOJobCategoryKey.ToString())));

                _ResultList.Add("Genders", "genderFemale,genderMale");

                _ResultList.Add("Jobvacany", "filled,vacant");

                _ResultList.Add("EEORatings", "NSP,BBS,ABS");

                _ResultList.Add("LastPerformanceRating", String.Join(",", _context.LastPerformanceRatings.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                              .Select(e => e.LastPerformanceRatingKey.ToString())));

                _ResultList.Add("SalaryRanges", String.Join(",", _context.SalaryRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                                .Select(e => e.SalaryKey.ToString())));
                _ResultList.Add("VacanyRanges", String.Join(",", _context.VacancyRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                               .Select(e => e.VacancyRangeKey.ToString())));

                _ResultList.Add("AgeRanges", String.Join(",", _context.AgeRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                               .Select(e => e.AgeRangeKey.ToString())));

                _ResultList.Add("PositionYearsOfServices", String.Join(",", _context.PositionYearsOfServices.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == _organizationId)
                                                          .Select(e => e.PositionYearsOfServiceKey.ToString())));

                _ResultList.Add("AgencyYearsOfServices", String.Join(",", _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId)
                            .Select(e => e.AgencyYearsOfServiceKey.ToString())));


                return _ResultList;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDBLegendList", "DashboardService.cs");
                throw;
            }
        }
        private int GetFileSubmissionID(int OrganizationId)
        {
            try
            {
                var FileSubmissionId = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == OrganizationId && e.Status == true)
                                       .Max(e => e.FileSubmission.FileSubmissionId);
                return FileSubmissionId;
            }
            catch
            {
                return 0;
            }
        }
        public Dictionary<string, string> GetUserLabel(int organisastionId)
        {
            try
            {

                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var _UserModel = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();

                int _RoleId = 0;
                if (_UserModel != null)
                {
                    _RoleId = _UserModel.UserRole.RoleId;
                }
                else
                {
                    if (AppUtility.GetRoleIdForAdminView().Length > 0)
                    {
                        _RoleId = Convert.ToInt32(AppUtility.GetRoleIdForAdminView());
                    }
                }

                var OrganizationLabelFieldData = _context.OrganizationLabelFields.Where(e => e.Organization.OrganizationId == organisastionId && e.UserRole.RoleId == _RoleId).ToList();
                if (OrganizationLabelFieldData.Count() > 0)
                {
                    Dictionary<String, String> _GetUserLabel = new Dictionary<string, string>();

                    var result = OrganizationLabelFieldData.ToDictionary(x => x.DefaultLabelField.LabelKey, x => x.Active ? x.DisplayLabelData : "");
                    return result;
                }
                else
                {
                    var DefaultLabelFieldData = _context.DefaultLabelFields.ToList();
                    var result = DefaultLabelFieldData.ToDictionary(x => x.LabelKey, x => x.DisplayLabelData);
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public OrhChartDashborad BindOrhChartDashborad()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                //var user = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                var _organization = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization).FirstOrDefault();

                Int32 adminselectedOrgId = 0;
                if (AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    adminselectedOrgId = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _organization = _context.Organizations.Where(e => e.OrganizationId == adminselectedOrgId).FirstOrDefault();
                }

                var ListFileSubmission = new List<SelectListItem>();

                var FileSubmission = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organization.OrganizationId && e.Status == true).ToList().OrderByDescending(e => e.FileSubmission.SubmissionDateTime);
                if (FileSubmission != null)
                {
                    ListFileSubmission.AddRange(FileSubmission.OrderByDescending(e => e.FileSubmission.SubmissionDateTime).Where(e => e.FileSubmission.FileSubmissionStatu.Status == "Validated")
                        .Select(g => new SelectListItem { Text = "Version : " + g.FileSubmission.FileVersionNumber.ToString() + " (" + g.FileSubmission.SubmissionDateTime.Value.ToString("MM/dd/yyyy") + ")" + " (" + g.FileSubmission.FileSubmissionStatu.Description + ") ", Value = g.FileSubmission.FileSubmissionId.ToString() }).ToList());
                }

                OrhChartDashborad _model = new OrhChartDashborad
                {
                    OrganizationId = _organization.OrganizationId,
                    OrganizationName = _organization.Name,
                    ListFileSubmission = ListFileSubmission,
                    Title = "Title",
                    SubTitle = "Subtitle",
                    EffectiveDate = Convert.ToDateTime(FileSubmission.FirstOrDefault().FileSubmission.SubmissionDateTime).ToString("MM/dd/yyyy"),
                    FileSubmissionId = FileSubmission.FirstOrDefault().FileSubmission.FileSubmissionId,
                    FilePath = @"../../Dashboard/organizationcsv/" + FileSubmission.FirstOrDefault().FileName + ".csv"
                };

                return _model;
            }
            catch
            {

                OrhChartDashborad errorModel = new OrhChartDashborad();
                errorModel.ListFileSubmission = new List<SelectListItem>();
                return errorModel;
            }
        }

        public OrhChartDashborad GetFileSubmissionDetail(int FileSubmissionId)
        {
            var FileSubmission = _context.GenerateCSVs.Where(e => e.FileSubmission.FileSubmissionId == FileSubmissionId).FirstOrDefault();

            OrhChartDashborad _model = new OrhChartDashborad
            {
                OrganizationId = FileSubmission.Organization.OrganizationId,
                OrganizationName = FileSubmission.Organization.Name,
                Title = "Title",
                SubTitle = "Subtitle",
                EffectiveDate = Convert.ToDateTime(FileSubmission.FileSubmission.SubmissionDateTime).ToString("MM/dd/yyyy"),
                FileSubmissionId = FileSubmission.FileSubmission.FileSubmissionId,
                FilePath = @"../../Dashboard/organizationcsv/" + FileSubmission.FileName + ".csv"
            };

            return _model;
        }
    }
}
