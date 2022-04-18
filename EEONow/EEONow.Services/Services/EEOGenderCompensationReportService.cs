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
    public class EEOGenderCompensationReportService : IEEOGenderCompensationReportService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEOGenderCompensationReportService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public EEOGenderCompensationReportModel GetEEOGenderCompensationReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle, string region)
        {
            try
            {
                EEOGenderCompensationReportModel _model = new EEOGenderCompensationReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;

                var _rptEEOGenderCompensation = _context.rptEEOGenderCompensationReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, EEOPositionTitle.Length > 0 ? EEOPositionTitle : null, region.Length > 0 ? region : null).ToList();
                // var EmployeeList = _context.Employees.Where(e => e.Organization != null && e.Race != null && e.Gender != null && e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId).ToList();

                //var JobTitleForEEOGenderCompensation = EmployeeList.Where(e => e.OPSPosition == false).Select(e => new JobTitleForEEOGenderCompensation { JobTitleName = e.PositionTitle, ProgramOffice = e.ProgramOfficeName, EEOJobCategoryId = e.EEOJobCategory.EEOJobCategoryId }).ToList();

                var JobTitleForEEOGenderCompensation = _rptEEOGenderCompensation.Select(e => new JobTitleForEEOGenderCompensation { JobTitleName = e.PositionTitle, TotalEmployee = e.TotalEmployeesByTitle }).ToList();

                //if (EEOProgramOffice.Length > 0)
                //{
                //    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.Where(e => e.ProgramOffice == EEOProgramOffice).ToList();
                //}

                if (EEOPositionTitle.Length > 0)
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.Where(e => e.JobTitleName == EEOPositionTitle).ToList();
                }
                //if (EEOJobCategory > 0)
                //{
                //    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.Where(e => e.EEOJobCategoryId == EEOJobCategory).ToList();
                //}
                JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.GroupBy(i => i.JobTitleName, (key, group) => group.First()).ToList();

                if (jobtitlesortorder == 1)
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.OrderBy(e => e.JobTitleName).ToList();
                }
                else
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.OrderByDescending(e => e.TotalEmployee).ToList();
                }
                _model.ListJobTitleForEEOGenderCompensation = new List<JobTitleForEEOGenderCompensation>();
                _model.ListJobTitleForEEOGenderCompensation.AddRange(JobTitleForEEOGenderCompensation);

                List<EEOGenderCompensation> _listEEOGenderCompensation = new List<EEOGenderCompensation>();
                List<EEOGenderCompensationEmployeeCount> _ListEEOGenderCompensationEmployeeCount = new List<EEOGenderCompensationEmployeeCount>();



                foreach (var itemJobTitle in JobTitleForEEOGenderCompensation)
                {
                    Int32 count = 0;

                    var itemMaleEEOGenderCompensation = _rptEEOGenderCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.GenderName.ToLower() == "male").FirstOrDefault();
                    var itemFeMaleEEOGenderCompensation = _rptEEOGenderCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.GenderName.ToLower() == "female").FirstOrDefault();

                    if (itemMaleEEOGenderCompensation != null && itemFeMaleEEOGenderCompensation != null)
                    {
                        EEOGenderCompensation _EEOGenderCompensation = new EEOGenderCompensation
                        {
                            JobTitleName = itemJobTitle.JobTitleName,
                            FTSMale = Convert.ToInt32(itemMaleEEOGenderCompensation.TotalEmployees),
                            FTSFemale = Convert.ToInt32(itemFeMaleEEOGenderCompensation.TotalEmployees),
                            HighMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.HighestSalary),
                            HighFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.HighestSalary),
                            MediumFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.MediumSalary),
                            MediumMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.MediumSalary),
                            LowMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.LowestSalary),
                            LowFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.LowestSalary),
                            ParityMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.ParitySalary),
                            ParityFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.ParitySalary),
                            DifferenceMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.DifferenceInSalary),
                            DifferenceFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.DifferenceInSalary),
                            PercentageMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.PercentageDifference),
                            PercentageFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.PercentageDifference)
                        };

                        _listEEOGenderCompensation.Add(_EEOGenderCompensation);
                        count = count + Convert.ToInt32(itemMaleEEOGenderCompensation.TotalEmployees) + Convert.ToInt32(itemFeMaleEEOGenderCompensation.TotalEmployees);

                    }


                    EEOGenderCompensationEmployeeCount _EEOGenderCompensationEmployeeCount = new EEOGenderCompensationEmployeeCount
                    {
                        EmployeeCount = count,
                        JobTitleName = itemJobTitle.JobTitleName
                    };
                    _ListEEOGenderCompensationEmployeeCount.Add(_EEOGenderCompensationEmployeeCount);
                }

                _model.ListEEOGenderCompensation = new List<EEOGenderCompensation>();
                _model.ListEEOGenderCompensation = _listEEOGenderCompensation;

                _model.ListEEOGenderCompensationEmployeeCount = new List<EEOGenderCompensationEmployeeCount>();
                _model.ListEEOGenderCompensationEmployeeCount = _ListEEOGenderCompensationEmployeeCount;

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOGenderCompensationReportService", "EEOGenderCompensationReportService.cs");
                throw;
            }
        }

        public EEOGenderCompensationReportModel GetEEOGenderCompensationExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle, string region)
        {
            try
            {
                EEOGenderCompensationReportModel _model = new EEOGenderCompensationReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;

                var _rptEEOGenderCompensation = _context.rptEEOGenderCompensationReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, EEOPositionTitle.Length > 0 ? EEOPositionTitle : null, region.Length > 0 ? region : null).ToList();

                var JobTitleForEEOGenderCompensation = _rptEEOGenderCompensation.Select(e => new JobTitleForEEOGenderCompensation { JobTitleName = e.PositionTitle, TotalEmployee = e.TotalEmployeesByTitle }).ToList();

                if (EEOPositionTitle.Length > 0)
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.Where(e => e.JobTitleName == EEOPositionTitle).ToList();
                }
                JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.GroupBy(i => i.JobTitleName, (key, group) => group.First()).ToList();

                if (jobtitlesortorder == 1)
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.OrderBy(e => e.JobTitleName).ToList();
                }
                else
                {
                    JobTitleForEEOGenderCompensation = JobTitleForEEOGenderCompensation.OrderByDescending(e => e.TotalEmployee).ToList();
                }
                _model.ListJobTitleForEEOGenderCompensation = new List<JobTitleForEEOGenderCompensation>();

                _model.ListJobTitleForEEOGenderCompensation.AddRange(JobTitleForEEOGenderCompensation);

                List<EEOGenderCompensation> _listEEOGenderCompensation = new List<EEOGenderCompensation>();
                List<EEOGenderCompensationEmployeeCount> _ListEEOGenderCompensationEmployeeCount = new List<EEOGenderCompensationEmployeeCount>();



                foreach (var itemJobTitle in JobTitleForEEOGenderCompensation)
                {
                    Int32 count = 0;

                    var itemMaleEEOGenderCompensation = _rptEEOGenderCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.GenderName.ToLower() == "male").FirstOrDefault();
                    var itemFeMaleEEOGenderCompensation = _rptEEOGenderCompensation.Where(e => e.PositionTitle == itemJobTitle.JobTitleName && e.GenderName.ToLower() == "female").FirstOrDefault();

                    if (itemMaleEEOGenderCompensation != null && itemFeMaleEEOGenderCompensation != null)
                    {
                        EEOGenderCompensation _EEOGenderCompensation = new EEOGenderCompensation
                        {
                            JobTitleName = itemJobTitle.JobTitleName,
                            FTSMale = Convert.ToInt32(itemMaleEEOGenderCompensation.TotalEmployees),
                            FTSFemale = Convert.ToInt32(itemFeMaleEEOGenderCompensation.TotalEmployees),
                            HighMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.HighestSalary),
                            HighFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.HighestSalary),
                            MediumFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.MediumSalary),
                            MediumMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.MediumSalary),
                            LowMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.LowestSalary),
                            LowFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.LowestSalary),
                            ParityMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.ParitySalary),
                            ParityFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.ParitySalary),
                            DifferenceMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.DifferenceInSalary),
                            DifferenceFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.DifferenceInSalary),
                            PercentageMale = Convert.ToDecimal(itemMaleEEOGenderCompensation.PercentageDifference),
                            PercentageFemale = Convert.ToDecimal(itemFeMaleEEOGenderCompensation.PercentageDifference)
                        };

                        _listEEOGenderCompensation.Add(_EEOGenderCompensation);
                        count = count + Convert.ToInt32(itemMaleEEOGenderCompensation.TotalEmployees) + Convert.ToInt32(itemFeMaleEEOGenderCompensation.TotalEmployees);

                    }


                    EEOGenderCompensationEmployeeCount _EEOGenderCompensationEmployeeCount = new EEOGenderCompensationEmployeeCount
                    {
                        EmployeeCount = count,
                        JobTitleName = itemJobTitle.JobTitleName
                    };
                    _ListEEOGenderCompensationEmployeeCount.Add(_EEOGenderCompensationEmployeeCount);
                }

                _model.ListEEOGenderCompensation = new List<EEOGenderCompensation>();
                _model.ListEEOGenderCompensation = _listEEOGenderCompensation;

                _model.ListEEOGenderCompensationEmployeeCount = new List<EEOGenderCompensationEmployeeCount>();
                _model.ListEEOGenderCompensationEmployeeCount = _ListEEOGenderCompensationEmployeeCount;

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOGenderCompensationReportService", "EEOGenderCompensationReportService.cs");
                throw;
            }
        }

    }
}
