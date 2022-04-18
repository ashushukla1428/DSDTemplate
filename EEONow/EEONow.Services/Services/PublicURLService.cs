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
    public class PublicURLService : IPublicURL
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public PublicURLService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public List<string> GetdbCsvFileList(string year, Int32 _organizationId)
        {
            try
            {
                List<String> _value = new List<String>();
                _value = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.Status == true).OrderByDescending(e => e.CreatedDate).Select(e => e.FileName).ToList();
                _value = _value.Where(e => year.Contains(trimFileNameForYear(e))).ToList();


                return _value;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetdbCsvFileList", "PublicURLService.cs");
                throw;
            }




        }
        public List<string> GetdbSelectedYearList(int OrgainisationId)
        {
            List<String> _value = new List<String>();
            var _FileList = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == OrgainisationId && e.Status == true).OrderByDescending(e => e.CreatedDate).Select(e => e.FileName).ToList();
            _value = _FileList.Select(e => trimFileNameForYear(e)).ToList();
            return _value.Distinct().ToList();
        }
        private string trimFileNameForYear(string FileName)
        {
            string result = FileName.Split('_')[4];
            return result;
        }
        public string GetdbCurrentSelectedYear(int OrgainisationId)
        {
            try
            {
                int fileSubmissionId = GetFileSubmissionID(OrgainisationId);
                String _CurrentUserFile = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == OrgainisationId && e.FileSubmission.FileSubmissionId == fileSubmissionId && e.Status == true).FirstOrDefault().FileName;
                return trimFileNameForYear(_CurrentUserFile);
            }
            catch
            {
                return "";
            }
        }
        public Dictionary<string, string> GetdbCurrentOrganisationColorCode(Int32 _organizationId, int RoleId)
        {
            try
            {

                var GraphOrganizationViewList = _context.AssignedGraphOrganizationViews.Where(e => e.Organization.OrganizationId == _organizationId && e.UserRole.RoleId == RoleId && e.GraphOrganizationView.Active == true).Select(e => e.GraphOrganizationView).ToList();
                var CurrentSelector = GraphOrganizationViewList.OrderBy(x => x.OrderNo).Select(e => e.Name).FirstOrDefault();
                var Result = GetdbDynamicColorChanger(_organizationId, CurrentSelector);

                return Result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetdbCurrentOrganisationColorCode", "PublicURLService.cs");
                throw;
            }
        }
        public string GetdbCurrentUserFile(Int32 _organizationId)
        {
            try
            {
                int fileSubmissionId = GetFileSubmissionID(_organizationId);
                String _CurrentUserFile = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == _organizationId && e.FileSubmission.FileSubmissionId == fileSubmissionId && e.Status == true).FirstOrDefault().FileName;
                return _CurrentUserFile;
            }
            catch
            {
                return "";
            }

        }
        public Dictionary<string, string> GetdbDynamicColorChanger(Int32 _organizationId, string Selectedvalue)
        {
            try
            {
                Dictionary<string, string> _list = new Dictionary<string, string>();
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
                AppUtility.LogMessage(ex, "GetdbDynamicColorChanger", "PublicURLService.cs");
                throw;
            }
        }
        public Dictionary<string, string> GetDBLegendList(Int32 _organizationId, string Selectedvalue, string filename)
        {
            try
            {
                Dictionary<string, string> _ResultList = new Dictionary<string, string>();
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
                AppUtility.LogMessage(ex, "GetDBLegendList", "PublicURLService.cs");
                throw;
            }
        }
        public Dictionary<string, string> GetLegendCollections(Int32 OrgainisationId, string filename)
        {
            try
            {
                Dictionary<string, string> _ResultList = new Dictionary<string, string>();

                _ResultList.Add("Races", String.Join(",", _context.Races.Where(e => e.Organization.OrganizationId == OrgainisationId)
                    .Select(e => e.RaceKey.ToString())));

                _ResultList.Add("EEOJobCategories", String.Join(",", _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrgainisationId)
                             .Select(e => e.EEOJobCategoryKey.ToString())));

                _ResultList.Add("Genders", "genderFemale,genderMale");

                _ResultList.Add("Jobvacany", "filled,vacant");

                _ResultList.Add("EEORatings", "NSP,BBS,ABS");

                _ResultList.Add("LastPerformanceRating", String.Join(",", _context.LastPerformanceRatings.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == OrgainisationId)
                                              .Select(e => e.LastPerformanceRatingKey.ToString())));

                _ResultList.Add("SalaryRanges", String.Join(",", _context.SalaryRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == OrgainisationId)
                                                .Select(e => e.SalaryKey.ToString())));
                _ResultList.Add("VacanyRanges", String.Join(",", _context.VacancyRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == OrgainisationId)
                                               .Select(e => e.VacancyRangeKey.ToString())));

                _ResultList.Add("AgeRanges", String.Join(",", _context.AgeRanges.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == OrgainisationId)
                                               .Select(e => e.AgeRangeKey.ToString())));

                _ResultList.Add("PositionYearsOfServices", String.Join(",", _context.PositionYearsOfServices.OrderBy(e => e.Number).Where(e => e.Organization.OrganizationId == OrgainisationId)
                                                          .Select(e => e.PositionYearsOfServiceKey.ToString())));

                _ResultList.Add("AgencyYearsOfServices", String.Join(",", _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == OrgainisationId)
                            .Select(e => e.AgencyYearsOfServiceKey.ToString())));


                return _ResultList;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetDBLegendList", "PublicURLService.cs");
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
        public Dictionary<string, string> GetUserLabel(int organisastionId, int RoleId)
        {
            try
            {
                var OrganizationLabelFieldData = _context.OrganizationLabelFields.Where(e => e.Organization.OrganizationId == organisastionId && e.UserRole.RoleId == RoleId).ToList();
                if (OrganizationLabelFieldData.Count() > 0)
                {
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


        public List<PublicURLModel> GetPublicURL()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);


                var _PublicURL = _context.PublicURLs.ToList();

                if (_Loginmodel.Roles != "DefinedSoftwareAdministrator")
                {
                    _PublicURL = _PublicURL.Where(e => e.Organization.OrganizationId == _Loginmodel.OrgId).ToList();
                }

                List<PublicURLModel> _lstModel = new List<PublicURLModel>();
                string baseUrl = ConfigurationManager.AppSettings["AppUrl"];
                String IFrame = "<iframe width='100%' style='overflow:hidden; position:fixed;border:none;margin:0px' height='100%;' src='" + baseUrl + "/PublicURL/Index?token={0}'></iframe>";
                _lstModel.AddRange(_PublicURL.Select(g => new PublicURLModel
                {
                    PublicURLId = g.PublicURLId,
                    Active=g.Active.Value,
                    OrgainisationName = g.Organization.Name.ToString(),
                    OrgainisationId = g.Organization.OrganizationId,
                    Token = g.Token,
                    PublicLink = String.Format(IFrame, g.Token)
                }).ToList());
                return _lstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetPublicURL", "PublicURLService.cs");
                throw;
            }

        }
        public Dictionary<Int32, Int32> GetOrgainisationIdViaToken(String Token)
        {
            try
            {
                var result = _context.PublicURLs.Where(e => e.Token == Token && e.Active == true).FirstOrDefault();
                Dictionary<Int32, Int32> _FinalResult = new Dictionary<int, int>();
                if (result != null)
                {
                    var validate = _context.GenerateCSVs.Where(e => e.Organization.OrganizationId == result.Organization.OrganizationId && e.Status == true).FirstOrDefault();


                    if (validate != null)
                    {
                        var PublicRole = result.Organization.UserRoles.Where(e => e.Name.ToLower() == ConfigurationManager.AppSettings["PublicUrlKey"].ToLower()).FirstOrDefault();

                        if (PublicRole != null)
                        {
                            _FinalResult.Add(result.Organization.OrganizationId, PublicRole.RoleId);
                        }
                        else
                        {
                            _FinalResult.Add(0, 0);
                        }
                    }
                    else
                    {
                        _FinalResult.Add(0, 0);
                    }
                }
                else
                {
                    _FinalResult.Add(0, 0);
                }
                return _FinalResult;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOrgainisationIdViaToken", "PublicURLService.cs");
                throw;
            }
        }



        public bool ReGenerateKey(int OrgainisationId)
        {
            try
            {
                var _PublicURL = _context.PublicURLs.Where(e => e.Organization.OrganizationId == OrgainisationId).FirstOrDefault();
                _PublicURL.Token = Guid.NewGuid().ToString();
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ReGenerateKey", "PublicURLService.cs");
                throw;
            }
        }
        public bool ActivateUrl(int OrgainisationId)
        {
            try
            {
                var _PublicURL = _context.PublicURLs.Where(e => e.Organization.OrganizationId == OrgainisationId).FirstOrDefault();
                _PublicURL.Active = _PublicURL.Active==true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ReGenerateKey", "PublicURLService.cs");
                throw;
            }
        }




    }
}
