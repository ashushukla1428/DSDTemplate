using EEONow.Interfaces;
using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEONow.Context;
using EEONow.Utilities;
using System.Configuration;
using System.Web.Mvc;
using System.Web;
using EEONow.Context.EntityContext;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.Entity;

using System.Web.Routing;
//using System.Web.Security;

namespace EEONow.Services
{
    public class EEOCompensationReportService : IEEOCompensationReportService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEOCompensationReportService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public EEOCompensationReportModel GetEEOCompensationReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle,string region)
        {
            try
            {
                EEOCompensationReportModel _model = new EEOCompensationReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForEEOCompensation { RacesId = e.RaceId, RacesNumber = e.RaceNumber, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForEEOCompensation>();
                _model.ListOfRaces.AddRange(ListOfRaces);


                var _rptEEOCompensation = _context.rptEEO_GenderAndRace_CompensationReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, EEOPositionTitle.Length > 0 ? EEOPositionTitle : null, region.Length > 0 ? region : null).ToList();
                // var EmployeeList = _context.Employees.Where(e => e.Organization != null && e.Race != null && e.Gender != null && e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId).ToList();

                //  var JobTitleForEEOCompensation = EmployeeList.Where(e => e.OPSPosition == false).Select(e => new JobTitleForEEOCompensation { JobTitleName = e.PositionTitle, ProgramOffice = e.ProgramOfficeName, EEOJobCategoryId = e.EEOJobCategory.EEOJobCategoryId }).ToList();

                var JobTitleForEEOCompensation = _rptEEOCompensation.Select(e => new JobTitleForEEOCompensation { JobTitleName = e.PositionTitle, TotalEmployee = e.TotalEmployeesByTitle }).Distinct().ToList();

                //if (EEOProgramOffice.Length > 0)
                //{
                //    JobTitleForEEOCompensation = JobTitleForEEOCompensation.Where(e => e.ProgramOffice == EEOProgramOffice).ToList();
                //}

                if (EEOPositionTitle.Length > 0)
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.Where(e => e.JobTitleName == EEOPositionTitle).ToList();
                }

                //if (EEOJobCategory > 0)
                //{
                //    JobTitleForEEOCompensation = JobTitleForEEOCompensation.Where(e => e.EEOJobCategoryId == EEOJobCategory).ToList();
                //}

                JobTitleForEEOCompensation = JobTitleForEEOCompensation.GroupBy(i => i.JobTitleName, (key, group) => group.First()).ToList();

                if (jobtitlesortorder == 1)
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.OrderBy(e => e.JobTitleName).ToList();
                }
                else
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.OrderByDescending(e => e.TotalEmployee).ToList();
                }

                _model.ListJobTitleForEEOCompensation = new List<JobTitleForEEOCompensation>();
                _model.ListJobTitleForEEOCompensation.AddRange(JobTitleForEEOCompensation);

                List<EEOCompensation> _listEEOCompensation = new List<EEOCompensation>();
                List<EEOCompensationEmployeeCount> _ListEEOCompensationEmployeeCount = new List<EEOCompensationEmployeeCount>();

                foreach (var itemJobTitle in JobTitleForEEOCompensation)
                {
                    Int32 count = 0;
                    foreach (var itemRaces in ListOfRaces)
                    {
                        var itemMaleEEOCompensation = _rptEEOCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.RaceNumber == itemRaces.RacesNumber && e.GenderName.ToUpper() == "MALE").FirstOrDefault();
                        var itemFeMaleEEOCompensation = _rptEEOCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.RaceNumber == itemRaces.RacesNumber && e.GenderName.ToUpper() == "FEMALE").FirstOrDefault();

                        if (itemMaleEEOCompensation != null && itemFeMaleEEOCompensation != null)
                        {
                            EEOCompensation _EEOCompensation = new EEOCompensation
                            {
                                RacesId = itemRaces.RacesId,
                                JobTitleName = itemJobTitle.JobTitleName,
                                FTSMale = Convert.ToInt32(itemMaleEEOCompensation.TotalEmployees),
                                FTSFemale = Convert.ToInt32(itemFeMaleEEOCompensation.TotalEmployees),
                                HighMale = Convert.ToDecimal(itemMaleEEOCompensation.HighestSalary),
                                HighFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.HighestSalary),
                                MediumFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.MediumSalary),
                                MediumMale = Convert.ToDecimal(itemMaleEEOCompensation.MediumSalary),
                                LowMale = Convert.ToDecimal(itemMaleEEOCompensation.LowestSalary),
                                LowFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.LowestSalary),
                                ParityMale = Convert.ToDecimal(itemMaleEEOCompensation.ParitySalary),
                                ParityFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.ParitySalary),
                                DifferenceMale = Convert.ToDecimal(itemMaleEEOCompensation.DifferenceInSalary),
                                DifferenceFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.DifferenceInSalary),
                                PercentageMale = Convert.ToDecimal(itemMaleEEOCompensation.PercentageDifference),
                                PercentageFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.PercentageDifference)
                            };

                            _listEEOCompensation.Add(_EEOCompensation);
                            count = count + Convert.ToInt32(itemMaleEEOCompensation.TotalEmployees) + Convert.ToInt32(itemFeMaleEEOCompensation.TotalEmployees);

                        }
                    }

                    EEOCompensationEmployeeCount _EEOCompensationEmployeeCount = new EEOCompensationEmployeeCount
                    {
                        EmployeeCount = count,
                        JobTitleName = itemJobTitle.JobTitleName
                    };
                    _ListEEOCompensationEmployeeCount.Add(_EEOCompensationEmployeeCount);
                }

                _model.ListEEOCompensation = new List<EEOCompensation>();
                _model.ListEEOCompensation = _listEEOCompensation;

                _model.ListEEOCompensationEmployeeCount = new List<EEOCompensationEmployeeCount>();
                _model.ListEEOCompensationEmployeeCount = _ListEEOCompensationEmployeeCount;

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOCompensationReportService", "EEOCompensationReportService.cs");
                throw;
            }
        }
        public EEOCompensationReportModel GetEEOCompensationExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle, string region)
        {
            try
            {
                EEOCompensationReportModel _model = new EEOCompensationReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForEEOCompensation { RacesId = e.RaceId, RacesNumber = e.RaceNumber, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForEEOCompensation>();
                _model.ListOfRaces.AddRange(ListOfRaces);


                var _rptEEOCompensation = _context.rptEEO_GenderAndRace_CompensationReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, EEOPositionTitle.Length > 0 ? EEOPositionTitle : null, region.Length > 0 ? region : null).ToList();
                var JobTitleForEEOCompensation = _rptEEOCompensation.Select(e => new JobTitleForEEOCompensation { JobTitleName = e.PositionTitle, TotalEmployee = e.TotalEmployeesByTitle }).Distinct().ToList();


                if (EEOPositionTitle.Length > 0)
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.Where(e => e.JobTitleName == EEOPositionTitle).ToList();
                }
                JobTitleForEEOCompensation = JobTitleForEEOCompensation.GroupBy(i => i.JobTitleName, (key, group) => group.First()).ToList();

                if (jobtitlesortorder == 1)
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.OrderBy(e => e.JobTitleName).ToList();
                }
                else
                {
                    JobTitleForEEOCompensation = JobTitleForEEOCompensation.OrderByDescending(e => e.TotalEmployee).ToList();
                }
                _model.ListJobTitleForEEOCompensation = new List<JobTitleForEEOCompensation>();

                _model.ListJobTitleForEEOCompensation.AddRange(JobTitleForEEOCompensation);


                List<EEOCompensation> _listEEOCompensation = new List<EEOCompensation>();
                List<EEOCompensationEmployeeCount> _ListEEOCompensationEmployeeCount = new List<EEOCompensationEmployeeCount>();

                foreach (var itemJobTitle in JobTitleForEEOCompensation)
                {
                    Int32 count = 0;
                    foreach (var itemRaces in ListOfRaces)
                    {
                        var itemMaleEEOCompensation = _rptEEOCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.RaceNumber == itemRaces.RacesNumber && e.GenderName.ToUpper() == "MALE").FirstOrDefault();
                        var itemFeMaleEEOCompensation = _rptEEOCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.RaceNumber == itemRaces.RacesNumber && e.GenderName.ToUpper() == "FEMALE").FirstOrDefault();

                        if (itemMaleEEOCompensation != null && itemFeMaleEEOCompensation != null)
                        {
                            EEOCompensation _EEOCompensation = new EEOCompensation
                            {
                                RacesId = itemRaces.RacesId,
                                JobTitleName = itemJobTitle.JobTitleName,
                                FTSMale = Convert.ToInt32(itemMaleEEOCompensation.TotalEmployees),
                                FTSFemale = Convert.ToInt32(itemFeMaleEEOCompensation.TotalEmployees),
                                HighMale = Convert.ToDecimal(itemMaleEEOCompensation.HighestSalary),
                                HighFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.HighestSalary),
                                MediumFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.MediumSalary),
                                MediumMale = Convert.ToDecimal(itemMaleEEOCompensation.MediumSalary),
                                LowMale = Convert.ToDecimal(itemMaleEEOCompensation.LowestSalary),
                                LowFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.LowestSalary),
                                ParityMale = Convert.ToDecimal(itemMaleEEOCompensation.ParitySalary),
                                ParityFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.ParitySalary),
                                DifferenceMale = Convert.ToDecimal(itemMaleEEOCompensation.DifferenceInSalary),
                                DifferenceFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.DifferenceInSalary),
                                PercentageMale = Convert.ToDecimal(itemMaleEEOCompensation.PercentageDifference),
                                PercentageFemale = Convert.ToDecimal(itemFeMaleEEOCompensation.PercentageDifference)
                            };

                            _listEEOCompensation.Add(_EEOCompensation);
                            count = count + Convert.ToInt32(itemMaleEEOCompensation.TotalEmployees) + Convert.ToInt32(itemFeMaleEEOCompensation.TotalEmployees);

                        }
                    }

                    EEOCompensationEmployeeCount _EEOCompensationEmployeeCount = new EEOCompensationEmployeeCount
                    {
                        EmployeeCount = count,
                        JobTitleName = itemJobTitle.JobTitleName
                    };
                    _ListEEOCompensationEmployeeCount.Add(_EEOCompensationEmployeeCount);
                }

                _model.ListEEOCompensation = new List<EEOCompensation>();
                _model.ListEEOCompensation = _listEEOCompensation;

                _model.ListEEOCompensationEmployeeCount = new List<EEOCompensationEmployeeCount>();
                _model.ListEEOCompensationEmployeeCount = _ListEEOCompensationEmployeeCount;

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOCompensationReportService", "EEOCompensationReportService.cs");
                throw;
            }
        }
        public List<SelectListItem> BindEEOJobCategoryDropDown(int? OrganizationId)
        {
            try
            {
                var _EEOJobCategories = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).OrderBy(e => e.EEOJobCategoryNumber).ToList();
                var _ListEEOJobCategories = new List<SelectListItem>();
                _ListEEOJobCategories.AddRange(_EEOJobCategories.Select(g => new SelectListItem { Text = g.Name.ToString(), Value = g.EEOJobCategoryId.ToString() }).ToList());
                return _ListEEOJobCategories;
            }
            catch (Exception ex)
            {

                AppUtility.LogMessage(ex, "BindEEOJobCategoryDropDown", "EEOCompensationReportService.cs");
                throw;
            }

        }
        public List<SelectListItem> BindEEOProgramOfficeDropDown(Int32 OrganizationId, Int32 FileSubmissionId)
        {
            try
            {
                var _EEOProgramOffice = _context.Employees.Where(e => e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId && e.OPSPosition == false)
                                        .Select(e => e.ProgramOfficeName).OrderBy(e => e).ToList();

                _EEOProgramOffice = _EEOProgramOffice.GroupBy(i => i, (key, group) => group.First()).ToList();
                var _ListEEOProgramOffice = new List<SelectListItem>();
                _ListEEOProgramOffice.AddRange(_EEOProgramOffice.Select(g => new SelectListItem { Text = g.ToString(), Value = g.ToString() }).ToList());
                return _ListEEOProgramOffice;
            }
            catch (Exception ex)
            {

                AppUtility.LogMessage(ex, "BindEEOProgramOfficeDropDown", "EEOCompensationReportService.cs");
                throw;
            }
        }
        public List<SelectListItem> BindEEOPositionTitleDropDown(Int32 OrganizationId, Int32 FileSubmissionId, int EEOEEOJobCategoryId, string EEOProgramOffice,string region)
        {
            try
            {
                var _EEOEmployees = _context.Employees.Where(e => e.Organization.OrganizationId == OrganizationId
                                                         && e.FileSubmission.FileSubmissionId == FileSubmissionId
                                                         //&& e.EEOJobCategory.EEOJobCategoryId == EEOEEOJobCategoryId
                                                         // && e.ProgramOfficeName == EEOProgramOffice
                                                         && e.OPSPosition == false).ToList();
                //.Select(e => e.PositionTitle).OrderBy(e => e).ToList();

                if (EEOEEOJobCategoryId > 0)
                {
                    _EEOEmployees = _EEOEmployees.Where(e => e.EEOJobCategory.EEOJobCategoryId == EEOEEOJobCategoryId).ToList();
                }

                if (EEOProgramOffice.Length > 0)
                {
                    _EEOEmployees = _EEOEmployees.Where(e => e.ProgramOfficeName == EEOProgramOffice).ToList();
                }
                if(region.Length>0)
                {
                    _EEOEmployees = _EEOEmployees.Where(e => e.RegionName == region).ToList();
                }

                var _EEOPositionTitle = _EEOEmployees.Select(e => e.PositionTitle).OrderBy(e => e).ToList();
                _EEOPositionTitle = _EEOPositionTitle.GroupBy(i => i, (key, group) => group.First()).ToList();
                var _ListEEOPositionTitle = new List<SelectListItem>();
                _ListEEOPositionTitle.AddRange(_EEOPositionTitle.Select(g => new SelectListItem { Text = g.ToString(), Value = g.ToString() }).ToList());
                return _ListEEOPositionTitle;
            }
            catch (Exception ex)
            {

                AppUtility.LogMessage(ex, "BindEEOPositionTitleDropDown", "EEOCompensationReportService.cs");
                throw;
            }
        }
    }
}
