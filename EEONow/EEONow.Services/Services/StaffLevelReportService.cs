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
    public class StaffLevelReportService : IStaffLevelReportService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public StaffLevelReportService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public StaffLevelReportModel GetStaffLevelReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice, string region)
        {
            try
            {
                StaffLevelReportModel _model = new StaffLevelReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForStaffLevel { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForStaffLevel>();
                _model.ListOfRaces.AddRange(ListOfRaces);
                var _rptStaffLevel = _context.rptStaffLevelsReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, region.Length > 0 ? region : null).ToList();

                var listofStaff= _rptStaffLevel.Select(e => new StaffLevel { LevelId = e.LevelNumber.Value, LevelName = e.LevelNumberDesc }).ToList()
                                .GroupBy(e => new { e.LevelId, e.LevelName }).Select(e => new StaffLevel { LevelId = e.Key.LevelId, LevelName = e.Key.LevelName }).ToList();
                _model.ListStaffLevel = listofStaff;
                _model.NumberOfStaffEmployee = _rptStaffLevel.Select(e => new NumberOfStaffEmployee
                {
                    LevelId = e.LevelNumber.Value,
                    GenderId = e.GenderId,
                    GenderName = e.Gender,
                    RacesId = e.RaceId,
                    RaceName = e.RaceName,
                    NoOfStaff = e.EmployeeNbrCount == null ? 0 : e.EmployeeNbrCount.Value
                }).Distinct().ToList();

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetStaffLevelReportService", "StaffLevelReportService.cs");
                throw;
            }
        }
        public StaffLevelReportModel GetStaffLevelExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice, string region)
        {
            try
            {
                StaffLevelReportModel _model = new StaffLevelReportModel();
                string OrganizationName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationName;
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForStaffLevel { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForStaffLevel>();
                _model.ListOfRaces.AddRange(ListOfRaces);
                var _rptStaffLevel = _context.rptStaffLevelsReport(OrganizationId, FileSubmissionId, EEOJobCategory, EEOProgramOffice.Length > 0 ? EEOProgramOffice : null, region.Length > 0 ? region : null).ToList();

                var listofStaff = _rptStaffLevel.Select(e => new StaffLevel { LevelId = e.LevelNumber.Value, LevelName = e.LevelNumberDesc }).ToList()
                                .GroupBy(e => new { e.LevelId, e.LevelName }).Select(e => new StaffLevel { LevelId = e.Key.LevelId, LevelName = e.Key.LevelName }).ToList();
                _model.ListStaffLevel = listofStaff;
                _model.NumberOfStaffEmployee = _rptStaffLevel.Select(e => new NumberOfStaffEmployee
                {
                    LevelId = e.LevelNumber.Value,
                    GenderId = e.GenderId,
                    RacesId = e.RaceId,
                    GenderName = e.Gender,
                    NoOfStaff = e.EmployeeNbrCount == null ? 0 : e.EmployeeNbrCount.Value
                }).Distinct().ToList();

                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetStaffLevelReportService", "StaffLevelReportService.cs");
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

                AppUtility.LogMessage(ex, "BindEEOJobCategoryDropDown", "StaffLevelReportService.cs");
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

                AppUtility.LogMessage(ex, "BindEEOProgramOfficeDropDown", "StaffLevelReportService.cs");
                throw;
            }
        }

    }
}
