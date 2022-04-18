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
    public class EEOProgressReportByRegionService : IEEOProgressReportByRegionService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEOProgressReportByRegionService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public EEOProgressReportByRegionModel GetEEOProgressReportByRegion(int? OrganizationId, int? FileSubmissionId, string region, string eeoprogramoffice, string begindate, string enddate, string supervisor)
        {
            try
            {
                DateTime StartDate = Convert.ToDateTime(begindate);
                DateTime EndDate = Convert.ToDateTime(enddate);
                EEOProgressReportByRegionModel _model = new EEOProgressReportByRegionModel();
                string OrganizationsName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationsName;
                _model.StartDate = StartDate.ToString("MMMM, dd yyyy");
                _model.EndDate = EndDate.ToString("MMMM, dd yyyy");

                var ListRacesForProgressRegion = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForProgressRegion { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForProgressRegion = new List<RacesForProgressRegion>();
                _model.ListRacesForProgressRegion.AddRange(ListRacesForProgressRegion);


                var ListEEOForProgressRegion = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForProgressRegion { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForProgressRegion = new List<EEOForProgressRegion>();
                _model.ListEEOForProgressRegion.AddRange(ListEEOForProgressRegion);
                
                List<EmployeeForProgressRegion> employeeList = new List<EmployeeForProgressRegion>();
                var fileSubmissionID = new SqlParameter("@fileSubmissionID", FileSubmissionId);
                var organizationID = new SqlParameter("@organizationID", OrganizationId);
                var supervisorID = new SqlParameter("@positionNumber", supervisor);
                var regionName = new SqlParameter("@regionName", region);
                var prgOffice = new SqlParameter("@prgOfficeName", eeoprogramoffice);
                var sstartDate = new SqlParameter("@startDate", StartDate);
                var eendDate = new SqlParameter("@endDate", EndDate);
                employeeList = _context.Database.SqlQuery<EmployeeForProgressRegion>("getEEOProgressReport @fileSubmissionID,@organizationID,@positionNumber,@regionName,@prgOfficeName,@startDate,@endDate", fileSubmissionID, organizationID, supervisorID, regionName, prgOffice, sstartDate, eendDate)
                                     .Select(e => new EmployeeForProgressRegion { 
                                         RacesId = (int)e.RacesId, 
                                         EEOId = (int)e.EEOId, 
                                         EmployeeId = (int)e.EmployeeId,
                                         BeginEnd = (int)e.BeginEnd,
                                         GenderId = (int)e.GenderId,
                                     }).ToList();

                //var employeeBeginList = _context.Employees
                //                       .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                //                       && e.Organization.OrganizationId == OrganizationId
                //                       && e.FileSubmission.FileSubmissionId == FileSubmissionId
                //                       && e.RegionName == (region.Length > 0 ? region : e.RegionName)
                //                       && e.ProgramOfficeName == (eeoprogramoffice.Length > 0 ? eeoprogramoffice : e.ProgramOfficeName)
                //                       && e.PositionNumber == (supervisor.Length > 0 ? supervisor : e.PositionNumber)
                //                        && e.PositionDateOfHire <= StartDate
                //                       )
                //                       .Select(e => new EmployeeForProgressRegion
                //                       {
                //                           RacesId = e.Race.RaceId,
                //                           EEOId = e.EEOJobCategory.EEOJobCategoryId,
                //                           EmployeeId = e.EmployeeId,
                //                           GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2,
                //                           BeginEnd = 1
                //                       }).ToList();
                //employeeList.AddRange(employeeBeginList);


                //var employeeEndList = _context.Employees
                //                       .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                //                       && e.Organization.OrganizationId == OrganizationId
                //                       && e.FileSubmission.FileSubmissionId == FileSubmissionId
                //                       && e.RegionName == (region.Length > 0 ? region : e.RegionName)                                       
                //                       && e.ProgramOfficeName == (eeoprogramoffice.Length > 0 ? eeoprogramoffice : e.ProgramOfficeName)
                //                       && e.PositionNumber == (supervisor.Length > 0 ? supervisor : e.PositionNumber)
                //                       //&& e.PositionDateOfHire<= StartDate
                //                       && e.PositionDateOfHire <= EndDate
                //                       )
                //                       .Select(e => new EmployeeForProgressRegion
                //                       {
                //                           RacesId = e.Race.RaceId,
                //                           EEOId = e.EEOJobCategory.EEOJobCategoryId,
                //                           EmployeeId = e.EmployeeId,
                //                           GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2,
                //                           BeginEnd = 2
                //                       }).ToList();

                //employeeList.AddRange(employeeEndList);
                var TotalEmployee = from c in employeeList
                                    group c by new
                                    {
                                        c.EEOId,
                                        c.RacesId,
                                        c.GenderId,
                                        c.BeginEnd
                                    } into gcs
                                    select new TotalEmployeegForProgressRegion()
                                    {
                                        EEOId = gcs.Key.EEOId,
                                        RacesId = gcs.Key.RacesId,
                                        GenderId = gcs.Key.GenderId,
                                        BeginEnd = gcs.Key.BeginEnd,
                                        TotalEmployeeCount = gcs.Count(),
                                    };                
                List<ComputeProgressRegionValue> ListComputeProgressRegionValue = new List<ComputeProgressRegionValue>();                
                List<RegionEmployeeDifference> ListRegionEmployeeDifference = new List<RegionEmployeeDifference>();
                foreach (var ItemEEOForProgressRegion in ListEEOForProgressRegion)
                {
                    int _TotalEmployeeForPercentageBegin = TotalEmployee.Where(e => e.EEOId == ItemEEOForProgressRegion.EEOId && e.BeginEnd == 1).GroupBy(e => e.EEOId).Select(e => e.Sum(r => r.TotalEmployeeCount)).FirstOrDefault();
                    int _TotalEmployeeForPercentageEnd = TotalEmployee.Where(e => e.EEOId == ItemEEOForProgressRegion.EEOId && e.BeginEnd == 2).GroupBy(e => e.EEOId).Select(e => e.Sum(r => r.TotalEmployeeCount)).FirstOrDefault();
                    foreach (var ItemRacesForProgressRegion in ListRacesForProgressRegion)
                    {
                        ComputeProgressRegionValue _ComputeProgressBeginRegionValue = new ComputeProgressRegionValue
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            TotalEmployeeMale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 1 && e.BeginEnd == 1).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            TotalEmployeeFemale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 2 && e.BeginEnd == 1).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            BeginEnd = 1
                        };
                        if (_TotalEmployeeForPercentageBegin > 0)
                        {
                            _ComputeProgressBeginRegionValue.PercentageEmployeeMale = (_ComputeProgressBeginRegionValue.TotalEmployeeMale * 100.00) / _TotalEmployeeForPercentageBegin;
                        }
                        if (_TotalEmployeeForPercentageEnd > 0)
                        {
                            _ComputeProgressBeginRegionValue.PercentageEmployeeFemale = (_ComputeProgressBeginRegionValue.TotalEmployeeFemale * 100.00) / _TotalEmployeeForPercentageBegin;
                        }
                        ListComputeProgressRegionValue.Add(_ComputeProgressBeginRegionValue);
                        ComputeProgressRegionValue _ComputeProgressEndRegionValue = new ComputeProgressRegionValue
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            TotalEmployeeMale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 1 && e.BeginEnd == 2).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            TotalEmployeeFemale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 2 && e.BeginEnd == 2).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            BeginEnd = 2
                        };

                        if (_TotalEmployeeForPercentageBegin > 0)
                        {
                            _ComputeProgressEndRegionValue.PercentageEmployeeMale = (_ComputeProgressEndRegionValue.TotalEmployeeMale * 100.00) / _TotalEmployeeForPercentageEnd;
                        }
                        if (_TotalEmployeeForPercentageEnd > 0)
                        {
                            _ComputeProgressEndRegionValue.PercentageEmployeeFemale = (_ComputeProgressEndRegionValue.TotalEmployeeFemale * 100.00) / _TotalEmployeeForPercentageEnd;
                        }
                        ListComputeProgressRegionValue.Add(_ComputeProgressEndRegionValue);

                        RegionEmployeeDifference _RegionEmployeeDifference = new RegionEmployeeDifference
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            DifferencesMale = _ComputeProgressEndRegionValue.TotalEmployeeMale - _ComputeProgressBeginRegionValue.TotalEmployeeMale,
                            DifferencesFemale = _ComputeProgressEndRegionValue.TotalEmployeeFemale - _ComputeProgressBeginRegionValue.TotalEmployeeFemale
                        };
                        if (_ComputeProgressEndRegionValue.TotalEmployeeMale > 0)
                        {
                            _RegionEmployeeDifference.PercentageDifferenceMale = _ComputeProgressEndRegionValue.PercentageEmployeeMale - _ComputeProgressBeginRegionValue.PercentageEmployeeMale;
                        }
                        if (_ComputeProgressEndRegionValue.TotalEmployeeFemale > 0)
                        {
                            _RegionEmployeeDifference.PercentageDifferenceFemale = _ComputeProgressEndRegionValue.PercentageEmployeeFemale - _ComputeProgressBeginRegionValue.PercentageEmployeeFemale;
                        }
                        ListRegionEmployeeDifference.Add(_RegionEmployeeDifference);
                    }
                }
                _model.ListRegionEmployeeDifference = new List<RegionEmployeeDifference>();
                _model.ListRegionEmployeeDifference = ListRegionEmployeeDifference;
                _model.ListComputeProgressRegionValue = new List<ComputeProgressRegionValue>();
                _model.ListComputeProgressRegionValue = ListComputeProgressRegionValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOProgressReportByRegionService", "EEOProgressReportByRegionService.cs");
                throw;
            }
        }


        public EEOProgressReportByRegionModel GetEEOExportbyProgressRegion(int? OrganizationId, int? FileSubmissionId, string region, string eeoprogramoffice, string begindate, string enddate,string supervisor)
        {
            try
            {
                DateTime StartDate = Convert.ToDateTime(begindate);
                DateTime EndDate = Convert.ToDateTime(enddate);
                EEOProgressReportByRegionModel _model = new EEOProgressReportByRegionModel();
                string OrganizationsName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationsName;
                _model.StartDate = StartDate.ToString("MMMM, dd yyyy");
                _model.EndDate = EndDate.ToString("MMMM, dd yyyy");

                var ListRacesForProgressRegion = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForProgressRegion { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForProgressRegion = new List<RacesForProgressRegion>();
                _model.ListRacesForProgressRegion.AddRange(ListRacesForProgressRegion);


                var ListEEOForProgressRegion = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForProgressRegion { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForProgressRegion = new List<EEOForProgressRegion>();
                 
                _model.ListEEOForProgressRegion.AddRange(ListEEOForProgressRegion);
                 
                List<EmployeeForProgressRegion> employeeList = new List<EmployeeForProgressRegion>();
                var fileSubmissionID = new SqlParameter("@fileSubmissionID", FileSubmissionId);
                var organizationID = new SqlParameter("@organizationID", OrganizationId);
                var supervisorID = new SqlParameter("@positionNumber", supervisor);
                var regionName = new SqlParameter("@regionName", region);
                var prgOffice = new SqlParameter("@prgOfficeName", eeoprogramoffice);
                var sstartDate = new SqlParameter("@startDate", StartDate);
                var eendDate = new SqlParameter("@endDate", EndDate);
                employeeList = _context.Database.SqlQuery<EmployeeForProgressRegion>("getEEOProgressReport @fileSubmissionID,@organizationID,@positionNumber,@regionName,@prgOfficeName,@startDate,@endDate", fileSubmissionID, organizationID, supervisorID, regionName, prgOffice, sstartDate, eendDate)
                                    .Select(e => new EmployeeForProgressRegion
                                    {
                                        RacesId = (int)e.RacesId,
                                        EEOId = (int)e.EEOId,
                                        EmployeeId = (int)e.EmployeeId,
                                        GenderId = (int)e.GenderId,
                                        BeginEnd = (int)e.BeginEnd
                                    }).ToList();
                //var employeeBeginList = _context.Employees
                //                       .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                //                       && e.Organization.OrganizationId == OrganizationId
                //                       && e.FileSubmission.FileSubmissionId == FileSubmissionId
                //                       && e.RegionName == (region.Length > 0 ? region : e.RegionName)
                //                       && e.ProgramOfficeName == (eeoprogramoffice.Length > 0 ? eeoprogramoffice : e.ProgramOfficeName)
                //                       && e.SupervisorPositionNumber == (supervisor.Length > 0 ? supervisor : e.SupervisorPositionNumber)
                //                        && e.PositionDateOfHire <= StartDate
                //                       )
                //                       .Select(e => new EmployeeForProgressRegion
                //                       {
                //                           RacesId = e.Race.RaceId,
                //                           EEOId = e.EEOJobCategory.EEOJobCategoryId,
                //                           EmployeeId = e.EmployeeId,
                //                           GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2,
                //                           BeginEnd = 1
                //                       }).ToList();
                //employeeList.AddRange(employeeBeginList);


                //var employeeEndList = _context.Employees
                //                       .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                //                       && e.Organization.OrganizationId == OrganizationId
                //                       && e.FileSubmission.FileSubmissionId == FileSubmissionId
                //                       && e.RegionName == (region.Length > 0 ? region : e.RegionName)
                //                       && e.ProgramOfficeName == (eeoprogramoffice.Length > 0 ? eeoprogramoffice : e.ProgramOfficeName)
                //                       && e.SupervisorPositionNumber == (supervisor.Length > 0 ? supervisor : e.SupervisorPositionNumber)
                //                       //&& e.PositionDateOfHire<= StartDate
                //                       && e.PositionDateOfHire <= EndDate
                //                       )
                //                       .Select(e => new EmployeeForProgressRegion
                //                       {
                //                           RacesId = e.Race.RaceId,
                //                           EEOId = e.EEOJobCategory.EEOJobCategoryId,
                //                           EmployeeId = e.EmployeeId,
                //                           GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2,
                //                           BeginEnd = 2
                //                       }).ToList();

                //employeeList.AddRange(employeeEndList);
                var TotalEmployee = from c in employeeList
                                    group c by new
                                    {
                                        c.EEOId,
                                        c.RacesId,
                                        c.GenderId,
                                        c.BeginEnd
                                    } into gcs
                                    select new TotalEmployeegForProgressRegion()
                                    {
                                        EEOId = gcs.Key.EEOId,
                                        RacesId = gcs.Key.RacesId,
                                        GenderId = gcs.Key.GenderId,
                                        BeginEnd = gcs.Key.BeginEnd,
                                        TotalEmployeeCount = gcs.Count(),
                                    };
                List<ComputeProgressRegionValue> ListComputeProgressRegionValue = new List<ComputeProgressRegionValue>();

                List<RegionEmployeeDifference> ListRegionEmployeeDifference = new List<RegionEmployeeDifference>();
                foreach (var ItemEEOForProgressRegion in ListEEOForProgressRegion)
                {
                    int _TotalEmployeeForPercentageBegin = TotalEmployee.Where(e => e.EEOId == ItemEEOForProgressRegion.EEOId && e.BeginEnd == 1).GroupBy(e => e.EEOId).Select(e => e.Sum(r => r.TotalEmployeeCount)).FirstOrDefault();
                    int _TotalEmployeeForPercentageEnd = TotalEmployee.Where(e => e.EEOId == ItemEEOForProgressRegion.EEOId && e.BeginEnd == 2).GroupBy(e => e.EEOId).Select(e => e.Sum(r => r.TotalEmployeeCount)).FirstOrDefault();
                    foreach (var ItemRacesForProgressRegion in ListRacesForProgressRegion)
                    {
                        ComputeProgressRegionValue _ComputeProgressBeginRegionValue = new ComputeProgressRegionValue
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            TotalEmployeeMale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 1 && e.BeginEnd == 1).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            TotalEmployeeFemale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 2 && e.BeginEnd == 1).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            BeginEnd = 1
                        };
                        if (_TotalEmployeeForPercentageBegin > 0)
                        {
                            _ComputeProgressBeginRegionValue.PercentageEmployeeMale = (_ComputeProgressBeginRegionValue.TotalEmployeeMale * 100.00) / _TotalEmployeeForPercentageBegin;
                        }
                        if (_TotalEmployeeForPercentageEnd > 0)
                        {
                            _ComputeProgressBeginRegionValue.PercentageEmployeeFemale = (_ComputeProgressBeginRegionValue.TotalEmployeeFemale * 100.00) / _TotalEmployeeForPercentageBegin;
                        }
                        ListComputeProgressRegionValue.Add(_ComputeProgressBeginRegionValue);
                        ComputeProgressRegionValue _ComputeProgressEndRegionValue = new ComputeProgressRegionValue
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            TotalEmployeeMale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 1 && e.BeginEnd == 2).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            TotalEmployeeFemale = TotalEmployee.Where(e => e.RacesId == ItemRacesForProgressRegion.RacesId && e.EEOId == ItemEEOForProgressRegion.EEOId && e.GenderId == 2 && e.BeginEnd == 2).Select(e => e.TotalEmployeeCount).FirstOrDefault(),
                            BeginEnd = 2
                        };

                        if (_TotalEmployeeForPercentageBegin > 0)
                        {
                            _ComputeProgressEndRegionValue.PercentageEmployeeMale = (_ComputeProgressEndRegionValue.TotalEmployeeMale * 100.00) / _TotalEmployeeForPercentageEnd;
                        }
                        if (_TotalEmployeeForPercentageEnd > 0)
                        {
                            _ComputeProgressEndRegionValue.PercentageEmployeeFemale = (_ComputeProgressEndRegionValue.TotalEmployeeFemale * 100.00) / _TotalEmployeeForPercentageEnd;
                        }
                        ListComputeProgressRegionValue.Add(_ComputeProgressEndRegionValue);

                        RegionEmployeeDifference _RegionEmployeeDifference = new RegionEmployeeDifference
                        {
                            EEOId = ItemEEOForProgressRegion.EEOId,
                            RacesId = ItemRacesForProgressRegion.RacesId,
                            DifferencesMale = _ComputeProgressEndRegionValue.TotalEmployeeMale - _ComputeProgressBeginRegionValue.TotalEmployeeMale,
                            DifferencesFemale = _ComputeProgressEndRegionValue.TotalEmployeeFemale - _ComputeProgressBeginRegionValue.TotalEmployeeFemale
                        };
                        if (_ComputeProgressEndRegionValue.TotalEmployeeMale > 0)
                        {
                            _RegionEmployeeDifference.PercentageDifferenceMale = _ComputeProgressEndRegionValue.PercentageEmployeeMale - _ComputeProgressBeginRegionValue.PercentageEmployeeMale;
                        }
                        if (_ComputeProgressEndRegionValue.TotalEmployeeFemale > 0)
                        {
                            _RegionEmployeeDifference.PercentageDifferenceFemale = _ComputeProgressEndRegionValue.PercentageEmployeeFemale - _ComputeProgressBeginRegionValue.PercentageEmployeeFemale;
                        }
                        ListRegionEmployeeDifference.Add(_RegionEmployeeDifference);
                    }
                }
                _model.ListRegionEmployeeDifference = new List<RegionEmployeeDifference>();
                _model.ListRegionEmployeeDifference = ListRegionEmployeeDifference;
                _model.ListComputeProgressRegionValue = new List<ComputeProgressRegionValue>();
                _model.ListComputeProgressRegionValue = ListComputeProgressRegionValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOProgressReportByRegionService", "EEOProgressReportByRegionService.cs");
                throw;
            }
        }

        public List<SelectListItem> BindEmployeeProgressRegionDropDown(int? OrganizationId, int? FileSubmissionId)
        {
            try
            {
                var _RegionList = _context.Employees.Where(e => e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId)
                                 .Select(e => e.RegionName).Distinct()
                                 .OrderBy(e => e).ToList();
                var _ListEEORegion = new List<SelectListItem>();
                _ListEEORegion.AddRange(_RegionList.Select(g => new SelectListItem { Text = g.ToString(), Value = g.ToString() }).ToList());
                return _ListEEORegion;
            }
            catch (Exception ex)
            {

                AppUtility.LogMessage(ex, "BindEmployeeRegionDropDown", "EEOProgressReportByRegionService.cs");
                throw;
            }

        }

    }
}
