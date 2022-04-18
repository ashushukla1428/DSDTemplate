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
    public class JobsByEEOCategoryReportService : IJobsByEEOCategoryReportService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public JobsByEEOCategoryReportService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public JobsByEEOCategoryReportModel GetJobsByEEOCategoryReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice, string region)
        {
            try
            {
                JobsByEEOCategoryReportModel _model = new JobsByEEOCategoryReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var _rptJobsByEEOCategory = _context.rptJobTitlesByEEOCategory(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, region.Length > 0 ? region : null).ToList();

                var listofJobsCategory = _rptJobsByEEOCategory.Select(e => new EEOJobsCategory { EEOJobCategoryNumber = e.EEOJobCategoryNumber, EEOJobCategoryName = e.EEOCategoryName }).ToList()
                                .GroupBy(e => new { e.EEOJobCategoryNumber, e.EEOJobCategoryName }).Select(e => new EEOJobsCategory { EEOJobCategoryName = e.Key.EEOJobCategoryName, EEOJobCategoryNumber = e.Key.EEOJobCategoryNumber }).ToList();
                _model.ListJobsByEEOCategory = listofJobsCategory;
                _model.ListPositionAndProgram = _rptJobsByEEOCategory.GroupBy(e => new { e.EEOJobCategoryNumber, e.PositionTitle }).Select(e => new PositionAndProgram
                {
                    EEOJobCategoryNumber = e.Key.EEOJobCategoryNumber,
                    PositionTitle = e.Key.PositionTitle,
                    ProgramOfficeName = e.Select(x => x.ProgramOfficeName).ToList(),

                }).Distinct().ToList();

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetJobsByEEOCategoryReportService", "JobsByEEOCategoryReportService.cs");
                throw;
            }
        }
        public JobsByEEOCategoryReportModel GetJobsByEEOCategoryExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice, string region)
        {
            try
            {
                JobsByEEOCategoryReportModel _model = new JobsByEEOCategoryReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var _rptJobsByEEOCategory = _context.rptJobTitlesByEEOCategory(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, region.Length > 0 ? region : null).ToList();

                var listofJobsCategory = _rptJobsByEEOCategory.Select(e => new EEOJobsCategory { EEOJobCategoryNumber = e.EEOJobCategoryNumber, EEOJobCategoryName = e.EEOCategoryName }).ToList()
                                .GroupBy(e => new { e.EEOJobCategoryNumber, e.EEOJobCategoryName }).Select(e => new EEOJobsCategory { EEOJobCategoryName = e.Key.EEOJobCategoryName, EEOJobCategoryNumber = e.Key.EEOJobCategoryNumber }).ToList();
                _model.ListJobsByEEOCategory = listofJobsCategory;
                _model.ListPositionAndProgram = _rptJobsByEEOCategory.GroupBy(e=> new { e.EEOJobCategoryNumber, e.PositionTitle}).Select(e => new PositionAndProgram
                {
                    EEOJobCategoryNumber = e.Key.EEOJobCategoryNumber,
                    PositionTitle = e.Key.PositionTitle,
                    ProgramOfficeName = e.Select(x=>x.ProgramOfficeName).ToList(),

                }).Distinct().ToList();

                return _model; 
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetJobsByEEOCategoryReportService", "JobsByEEOCategoryReportService.cs");
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

                AppUtility.LogMessage(ex, "BindEEOJobCategoryDropDown", "JobsByEEOCategoryReportService.cs");
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

                AppUtility.LogMessage(ex, "BindEEOProgramOfficeDropDown", "JobsByEEOCategoryReportService.cs");
                throw;
            }
        }

    }
}
