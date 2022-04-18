using EEONow.Context.EntityContext;
using EEONow.Interface;
using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EEONow.Services
{
    public class UploadService : IUploadService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public UploadService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
            //_context.Configuration.AutoDetectChangesEnabled = false;
            // _context.Configuration.ValidateOnSaveEnabled = false;
        }
        public List<FileSubmissionRecordsModel> GetFileSubmissionRecords()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                // int _organizationId = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();

                var _ListRecords = _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _Organization.OrganizationId && p.FileSubmission.FileSubmissionStatu.Status != "Validated").ToList();

                List<FileSubmissionRecordsModel> _LstModel = new List<FileSubmissionRecordsModel>();

                _LstModel.AddRange(_ListRecords.Select(g => new FileSubmissionRecordsModel
                {
                    FileSubmissionRecordNumber = g.FileSubmissionRecordNumber,
                    FileSubmissionId = g.FileSubmissionId,
                    FirstName = g.FirstName,
                    MiddleName = g.MiddleName,
                    LastName = g.LastName,
                    Filled = g.Filled,
                    PositionNumber = g.PositionNumber,
                    SupervisorPositionNumber = g.SupervisorPositionNumber,
                    PhoneNumber = g.PhoneNumber,
                    Email = g.Email,
                    // PicturePath = g.PicturePath,
                    SupervisorFlag = g.SupervisorFlag,
                    Gender = g.Gender,
                    Race = g.Race,
                    Age = g.Age,
                    PositionTitle = g.PositionTitle,
                    ProgramOfficeName = g.ProgramOfficeName,
                    Salary = g.Salary,
                    EEOCode = g.EEOCode,
                    LastPerformanceRating = g.LastPerformanceRating,
                    StateCumulativeMonthsOfService = g.StateCumulativeMonthsOfService,
                    AgencyDateOfHire = g.AgencyDateOfHire,
                    WorkAddress = g.WorkAddress,
                    WorkCity = g.WorkCity,
                    WorkStateName = g.WorkState,
                    NationalOrigin = g.NationalOrigin,
                    OPSPosition = g.OPSPosition,
                    PositionDateOfHire = g.PositionDateOfHire,
                    UserID = g.UserID,
                    WorkZipCode = g.WorkZipCode,
                    WorkCounty = g.WorkCounty
                }).ToList());
                return _LstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileSubmissionRecords", "UploadService.cs");
                throw;
            }
        }
        public ResponseModel CsvUploading(String FileName, String OrignalName, String NewFileName, DateTime SubmissionDateTime)
        {
            try
            {
                var ResultTable = ConvertCSVtoDataTable(FileName);
                List<FileSubmissionRecord> _listFileSubmissionRecords = new List<FileSubmissionRecord>();
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                Int32 _FileVersionNumber = 0;
                if (_Organization.FileSubmissions != null && _Organization.FileSubmissions.Count() > 0)
                {
                    _FileVersionNumber = _Organization.FileSubmissions.Count() + 1;
                }
                else
                {
                    _FileVersionNumber = 1;
                }
                FileSubmission insertRecord = new FileSubmission
                {
                    Organization = _Organization,
                    FileSubmissionStatu = _context.FileSubmissionStatus.Where(e => e.Status == "Uploaded").FirstOrDefault(),
                    OriginalFileName = OrignalName,
                    NewFileName = NewFileName,
                    SubmissionDateTime = SubmissionDateTime,
                    FileVersionNumber = _FileVersionNumber,
                    AvailableLaborMarketFileVersion = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.OrganizationId == _Organization.OrganizationId && e.Active == true).FirstOrDefault(),
                    CreateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateUserId = _user,
                    UpdateDateTime = DateTime.Now
                };

                var resultFileSubmissions = _context.FileSubmissions.Add(insertRecord);
                _listFileSubmissionRecords = (from DataRow dr in ResultTable.Rows
                                              select new FileSubmissionRecord()
                                              {
                                                  FirstName = dr["Filled"].ToString().Trim() == "0" ? "vacant" : dr["FirstName"].ToString(),
                                                  MiddleName = dr["MiddleName"].ToString(),
                                                  LastName = dr["Filled"].ToString().Trim() == "0" ? "vacant" : dr["LastName"].ToString(),
                                                  Filled = dr["Filled"].ToString(),
                                                  PositionNumber = dr["PositionNumber"].ToString(),
                                                  SupervisorPositionNumber = dr["SupervisorPositionNumber"].ToString(),
                                                  PhoneNumber = dr["PhoneNumber"].ToString(),
                                                  Email = dr["Email"].ToString(),
                                                  NationalOrigin = dr["NationalOrigin"].ToString(),
                                                  Age = dr["Age"].ToString(),
                                                  //PicturePath = dr["PicturePath"].ToString(),
                                                  SupervisorFlag = dr["SupervisorFlag"].ToString(),
                                                  Gender = dr["Gender"].ToString(),
                                                  Race = dr["Race"].ToString(),
                                                  //DateOfBirth = dr["DateOfBirth"].ToString(),
                                                  PositionTitle = dr["PositionTitle"].ToString(),
                                                  ProgramOfficeName = dr["ProgramOfficeName"].ToString(),
                                                  Salary = dr["Salary"].ToString().Replace("\"", "").Replace("$", ""),
                                                  EEOCode = dr["eeoCode"].ToString(),
                                                  LastPerformanceRating = dr["LastPerformanceRating"].ToString(),
                                                  StateCumulativeMonthsOfService = dr["StateCumulativeMonthsOfService"].ToString(),
                                                  AgencyDateOfHire = dr["AgencyDateOfHire"].ToString(),
                                                  WorkAddress = dr["WorkAddress"].ToString(),
                                                  WorkCity = dr["WorkCity"].ToString(),
                                                  WorkState = dr["WorkState"].ToString(),
                                                  WorkZipCode = dr["WorkZipCode"].ToString(),
                                                  WorkCounty = dr["WorkCounty"].ToString(),
                                                  UserID = dr["UserID"].ToString(),
                                                  PositionDateOfHire = dr["PositionDateOfHire"].ToString(),
                                                  OPSPosition = dr["OPSPosition"].ToString(),
                                                  RegionName = dr["RegionName"].ToString(),
                                                  VacancyDate = dr["VacancyDate"].ToString(),
                                                  CreateDateTime = DateTime.Now,
                                                  UpdateDateTime = DateTime.Now,
                                                  CreateUserId = _user,
                                                  UpdateUserId = _user,
                                                  FileSubmission = resultFileSubmissions
                                              }).ToList();
                var result = _context.FileSubmissionRecords.AddRange(_listFileSubmissionRecords);
                _context.SaveChanges();

                return new ResponseModel { Message = "File successfully uploaded", Succeeded = true, Id = 1 };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "CsvUploading", "UploadService.cs");
                throw;
            }
        }
        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            try
            {
                DataTable dt = new DataTable();
                //Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                using (StreamReader sr = new StreamReader(strFilePath))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {

                        string[] rows = CSVParser.Split(sr.ReadLine());//sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Replace("\"", "");
                        }
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "ConvertCSVtoDataTable", "UploadService.cs");
                throw;
            }
        }
        public string GetFileName()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                // var org = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization).FirstOrDefault();
                String orgCode = _Organization.OrgCode;
                String orgId = _Organization.OrganizationId.ToString();
                String FileName = orgCode + orgId + Guid.NewGuid().ToString();
                return FileName;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetFileName", "UploadService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> DeleteFileSubmissionRecords()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                // int _organizationId = await _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefaultAsync();
                var _ListRecords = await _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _Organization.OrganizationId && p.FileSubmission.FileSubmissionStatu.Status != "Validated").ToListAsync();
                var _FileSubmission = await _context.FileSubmissions.Where(e => e.Organization.OrganizationId == _Organization.OrganizationId && e.FileSubmissionStatu.Status == "Uploaded").FirstOrDefaultAsync();
                //_FileSubmission.FileSubmissionStatu = await _context.FileSubmissionStatus.Where(e => e.Status == "Deleted).FirstOrDefaultAsync();

                var errorList = await _context.FileSubmissionErrors.Where(e => e.FileSubmissionRecord.FileSubmissionId == _FileSubmission.FileSubmissionId).ToListAsync();
                _context.FileSubmissionErrors.RemoveRange(errorList);
                _context.FileSubmissionRecords.RemoveRange(_ListRecords);
                _context.FileSubmissions.Remove(_FileSubmission);
                await _context.SaveChangesAsync();

                return new ResponseModel { Message = "Delete success", Succeeded = true, Id = 1 };
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "DeleteFileSubmissionRecords", "UploadService.cs");
                throw;
            }
        }
        private DateTime FormateDate(String value)
        {
            try
            {
                DateTime _result = Convert.ToDateTime(value);
                return _result;
            }
            catch
            {
                try
                {


                    Int32 Year = Convert.ToInt32(value.Substring(value.Length - 4, 4));
                    Int32 Day = Convert.ToInt32(value.Substring(value.Length - 6, 2));
                    Int32 Month = Convert.ToInt32(value.Substring(0, value.Length - 6));
                    DateTime _result = new DateTime(Year, Month, Day);
                    return _result;
                }
                catch
                {

                    return DateTime.Now;
                }
            }

        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                if (emailaddress.Length > 0)
                {
                    MailAddress m = new MailAddress(emailaddress);
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public ResponseModel ValidateEmployeeRecords()
        {

            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            int _user = Convert.ToInt32(_Loginmodel.UserId);

            var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
            Organization _Organization = new Organization();
            if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
            {
                int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
            }
            else
            {
                if (User.UserRole != null)
                {
                    _Organization = User.UserRole.Organization;
                }
            }

            int _organizationId = _Organization.OrganizationId;
            String _OrgCode = _Organization.OrgCode;
            var _ListRecords = _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _organizationId && p.FileSubmission.FileSubmissionStatu.Status != "Validated").ToList();
            var _FileSubmission = _ListRecords.Select(e => e.FileSubmission).FirstOrDefault();


            List<FileSubmissionError> ListSubmissionError = new List<FileSubmissionError>();
            int listCount = _ListRecords.Count();
            try
            {
                #region AllFields
                #region FirstName              
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.FirstName) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.FirstName) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : FirstName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region LastName    
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.LastName) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.LastName) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : LastName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }

                #endregion

                #region Filled  
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Filled) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Filled) == true).ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Filled,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }

                #endregion

                #region VacancyDate              
                try
                {
                    if (listCount == _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Filled) == false).Count())
                    {
                        if (_ListRecords.Where(e => e.Filled == "0").Count() != _ListRecords.Where(e => e.Filled == "0" && string.IsNullOrWhiteSpace(e.VacancyDate) == false).Count())
                        {
                            foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.VacancyDate) == true && e.Filled == "0").ToList())
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : VacancyDate,<b> Error </b>:  Value should not be Null or Empty",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region PositionNumber             

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionNumber) == false && e.PositionNumber.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionNumber) == true && e.PositionNumber.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PositionNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region SupervisorPositionNumber                             
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.SupervisorPositionNumber) == false && e.SupervisorPositionNumber.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.SupervisorPositionNumber) == true && e.SupervisorPositionNumber.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : SupervisorPositionNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region PhoneNumber              
                try
                {
                    if (listCount != _ListRecords.Where(e => e.PhoneNumber.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => e.PhoneNumber.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PhoneNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Email                             
                try
                {
                    if (listCount != _ListRecords.Where(e => IsValid(e.Email)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => IsValid(e.Email) == false && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Email,<b> Error </b>:  invalid Email Format",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region SupervisorFlag              

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.SupervisorFlag) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.SupervisorFlag) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : SupervisorFlag,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Gender

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Gender) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Gender) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Gender,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    else
                    {
                        HashSet<string> DbGenderList = new HashSet<string>(_Organization.Genders.Select(s => s.Name.ToLower()).Distinct());
                        var CSVGenderList = _ListRecords.Where(m => !DbGenderList.Contains(m.Gender.Trim().ToLower())).ToList();
                        if (CSVGenderList.Count() > 0)
                        {
                            foreach (var item in CSVGenderList)
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : Gender,<b> Error </b>:  Respective data is not exist in the database.  ",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Race
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Race) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Race) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Race,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    else
                    {
                        HashSet<int> DbRaceList = new HashSet<int>(_Organization.Races.Select(s => s.RaceNumber).Distinct());
                        var CSVRaceList = _ListRecords.Where(m => !DbRaceList.Contains(Convert.ToInt32(m.Race))).ToList();
                        if (CSVRaceList.Count() > 0)
                        {
                            foreach (var item in CSVRaceList)
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : Race,<b> Error </b>:  Respective data is not exist in the database or not belong to your organisation.   ",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }

                #endregion

                #region National Origin (Ethnicity)             

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.NationalOrigin) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.NationalOrigin) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : National Origin (Ethnicity),<b> Error </b>:  Value should not be Null or Empty.",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Age             

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Age) == false && e.Age.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Age) == true && e.Age.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Age,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region PositionTitle              

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionTitle) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionTitle) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PositionTitle,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }

                #endregion

                #region ProgramOfficeName             
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.ProgramOfficeName) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Gender) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : ProgramOfficeName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region OPSPosition
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.OPSPosition) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.OPSPosition) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : OPSPosition,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Salary              
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Salary) == false && e.Salary.GetType() != typeof(Decimal)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.Salary) == true && e.Salary.GetType() == typeof(Decimal) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Salary,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region EEOJobCategory
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.EEOCode) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.EEOCode) == true).ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : EEOCode,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    else
                    {
                        HashSet<int> DbEEOCodeList = new HashSet<int>(_Organization.EEOJobCategories.Select(s => s.EEOJobCategoryNumber).Distinct());
                        var CSVEEOCodeList = _ListRecords.Where(m => !DbEEOCodeList.Contains(Convert.ToInt32(m.EEOCode))).ToList();
                        if (CSVEEOCodeList.Count() > 0)
                        {
                            foreach (var item in CSVEEOCodeList)
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : EEOJobCategory,<b> Error </b>:  Respective data is not exist in the database or not belong to your organisation.   ",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region LastPerformanceRating              
                try
                {
                    if (listCount != _ListRecords.Where(e => e.LastPerformanceRating.GetType() != typeof(Decimal)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => e.LastPerformanceRating.GetType() == typeof(Decimal) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : LastPerformanceRating,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }

                #endregion

                #region PositionDateOfHire              
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionDateOfHire) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.PositionDateOfHire) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PositionDateOfHire,<b> Error </b>:  Value should not be Null or Empty. ",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region AgencyDateOfHire              
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.AgencyDateOfHire) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.AgencyDateOfHire) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : AgencyDateOfHire,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region StateCumulativeMonthsOfService             

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.StateCumulativeMonthsOfService) == false && e.StateCumulativeMonthsOfService.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.StateCumulativeMonthsOfService) == true && e.StateCumulativeMonthsOfService.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : StateCumulativeMonthsOfService,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region UserID             

                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.UserID) == false && e.UserID.GetType() != typeof(Int64)).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.UserID) == true && e.UserID.GetType() == typeof(Int64) && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : UserID,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region RegionName              
                try
                {
                    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.RegionName) == false).Count())
                    {
                        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.RegionName) == true && e.Filled == "1").ToList())
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : RegionName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region comment   
                //#region WorkAddress              
                //try
                //{
                //    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkAddress) == false).Count())
                //    {
                //        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkAddress) == true).ToList())
                //        {
                //            FileSubmissionError SubmissionError = new FileSubmissionError
                //            {
                //                FileSubmissionRecord = item,
                //                ErrorDescription = "<b>Column Name</b> : Work Address,<b> Error </b>:  Value should not be Null or Empty",
                //                CreateUserId = _user,
                //                CreateDateTime = DateTime.Now,
                //                UpdateUserId = _user,
                //                UpdateDateTime = DateTime.Now
                //            };
                //            ListSubmissionError.Add(SubmissionError);
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}
                //#endregion
                //#region WorkCity              
                //try
                //{
                //    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkCity) == false).Count())
                //    {
                //        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkCity) == true).ToList())
                //        {
                //            FileSubmissionError SubmissionError = new FileSubmissionError
                //            {
                //                FileSubmissionRecord = item,
                //                ErrorDescription = "<b>Column Name</b> : Work City,<b> Error </b>:  Value should not be Null or Empty",
                //                CreateUserId = _user,
                //                CreateDateTime = DateTime.Now,
                //                UpdateUserId = _user,
                //                UpdateDateTime = DateTime.Now
                //            };
                //            ListSubmissionError.Add(SubmissionError);
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}

                //#endregion
                //#region State
                //try
                //{
                //    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkState) == false).Count())
                //    {
                //        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkState) == true).ToList())
                //        {
                //            FileSubmissionError SubmissionError = new FileSubmissionError
                //            {
                //                FileSubmissionRecord = item,
                //                ErrorDescription = "<b>Column Name</b> : State,<b> Error </b>:  Value should not be Null or Empty",
                //                CreateUserId = _user,
                //                CreateDateTime = DateTime.Now,
                //                UpdateUserId = _user,
                //                UpdateDateTime = DateTime.Now
                //            };
                //            ListSubmissionError.Add(SubmissionError);
                //        }
                //    }
                //    else
                //    {
                //        HashSet<int> DbWorkStateNameList = new HashSet<int>(_context.States.Select(s => s.StateId).Distinct());
                //        var CSVWorkStateNameList = _ListRecords.Where(m => !DbWorkStateNameList.Contains(Convert.ToInt32(m.WorkState))).ToList();
                //        if (CSVWorkStateNameList.Count() > 0)
                //        {
                //            foreach (var item in CSVWorkStateNameList)
                //            {
                //                FileSubmissionError SubmissionError = new FileSubmissionError
                //                {
                //                    FileSubmissionRecord = item,
                //                    ErrorDescription = "<b>Column Name</b> : State,<b> Error </b>:  Respective data is not exist in the database.   ",
                //                    CreateUserId = _user,
                //                    CreateDateTime = DateTime.Now,
                //                    UpdateUserId = _user,
                //                    UpdateDateTime = DateTime.Now
                //                };
                //                ListSubmissionError.Add(SubmissionError);
                //            }
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}

                //#endregion
                //#region WorkZipCode              
                //try
                //{
                //    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkZipCode) == false && e.WorkZipCode.GetType() != typeof(Int64)).Count())
                //    {
                //        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkZipCode) == true && e.WorkZipCode.GetType() == typeof(Int64)).ToList())
                //        {
                //            FileSubmissionError SubmissionError = new FileSubmissionError
                //            {
                //                FileSubmissionRecord = item,
                //                ErrorDescription = "<b>Column Name</b> : WorkZipCode,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                //                CreateUserId = _user,
                //                CreateDateTime = DateTime.Now,
                //                UpdateUserId = _user,
                //                UpdateDateTime = DateTime.Now
                //            };
                //            ListSubmissionError.Add(SubmissionError);
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}

                //#endregion
                //#region County
                //try
                //{
                //    if (listCount != _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkCounty) == false).Count())
                //    {
                //        foreach (var item in _ListRecords.Where(e => string.IsNullOrWhiteSpace(e.WorkCounty) == true).ToList())
                //        {
                //            FileSubmissionError SubmissionError = new FileSubmissionError
                //            {
                //                FileSubmissionRecord = item,
                //                ErrorDescription = "<b>Column Name</b> : Work County,<b> Error </b>:  Value should not be Null or Empty",
                //                CreateUserId = _user,
                //                CreateDateTime = DateTime.Now,
                //                UpdateUserId = _user,
                //                UpdateDateTime = DateTime.Now
                //            };
                //            ListSubmissionError.Add(SubmissionError);
                //        }
                //    }
                //    else
                //    {
                //        HashSet<int> DbWorkCountyList = new HashSet<int>(_context.Counties.Select(s => s.CountyId).Distinct());
                //        var CSVWorkCountyList = _ListRecords.Where(m => !DbWorkCountyList.Contains(Convert.ToInt32(m.WorkCounty))).ToList();
                //        if (CSVWorkCountyList.Count() > 0)
                //        {
                //            foreach (var item in CSVWorkCountyList)
                //            {
                //                FileSubmissionError SubmissionError = new FileSubmissionError
                //                {
                //                    FileSubmissionRecord = item,
                //                    ErrorDescription = "<b>Column Name</b> : Work County,<b> Error </b>:  Respective data is not exist in the database.   ",
                //                    CreateUserId = _user,
                //                    CreateDateTime = DateTime.Now,
                //                    UpdateUserId = _user,
                //                    UpdateDateTime = DateTime.Now
                //                };
                //                ListSubmissionError.Add(SubmissionError);
                //            }
                //        }
                //    }
                //}
                //catch (Exception)
                //{

                //}

                //#endregion
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                FileSubmissionError SubmissionError = new FileSubmissionError
                {
                    FileSubmissionRecord = null,
                    ErrorDescription = ex.Message.ToString(),
                    CreateUserId = _user,
                    CreateDateTime = DateTime.Now,
                    UpdateUserId = _user,
                    UpdateDateTime = DateTime.Now
                };
                ListSubmissionError.Add(SubmissionError);
            }

            if (ListSubmissionError.Count() == 0)
            {
                try
                {
                    //get a list of records and filter by the organization ID that is selected and uploading to database.
                    var dbState = _context.States.ToList();
                    var dbCounty = _context.Counties.ToList();
                    var dbGender = _context.Genders.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbRace = _context.Races.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbLastPerformanceRatings = _context.LastPerformanceRatings.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbEEOJobCategory = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbAgencyYearsOfService = _context.AgencyYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbPositionYearsOfService = _context.PositionYearsOfServices.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbAgeRanges = _context.AgeRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbSalaryRanges = _context.SalaryRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToList();
                    var dbVacancyRanges = _context.VacancyRanges.Where(e => e.Organization.OrganizationId == _organizationId).ToList();

                    var ListEmpModel = _ListRecords.Select(e => new Employee
                    {
                        Organization = _Organization,
                        FileSubmission = _FileSubmission,
                        FirstName = e.FirstName,
                        MiddleName = e.MiddleName,
                        LastName = e.LastName,
                        Filled = e.Filled == "0" ? false : true,
                        PositionNumber = e.PositionNumber,
                        SupervisorPositionNumber = e.SupervisorPositionNumber,
                        PhoneNumber = e.PhoneNumber,
                        Email = e.Email,
                        NationalOrigin = e.NationalOrigin == "1" ? "Hispanic or Latino" : "Not Hispanic or Latino",
                        OPSPosition = e.OPSPosition == "0" ? false : true,
                        UserID = e.Filled == "1" ? Convert.ToInt32(e.UserID) : 0,
                        SupervisorFlag = e.SupervisorFlag == "0" ? false : true,
                        PositionTitle = e.PositionTitle,
                        ProgramOfficeName = e.ProgramOfficeName,
                        WorkAddress = e.WorkAddress,
                        WorkCity = e.WorkCity,
                        WorkZipCode = e.WorkZipCode,
                        CreateUserId = _user,
                        CreateDateTime = DateTime.Now,
                        UpdateUserId = _user,
                        UpdateDateTime = DateTime.Now,
                        RegionName = e.RegionName,
                        County = e.Filled == "1" ? dbCounty.Where(e1 => e1.Name.Trim().ToLower() == e.WorkCounty.Trim().ToLower()).FirstOrDefault() : null,
                        State = e.Filled == "1" ? dbState.Where(e1 => e1.Name.Trim().ToLower() == e.WorkState.Trim().ToLower()).FirstOrDefault() : null,
                        Gender = e.Filled == "1" ? dbGender.Where(e1 => e1.GenderId == _Organization.Genders.Where(e2 => e2.Name.Trim().ToLower() == e.Gender.Trim().ToLower()).Select(e3 => e3.GenderId).FirstOrDefault()).FirstOrDefault() : null,
                        Race = e.Filled == "1" ? dbRace.Where(e1 => e1.RaceNumber == Convert.ToInt32(e.Race) && e1.Organization.OrganizationId == _organizationId).FirstOrDefault() : null,
                        EEOJobCategory = dbEEOJobCategory.Where(e1 => e1.EEOJobCategoryNumber == Convert.ToInt32(e.EEOCode) && e1.Organization.OrganizationId == _organizationId).FirstOrDefault(),
                        PositionYearsOfService = e.Filled == "1" ? dbPositionYearsOfService.Where(e1 => e1.MaxValue >= Convert.ToDecimal((DateTime.Now - FormateDate(e.PositionDateOfHire)).TotalDays / 365) && e1.MinValue <= Convert.ToDecimal((DateTime.Now - FormateDate(e.PositionDateOfHire)).TotalDays / 365)).FirstOrDefault() : null,
                        AgencyYearsOfService = e.Filled == "1" ? dbAgencyYearsOfService.Where(e1 => e1.MaxValue >= Convert.ToDecimal((DateTime.Now - FormateDate(e.AgencyDateOfHire)).TotalDays / 365) && e1.MinValue <= Convert.ToDecimal((DateTime.Now - FormateDate(e.AgencyDateOfHire)).TotalDays / 365)).FirstOrDefault() : null,
                        SalaryRange = e.Filled == "1" ? dbSalaryRanges.Where(e1 => e1.MaxValue >= ConvertToDecimal(e.Salary) && e1.MinValue <= ConvertToDecimal(e.Salary)).FirstOrDefault() : null,
                        AgeRange = e.Filled == "1" ? dbAgeRanges.Where(e1 => e1.MaxValue >= ConvertToDecimal(e.Age) && e1.MinValue <= ConvertToDecimal(e.Age)).FirstOrDefault() : null,
                        Salary = e.Filled == "1" ? ConvertToDecimal(e.Salary) : 0,
                        LastPerformanceRating = e.Filled == "1" ? e.LastPerformanceRating.Length == 0 ? dbLastPerformanceRatings.Where(e1 => e1.MaxValue == -1).FirstOrDefault() : dbLastPerformanceRatings.Where(e1 => e1.MaxValue >= ConvertToDecimal(e.LastPerformanceRating) && e1.MinValue <= ConvertToDecimal(e.LastPerformanceRating)).FirstOrDefault() : null,
                        LastPerformanceRatingValue = e.Filled == "1" ? e.LastPerformanceRating.Length == 0 ? -1 : ConvertToDecimal(e.LastPerformanceRating) : 0,
                        StateCumulativeMonthsOfService = e.Filled == "1" ? Convert.ToInt32(e.StateCumulativeMonthsOfService) : 0,
                        AgencyDateOfHire = e.Filled == "1" ? FormateDate(e.AgencyDateOfHire) : new DateTime(1900, 01, 01),
                        VacancyDate = e.Filled == "0" ? FormateDate(e.VacancyDate) : new DateTime(1900, 01, 01),
                        VacancyRange = e.Filled == "0" ? dbVacancyRanges.Where(e1 => e1.MaxValue >= DateTime.Now.Subtract(FormateDate(e.VacancyDate)).Days && e1.MinValue <= DateTime.Now.Subtract(FormateDate(e.VacancyDate)).Days).FirstOrDefault() : dbVacancyRanges.Where(e1 => e1.Number == 1).FirstOrDefault(),
                        Age = e.Filled == "1" ? Convert.ToInt32(e.Age) : 0,
                        PositionDateOfHire = e.Filled == "1" ? FormateDate(e.PositionDateOfHire) : new DateTime(1900, 01, 01)

                    }).ToList();

                    _context.Employees.AddRange(ListEmpModel);
                    #region Generate CSV Section 
                    String FileName = _OrgCode + "_" + _FileSubmission.FileVersionNumber + "_" + _FileSubmission.SubmissionDateTime.Value.Month + "_" + _FileSubmission.SubmissionDateTime.Value.Day + "_" + _FileSubmission.SubmissionDateTime.Value.Year;
                    GenerateCSV _insertGenerateCSV = new GenerateCSV
                    {
                        FileName = FileName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _user,
                        FileSubmission = _FileSubmission,
                        Organization = _Organization,
                        Status = false
                    };
                    _context.GenerateCSVs.Add(_insertGenerateCSV);
                    _FileSubmission.FileSubmissionStatu = _context.FileSubmissionStatus.Where(e => e.Status == "Validated").FirstOrDefault();
                    #endregion
                    _context.SaveChanges();

                    // insert data into Employee level table
                    GenerateEmployeeLevel(_organizationId, _FileSubmission.FileSubmissionId);
                    DBCreateEEORatingMetrics(_organizationId, _FileSubmission.FileSubmissionId);
                    AppUtility.SendValidateEmployeeNotification(_insertGenerateCSV.FileName);
                }
                catch (Exception ex)
                {
                    AppUtility.LogMessage(ex, "ValidateEmployeeRecords", "UploadService.cs");
                    throw;
                }

                return new ResponseModel { Message = "All records are validate successfully and saved in the employee database", Succeeded = true, Id = 1 };
            }
            else
            {
                try
                {
                    foreach (var _ItemSubmissionError in ListSubmissionError)
                    {
                        var FileRecord = _ItemSubmissionError.FileSubmissionRecord;
                        FileRecord.IsValidate = false;
                    }
                    _context.FileSubmissionErrors.AddRange(ListSubmissionError);
                    _context.SaveChanges();
                }
                catch
                {

                }

                return new ResponseModel { Message = "Following records not validated, please review and fix the data before validate", Succeeded = false, Id = 0 };
            }


        }
        public AgencyYearsOfService GetValue(string AgencyDateofHire)
        {
            return null;
        }
        public ResponseModel ValidateEmployeeRecordsOld()
        {
            LoginResponse _Loginmodel = AppUtility.DecryptCookie();
            int _user = Convert.ToInt32(_Loginmodel.UserId);
            var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
            Organization _Organization = new Organization();
            if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
            {
                int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
            }
            else
            {
                if (User.UserRole != null)
                {
                    _Organization = User.UserRole.Organization;
                }
            }
            // var _organization = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization).FirstOrDefault();
            int _organizationId = _Organization.OrganizationId;
            string _OrgCode = _Organization.OrgCode;
            var _ListRecords = _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _organizationId && p.FileSubmission.FileSubmissionStatu.Status != "Validated").ToList();
            var _FileSubmission = _ListRecords.Select(e => e.FileSubmission).FirstOrDefault();

            List<Employee> ListEmpModel = new List<Employee>();
            List<FileSubmissionError> ListSubmissionError = new List<FileSubmissionError>();


            foreach (var item in _ListRecords)
            {
                try
                {
                    //Int32 RaceId = Convert.ToInt32(item.Race);
                    //Int32 EEOCodeId = Convert.ToInt32(item.EEOCode);
                    // Int32 WorkStateId = Convert.ToInt32(item.WorkStateName);
                    // Int32 CountyId = Convert.ToInt32(item.WorkCounty);
                    // String _gender = item.Gender == "1" ? "Male" : "Female";
                    // Int32 GenderId = _organization.Genders.Where(e => e.Name == (_gender)).Select(e => e.GenderId).FirstOrDefault();
                    Employee EmpModel = new Employee();
                    EmpModel.Organization = item.FileSubmission.Organization;
                    EmpModel.FileSubmission = item.FileSubmission;
                    #region AllFields

                    #region FirstName              
                    try
                    {
                        if (item.FirstName != null && item.FirstName.Length > 0)
                        {
                            EmpModel.FirstName = item.FirstName;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : FirstName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : FirstName,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    EmpModel.MiddleName = item.MiddleName;
                    #region LastName              
                    try
                    {
                        if (item.LastName != null && item.LastName.Length > 0)
                        {
                            EmpModel.LastName = item.LastName;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : LastName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : LastName,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region Vacant              
                    try
                    {
                        if (item.Filled != null && item.Filled.Length > 0)
                        {
                            EmpModel.Filled = item.Filled == "0" ? false : true;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Vacant,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : Vacant,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region PositionNumber              
                    try
                    {
                        if (item.PositionNumber != null && item.PositionNumber.Length > 0 && Int64.TryParse(item.PositionNumber, out var _PositionNumber))
                        {
                            EmpModel.PositionNumber = item.PositionNumber;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PositionNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PositionNumber,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region SupervisorPositionNumber              
                    try
                    {
                        if (item.SupervisorPositionNumber != null && item.SupervisorPositionNumber.Length > 0 && Int64.TryParse(item.SupervisorPositionNumber, out var _SupervisorPositionNumber))
                        {
                            EmpModel.SupervisorPositionNumber = item.SupervisorPositionNumber;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : SupervisorPositionNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : SupervisorPositionNumber,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region PhoneNumber              
                    try
                    {
                        if (item.PhoneNumber != null && item.PhoneNumber.Length > 0 && Int64.TryParse(item.PhoneNumber, out var _PhoneNumber))
                        {
                            EmpModel.PhoneNumber = item.PhoneNumber;
                        }
                        else
                        {
                            if (item.PhoneNumber == null)
                            {
                                EmpModel.PhoneNumber = item.PhoneNumber;
                            }
                            else
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : PhoneNumber,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PhoneNumber,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region Email              
                    try
                    {
                        if (item.Email != null && item.Email.Length > 0 && IsValid(item.Email))
                        {
                            EmpModel.Email = item.Email;
                        }
                        else
                        {
                            if (item.Email == null)
                            {
                                EmpModel.Email = item.Email;
                            }
                            else
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : Email,<b> Error </b>:  invalid Email Format",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : Email,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    //EmpModel.PicturePath = item.PicturePath;
                    #region SupervisorFlag              
                    try
                    {
                        if (item.SupervisorFlag != null && item.SupervisorFlag.Length > 0)
                        {
                            EmpModel.SupervisorFlag = item.SupervisorFlag == "0" ? false : true;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : SupervisorFlag,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : SupervisorFlag,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region Gender
                    try
                    {
                        String _gender = item.Gender == "1" ? "Male" : "Female";
                        Int32 GenderId = _Organization.Genders.Where(e => e.Name == (_gender)).Select(e => e.GenderId).FirstOrDefault();
                        EmpModel.Gender = _context.Genders.Where(e => e.GenderId == GenderId).First();
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : Gender,<b> Error </b>:  Respective data is not exist in the database.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region Race
                    try
                    {
                        Int32 RaceId = Convert.ToInt32(item.Race);
                        EmpModel.Race = _context.Races.Where(e => e.RaceNumber == RaceId && e.Organization.OrganizationId == _organizationId).First();
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : Race,<b> Error </b>:  Respective data is not exist in the database or not belong to your organisation.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region DateOfBirth
                    try
                    {
                        EmpModel.Age = Convert.ToInt32(item.Age);// FormateDate(item.DateOfBirth);
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : DateOfBirth,<b> Error </b>:  Invalid Date format.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region PositionTitle              
                    try
                    {
                        if (item.PositionTitle != null && item.PositionTitle.Length > 0)
                        {
                            EmpModel.PositionTitle = item.PositionTitle;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : PositionTitle,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PositionTitle,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region ProgramOfficeName              
                    try
                    {
                        if (item.ProgramOfficeName != null && item.ProgramOfficeName.Length > 0)
                        {
                            EmpModel.ProgramOfficeName = item.ProgramOfficeName;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : ProgramOfficeName,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : ProgramOfficeName,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region Salary              
                    try
                    {
                        if (item.Salary != null && item.Salary.Length > 0 && Decimal.TryParse(item.Salary, out var _Salary))
                        {
                            EmpModel.Salary = Convert.ToDecimal(item.Salary);
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : Salary,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);

                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : Salary,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region EEOJobCategory
                    try
                    {
                        Int32 EEOCodeId = Convert.ToInt32(item.EEOCode);
                        EmpModel.EEOJobCategory = _context.EEOJobCategories.Where(e => e.EEOJobCategoryNumber == EEOCodeId && e.Organization.OrganizationId == _organizationId).First();
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : EEOJobCategory,<b> Error </b>:  Respective data is not exist in the database or not belong to your organisation. " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region LastPerformanceRating              
                    try
                    {
                        if (item.LastPerformanceRating != null && item.LastPerformanceRating.Length > 0 && Decimal.TryParse(item.LastPerformanceRating, out var _LastPerformanceRating))
                        {
                            // EmpModel.LastPerformanceRating = item.LastPerformanceRating;
                        }
                        else
                        {
                            if (item.LastPerformanceRating == null)
                            {
                                // EmpModel.LastPerformanceRating = item.LastPerformanceRating;
                            }
                            else
                            {
                                FileSubmissionError SubmissionError = new FileSubmissionError
                                {
                                    FileSubmissionRecord = item,
                                    ErrorDescription = "<b>Column Name</b> : LastPerformanceRating,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                    CreateUserId = _user,
                                    CreateDateTime = DateTime.Now,
                                    UpdateUserId = _user,
                                    UpdateDateTime = DateTime.Now
                                };
                                ListSubmissionError.Add(SubmissionError);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PhoneNumber,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region StateDateOfHire
                    try
                    {
                        EmpModel.StateCumulativeMonthsOfService = Convert.ToInt32(item.StateCumulativeMonthsOfService);
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : StateDateOfHire,<b> Error </b>:  Invalid Date format.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region AgencyDateOfHire
                    try
                    {
                        EmpModel.AgencyDateOfHire = FormateDate(item.AgencyDateOfHire);
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : AgencyDateOfHire,<b> Error </b>:  Invalid Date format.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    EmpModel.AgencyYearsOfService = _context.AgencyYearsOfServices.Where(e => e.AgencyYearsOfServiceId == 1).First();// not defined yet  
                    #region WorkAddress              
                    try
                    {
                        if (item.WorkAddress != null && item.WorkAddress.Length > 0)
                        {
                            EmpModel.WorkAddress = item.WorkAddress;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : WorkAddress,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PositionTitle,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region WorkCity              
                    try
                    {
                        if (item.WorkCity != null && item.WorkCity.Length > 0)
                        {
                            EmpModel.WorkCity = item.WorkCity;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : WorkCity,<b> Error </b>:  Value should not be Null or Empty",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : PositionTitle,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region State
                    try
                    {
                        Int32 WorkStateId = Convert.ToInt32(item.WorkState);
                        EmpModel.State = _context.States.Where(e => e.StateId == WorkStateId).First();
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : State,<b> Error </b>:  Respective data is not exist in the database.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region WorkZipCode              
                    try
                    {
                        if (item.WorkZipCode != null && item.WorkZipCode.Length > 0 && Int64.TryParse(item.WorkZipCode, out var _WorkZipCode))
                        {
                            EmpModel.WorkZipCode = item.WorkZipCode;
                        }
                        else
                        {
                            FileSubmissionError SubmissionError = new FileSubmissionError
                            {
                                FileSubmissionRecord = item,
                                ErrorDescription = "<b>Column Name</b> : WorkZipCode,<b> Error </b>:  Value should not be Null or Empty. It should be Numeric",
                                CreateUserId = _user,
                                CreateDateTime = DateTime.Now,
                                UpdateUserId = _user,
                                UpdateDateTime = DateTime.Now
                            };
                            ListSubmissionError.Add(SubmissionError);

                        }
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : WorkZipCode,<b> Error </b>:  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    #region County
                    try
                    {
                        Int32 CountyId = Convert.ToInt32(item.WorkCounty);
                        EmpModel.County = _context.Counties.Where(e => e.CountyId == CountyId).First();
                    }
                    catch (Exception ex)
                    {
                        FileSubmissionError SubmissionError = new FileSubmissionError
                        {
                            FileSubmissionRecord = item,
                            ErrorDescription = "<b>Column Name</b> : County,<b> Error </b>:  Respective data is not exist in the database.  " + ex.Message.ToString(),
                            CreateUserId = _user,
                            CreateDateTime = DateTime.Now,
                            UpdateUserId = _user,
                            UpdateDateTime = DateTime.Now
                        };
                        ListSubmissionError.Add(SubmissionError);
                    }
                    #endregion
                    EmpModel.CreateUserId = _user;
                    EmpModel.CreateDateTime = DateTime.Now;
                    EmpModel.UpdateUserId = _user;
                    EmpModel.UpdateDateTime = DateTime.Now;
                    ListEmpModel.Add(EmpModel);
                    item.IsValidate = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    FileSubmissionError SubmissionError = new FileSubmissionError
                    {
                        FileSubmissionRecord = item,
                        ErrorDescription = ex.Message.ToString(),
                        CreateUserId = _user,
                        CreateDateTime = DateTime.Now,
                        UpdateUserId = _user,
                        UpdateDateTime = DateTime.Now
                    };
                    ListSubmissionError.Add(SubmissionError);
                }
            }


            if (ListSubmissionError.Count() == 0)
            {
                try
                {

                    _context.Employees.AddRange(ListEmpModel);
                    #region Generate CSV Section 
                    String FileName = _OrgCode + "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
                    GenerateCSV _insertGenerateCSV = new GenerateCSV
                    {
                        FileName = FileName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = _user,
                        FileSubmission = _FileSubmission,
                        Organization = _Organization,
                        Status = false
                    };
                    _context.GenerateCSVs.Add(_insertGenerateCSV);
                    #endregion
                    _FileSubmission.FileSubmissionStatu = _context.FileSubmissionStatus.Where(e => e.Status == "Validated").FirstOrDefault();
                    _context.SaveChanges();

                    #region Generate Csv file for Dasboard 
                    //HangfireService _HangfireService = new HangfireService();
                    //_HangfireService.GenerateDashboardViaOrganizationId(_organizationId);
                    #endregion
                }
                catch
                {
                    //return new ResponseModel { Message = "Error in mapping the data with master table data, please contact administrator", Succeeded = false, Id = 0 };

                }

                return new ResponseModel { Message = "All records are validate successfully and saved in the employee database", Succeeded = true, Id = 1 };
            }
            else
            {
                try
                {
                    foreach (var _ItemSubmissionError in ListSubmissionError)
                    {
                        var FileRecord = _ItemSubmissionError.FileSubmissionRecord;
                        FileRecord.IsValidate = false;
                    }
                    _context.FileSubmissionErrors.AddRange(ListSubmissionError);
                    _context.SaveChanges();
                }
                catch
                {

                }

                return new ResponseModel { Message = "Following records not validated, please review and fix the data before validate", Succeeded = false, Id = 0 };
            }


        }
        public List<ValidateEmployeeRecords> GetNotValidateFileSubmissionRecords()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                //var _organization = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization).FirstOrDefault();
                int _organizationId = _Organization.OrganizationId;

                var _ListRecords = _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _organizationId && p.IsValidate == false).ToList();

                List<ValidateEmployeeRecords> _LstModel = new List<ValidateEmployeeRecords>();

                foreach (var item in _ListRecords)
                {
                    Int32 RaceId = Convert.ToInt32(item.Race);
                    Int32 EEOCodeId = Convert.ToInt32(item.EEOCode);
                    Int32 WorkStateId = Convert.ToInt32(item.WorkState);
                    Int32 CountyId = Convert.ToInt32(item.WorkCounty);
                    String _gender = item.Gender == "1" ? "Male" : "Female";
                    Int32 GenderId = _Organization.Genders.Where(e => e.Name == (_gender)).Select(e => e.GenderId).FirstOrDefault();
                    ValidateEmployeeRecords _ErrorEmp = new ValidateEmployeeRecords();
                    _ErrorEmp.FileSubmissionRecordNumber = item.FileSubmissionRecordNumber;
                    _ErrorEmp.FirstName = item.FirstName;
                    _ErrorEmp.MiddleName = item.MiddleName;
                    _ErrorEmp.LastName = item.LastName;
                    try
                    {
                        _ErrorEmp.Vacant = item.Filled == "0" ? false : true;
                    }
                    catch
                    {
                        _ErrorEmp.Vacant = false;
                    }

                    _ErrorEmp.PositionNumber = item.PositionNumber;
                    _ErrorEmp.SupervisorPositionNumber = item.SupervisorPositionNumber;
                    _ErrorEmp.PhoneNumber = item.PhoneNumber;
                    _ErrorEmp.Email = item.Email;
                    // _ErrorEmp.PicturePath = item.PicturePath;

                    try
                    {
                        _ErrorEmp.SupervisorFlag = item.SupervisorFlag == "0" ? false : true;
                    }
                    catch
                    {
                        _ErrorEmp.SupervisorFlag = false;
                    }
                    try
                    {
                        _ErrorEmp.GenderId = Convert.ToInt32(item.Gender);
                    }
                    catch
                    {
                        _ErrorEmp.GenderId = 1;
                    }
                    try
                    {
                        _ErrorEmp.RaceId = _context.Races.Where(e => e.RaceNumber == RaceId).Select(e => e.RaceNumber).First();
                    }
                    catch
                    {
                        _ErrorEmp.RaceId = 0;
                    }
                    try
                    {
                        _ErrorEmp.DateOfBirth = Convert.ToDateTime(item.Age);
                    }
                    catch
                    {
                        _ErrorEmp.DateOfBirth = DateTime.Now;
                    }

                    _ErrorEmp.PositionTitle = item.PositionTitle;
                    _ErrorEmp.ProgramOfficeName = item.ProgramOfficeName;

                    try
                    {
                        _ErrorEmp.Salary = Convert.ToDecimal(item.Salary);
                    }
                    catch
                    {
                        _ErrorEmp.Salary = 0;
                    }

                    try
                    {
                        _ErrorEmp.EEOCodeId = _context.EEOJobCategories.Where(e => e.EEOJobCategoryNumber == EEOCodeId).Select(e => e.EEOJobCategoryNumber).First();
                    }
                    catch
                    {
                        _ErrorEmp.EEOCodeId = 0;
                    }

                    try
                    {
                        _ErrorEmp.LastPerformanceRating = Convert.ToInt32(item.LastPerformanceRating);
                    }
                    catch
                    {
                        _ErrorEmp.LastPerformanceRating = 0;
                    }
                    try
                    {
                        _ErrorEmp.StateDateOfHire = Convert.ToDateTime(item.StateCumulativeMonthsOfService);
                    }
                    catch
                    {
                        _ErrorEmp.StateDateOfHire = DateTime.Now;
                    }
                    try
                    {
                        _ErrorEmp.AgencyDateOfHire = Convert.ToDateTime(item.AgencyDateOfHire);
                    }
                    catch
                    {
                        _ErrorEmp.AgencyDateOfHire = DateTime.Now;
                    }
                    _ErrorEmp.WorkAddress = item.WorkAddress;
                    _ErrorEmp.WorkCity = item.WorkCity;
                    try
                    {
                        _ErrorEmp.StateId = _context.States.Where(e => e.StateId == WorkStateId).Select(e => e.StateId).First();
                    }
                    catch
                    {
                        _ErrorEmp.StateId = 0;
                    }

                    _ErrorEmp.WorkZipCode = item.WorkZipCode;

                    try
                    {
                        _ErrorEmp.CountyId = _context.Counties.Where(e => e.CountyId == CountyId).Select(e => e.CountyId).First();
                    }
                    catch
                    {
                        _ErrorEmp.CountyId = 0;

                    }
                    _LstModel.Add(_ErrorEmp);
                }
                return _LstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetNotValidateFileSubmissionRecords", "UploadService.cs");
                throw;
            }
        }
        public async Task<ResponseModel> UpdateFileSubmission(ValidateEmployeeRecords _EmployeeRecords)
        {
            try
            {
                var _FileSubmissionRecords = await _repository.FindAsync<FileSubmissionRecord>(x => x.FileSubmissionRecordNumber == _EmployeeRecords.FileSubmissionRecordNumber);
                if (_FileSubmissionRecords != null)
                {
                    //LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                    //int _user = Convert.ToInt32(_Loginmodel.UserId);
                    _FileSubmissionRecords.FirstName = _EmployeeRecords.FirstName;
                    _FileSubmissionRecords.MiddleName = _EmployeeRecords.MiddleName;
                    _FileSubmissionRecords.LastName = _EmployeeRecords.LastName;
                    _FileSubmissionRecords.Filled = _EmployeeRecords.Vacant == true ? "1" : "0";
                    _FileSubmissionRecords.PositionNumber = _EmployeeRecords.PhoneNumber;
                    _FileSubmissionRecords.SupervisorPositionNumber = _EmployeeRecords.SupervisorPositionNumber;
                    _FileSubmissionRecords.PhoneNumber = _EmployeeRecords.PhoneNumber;
                    _FileSubmissionRecords.Email = _EmployeeRecords.Email;
                    //_FileSubmissionRecords.PicturePath = _EmployeeRecords.PicturePath;
                    _FileSubmissionRecords.SupervisorFlag = _EmployeeRecords.SupervisorFlag == true ? "1" : "0";
                    _FileSubmissionRecords.Gender = _EmployeeRecords.GenderId.ToString();
                    _FileSubmissionRecords.Race = _EmployeeRecords.RaceId.ToString();
                    //_FileSubmissionRecords.DateOfBirth = _EmployeeRecords.DateOfBirth.ToShortDateString();
                    _FileSubmissionRecords.PositionTitle = _EmployeeRecords.PositionTitle;
                    _FileSubmissionRecords.ProgramOfficeName = _EmployeeRecords.ProgramOfficeName.ToString();
                    _FileSubmissionRecords.Salary = _EmployeeRecords.Salary.ToString();
                    _FileSubmissionRecords.EEOCode = _EmployeeRecords.EEOCodeId.ToString();
                    _FileSubmissionRecords.LastPerformanceRating = _EmployeeRecords.LastPerformanceRating.ToString();
                    // _FileSubmissionRecords.StateDateOfHire = _EmployeeRecords.StateDateOfHire.ToShortDateString();
                    _FileSubmissionRecords.AgencyDateOfHire = _EmployeeRecords.AgencyDateOfHire.ToShortDateString();
                    _FileSubmissionRecords.WorkAddress = _EmployeeRecords.WorkAddress;
                    _FileSubmissionRecords.WorkCity = _EmployeeRecords.WorkCity;
                    // _FileSubmissionRecords.WorkStateName = _EmployeeRecords.StateId.ToString();
                    _FileSubmissionRecords.WorkZipCode = _EmployeeRecords.WorkZipCode;
                    _FileSubmissionRecords.WorkCounty = _EmployeeRecords.CountyId.ToString();
                    _FileSubmissionRecords.IsValidate = true;
                    await _repository.SaveChangesAsync();
                    return new ResponseModel { Message = "Data successfully updated", Succeeded = true, Id = 1 };
                }
                else
                {
                    return new ResponseModel { Message = "Failure to update", Succeeded = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "UpdateFileSubmission", "UploadService.cs");
                throw;
            }
        }
        public FileVersionClass BindFileSubmissionToolbar()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }
                // var _OrganigationID = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization.OrganizationId).FirstOrDefault();
                var _data = _context.FileSubmissions.Where(e => e.Organization.OrganizationId == _Organization.OrganizationId && e.FileSubmissionStatu.Status == "Uploaded").FirstOrDefault();

                if (_data != null)
                {
                    FileVersionClass _result = new FileVersionClass
                    {

                        FileSubmissionId = _data.FileSubmissionId,
                        FileVersionNumber = _data.FileVersionNumber,
                        SubmissionDateTime = _data.SubmissionDateTime.Value.ToShortDateString()
                    };
                    return _result;
                }
                return null;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindFileSubmissionToolbar", "UploadService.cs");
                throw;
            }
        }
        public List<EmployeeErrorlist> BindEmployeeSubmissionErrorList()
        {
            try
            {
                LoginResponse _Loginmodel = AppUtility.DecryptCookie();
                int _user = Convert.ToInt32(_Loginmodel.UserId);
                var User = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).FirstOrDefault();
                Organization _Organization = new Organization();
                List<EmployeeErrorlist> _LstModel = new List<EmployeeErrorlist>();
                if (AppUtility.DecryptCookie().Roles == "DefinedSoftwareAdministrator" && AppUtility.GetOrgIdForAdminView().Length > 0)
                {
                    int org = Convert.ToInt32(AppUtility.GetOrgIdForAdminView());
                    _Organization = _context.Organizations.Where(e => e.OrganizationId == org).FirstOrDefault();
                }
                else
                {
                    if (User != null && User.UserRole != null)
                    {
                        _Organization = User.UserRole.Organization;
                    }
                }

                // var _organization = _context.Users.Where(e => e.UserId == _user && e.UserRole != null).Select(e => e.UserRole.Organization).FirstOrDefault();
                int _organizationId = _Organization.OrganizationId;
                var _ListRecords = _context.FileSubmissionRecords.Where(p => p.FileSubmission.Organization.OrganizationId == _organizationId && p.IsValidate == false && p.FileSubmission.FileSubmissionStatu.Status == "Uploaded").ToList();


                foreach (var item in _ListRecords)
                {

                    string Error = "<ol>";
                    var ErrorList = _context.FileSubmissionErrors.Where(e => e.FileSubmissionRecord.FileSubmissionRecordNumber == item.FileSubmissionRecordNumber).Select(e => e.ErrorDescription).ToList();
                    foreach (var _itemError in ErrorList)
                    {
                        Error = Error + "<li>" + _itemError + "</li>";
                    }
                    Error = Error + "</ol>";
                    EmployeeErrorlist _ErrorEmp = new EmployeeErrorlist();
                    _ErrorEmp.Name = item.LastName + " " + item.FirstName + "," + item.MiddleName;
                    _ErrorEmp.Email = item.Email;
                    _ErrorEmp.Position = item.PositionNumber;
                    _ErrorEmp.ErrorMessage = Error;
                    _LstModel.Add(_ErrorEmp);
                }

                return _LstModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "BindEmployeeSubmissionErrorList", "UploadService.cs");
                throw;
            }
        }
        private Int32 ConvertToInteger(string Value)
        {
            try
            {
                return Convert.ToInt32(Value);
            }
            catch
            {

                return 0;
            }
        }
        private Decimal ConvertToDecimal(string Value)
        {
            try
            {
                return Convert.ToDecimal(Value);
            }
            catch
            {

                return 0;
            }
        }
        private bool GenerateEmployeeLevel(int OrganosationId, int FileSubmissionId)
        {

            try
            {
                var ResultGenerateEmployeeLevel = _context.Sp_InsertEmployeeLevel(FileSubmissionId, OrganosationId);
                return true;
            }
            catch
            {

                return false;
            }
        }
        private bool DBCreateEEORatingMetrics(int OrganosationId, int FileSubmissionId)
        {

            try
            {
                var ResultCreateEEORatingMetrics = _context.CreateEEORatingMetrics(FileSubmissionId, OrganosationId);
                return true;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "DBCreateEEORatingMetrics", "UploadService.cs");
                return false;
            }
        }
    }
}
/*
Missing Index Details from SQLQuery8.sql - 10.91.0.56.definedSoftware (sa (55))
The Query Processor estimates that implementing the following index could improve the query cost by 25.4781%.
*/

/*
USE [definedSoftware]
GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
ON [dbo].[EmployeeLevel] ([PositionNumber])
INCLUDE ([EmployeeLevelNumber])
GO
*/
