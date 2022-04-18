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
    public class EEOReportbyRegionService : IEEOReportbyRegionService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEOReportbyRegionService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public EEOReportbyRegionModel GetEEOReportbyRegionService(int? OrganizationId, int? FileSubmissionId, string region)
        {
            try
            {

                EEOReportbyRegionModel _model = new EEOReportbyRegionModel();
                string OrganizationsName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationsName;

                var ListRacesForRegion = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForRegion { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForRegion = new List<RacesForRegion>();
                _model.ListRacesForRegion.AddRange(ListRacesForRegion);


                var ListEEOForRegion = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForRegion { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForRegion = new List<EEOForRegion>();
                _model.ListEEOForRegion.AddRange(ListEEOForRegion);


                List<EmployeeForRegion> employeeList = _context.Employees
                                                        .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                                                        && e.Organization.OrganizationId == OrganizationId
                                                        && e.FileSubmission.FileSubmissionId == FileSubmissionId
                                                        && e.RegionName == (region.Length > 0 ? region : e.RegionName))
                                                        .Select(e => new EmployeeForRegion
                                                        {
                                                            RacesId = e.Race.RaceId,
                                                            EEOId = e.EEOJobCategory.EEOJobCategoryId,
                                                            EmployeeId = e.EmployeeId,
                                                            GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2
                                                        }).ToList();

                var TotalWorkforce = from c in employeeList
                                     group c by new
                                     {
                                         c.EEOId,
                                         c.RacesId,
                                         c.GenderId,
                                     } into gcs
                                     select new EmployeeWithTotalWorkforce()
                                     {
                                         EEOId = gcs.Key.EEOId,
                                         RacesId = gcs.Key.RacesId,
                                         GenderId = gcs.Key.GenderId,
                                         TotalWorkforceCount = gcs.Count(),
                                     };

                var EmployeeWithSelectedEEO = from c in employeeList
                                              group c by new
                                              {
                                                  c.EEOId,
                                                  c.GenderId,
                                              } into gcs
                                              select new EmployeeWithSelectedEEO()
                                              {
                                                  EEOId = gcs.Key.EEOId,
                                                  GenderId = gcs.Key.GenderId,
                                                  TotalEmployeeWithSelectedEEOCount = gcs.Count(),
                                              };

                var DbAvailableLaborMarketFileVersion = _context.FileSubmissions.Where(e => e.FileSubmissionId == FileSubmissionId && e.Organization.OrganizationId == OrganizationId).Select(e => e.AvailableLaborMarketFileVersion).FirstOrDefault();

                List<CountedRegion> CountedRegionlist = DbAvailableLaborMarketFileVersion.AvailableLaborMarketDatas.Select(r => new CountedRegion { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList();

                var TotalRegion = from c in CountedRegionlist
                                  group c by new
                                  {
                                      c.EEOId,
                                      c.RacesId
                                  } into gcs
                                  select new EmployeeWithCountedRegion()
                                  {
                                      EEOId = gcs.Key.EEOId,
                                      RacesId = gcs.Key.RacesId,
                                      TotalMale = gcs.Sum(x => x.MaleCount),
                                      TotalFemale = gcs.Sum(x => x.FemaleCount)
                                  };

                var RegionWithSelectedEEO = from c in CountedRegionlist
                                            group c by new
                                            {
                                                c.EEOId
                                            } into gcs
                                            select new EmployeeWithCountedRegion()
                                            {
                                                EEOId = gcs.Key.EEOId,
                                                TotalMale = gcs.Sum(x => x.MaleCount),
                                                TotalFemale = gcs.Sum(x => x.FemaleCount)
                                            };


                List<ComputeRegionValue> ListComputeRegionValue = new List<ComputeRegionValue>();
                foreach (var ItemEEOForRegion in ListEEOForRegion)
                {
                    foreach (var ItemRacesForRegion in ListRacesForRegion)
                    {
                        ComputeRegionValue _ComputeRegionValue = new ComputeRegionValue
                        {
                            EEOId = ItemEEOForRegion.EEOId,
                            RacesId = ItemRacesForRegion.RacesId,
                            TotalWorkforceMale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForRegion.RacesId && e.EEOId == ItemEEOForRegion.EEOId && e.GenderId == 1).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForRegion.RacesId && e.EEOId == ItemEEOForRegion.EEOId && e.GenderId == 2).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalSelectedEEO = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForRegion.EEOId).Sum(e => e.TotalEmployeeWithSelectedEEOCount),
                        };
                        ListComputeRegionValue.Add(_ComputeRegionValue);
                    }
                }
                _model.ListComputeRegionValue = new List<ComputeRegionValue>();
                _model.ListComputeRegionValue = ListComputeRegionValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOReportbyRegionService", "EEOReportbyRegionService.cs");
                throw;
            }
        }


        public EEOReportbyRegionModel GetEEOExportbyRegionService(int? OrganizationId, int? FileSubmissionId, string region)
        {
            try
            {

                EEOReportbyRegionModel _model = new EEOReportbyRegionModel();
                string OrganizationsName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();
                _model.OrganizationName = OrganizationsName;

                var ListRacesForRegion = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForRegion { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForRegion = new List<RacesForRegion>();
                _model.ListRacesForRegion.AddRange(ListRacesForRegion);


                var ListEEOForRegion = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForRegion { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForRegion = new List<EEOForRegion>();

                _model.ListEEOForRegion.AddRange(ListEEOForRegion);

                List<EmployeeForRegion> employeeList = _context.Employees
                                                        .Where(e => e.Organization != null && e.Race != null && e.Gender != null
                                                        && e.Organization.OrganizationId == OrganizationId
                                                        && e.FileSubmission.FileSubmissionId == FileSubmissionId
                                                        && e.RegionName == (region.Length > 0 ? region : e.RegionName))
                                                        .Select(e => new EmployeeForRegion
                                                        {
                                                            RacesId = e.Race.RaceId,
                                                            EEOId = e.EEOJobCategory.EEOJobCategoryId,
                                                            EmployeeId = e.EmployeeId,
                                                            GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2
                                                        }).ToList();

                var TotalWorkforce = from c in employeeList
                                     group c by new
                                     {
                                         c.EEOId,
                                         c.RacesId,
                                         c.GenderId,
                                     } into gcs
                                     select new EmployeeWithTotalWorkforce()
                                     {
                                         EEOId = gcs.Key.EEOId,
                                         RacesId = gcs.Key.RacesId,
                                         GenderId = gcs.Key.GenderId,
                                         TotalWorkforceCount = gcs.Count(),
                                     };

                var EmployeeWithSelectedEEO = from c in employeeList
                                              group c by new
                                              {
                                                  c.EEOId,
                                                  c.GenderId,
                                              } into gcs
                                              select new EmployeeWithSelectedEEO()
                                              {
                                                  EEOId = gcs.Key.EEOId,
                                                  GenderId = gcs.Key.GenderId,
                                                  TotalEmployeeWithSelectedEEOCount = gcs.Count(),
                                              };

                var DbAvailableLaborMarketFileVersion = _context.FileSubmissions.Where(e => e.FileSubmissionId == FileSubmissionId && e.Organization.OrganizationId == OrganizationId).Select(e => e.AvailableLaborMarketFileVersion).FirstOrDefault();

                List<CountedRegion> CountedRegionlist = DbAvailableLaborMarketFileVersion.AvailableLaborMarketDatas.Select(r => new CountedRegion { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList();

                var TotalRegion = from c in CountedRegionlist
                                  group c by new
                                  {
                                      c.EEOId,
                                      c.RacesId
                                  } into gcs
                                  select new EmployeeWithCountedRegion()
                                  {
                                      EEOId = gcs.Key.EEOId,
                                      RacesId = gcs.Key.RacesId,
                                      TotalMale = gcs.Sum(x => x.MaleCount),
                                      TotalFemale = gcs.Sum(x => x.FemaleCount)
                                  };

                var RegionWithSelectedEEO = from c in CountedRegionlist
                                            group c by new
                                            {
                                                c.EEOId
                                            } into gcs
                                            select new EmployeeWithCountedRegion()
                                            {
                                                EEOId = gcs.Key.EEOId,
                                                TotalMale = gcs.Sum(x => x.MaleCount),
                                                TotalFemale = gcs.Sum(x => x.FemaleCount)
                                            };


                List<ComputeRegionValue> ListComputeRegionValue = new List<ComputeRegionValue>();
                foreach (var ItemEEOForRegion in ListEEOForRegion)
                {
                    foreach (var ItemRacesForRegion in ListRacesForRegion)
                    {
                        ComputeRegionValue _ComputeRegionValue = new ComputeRegionValue
                        {
                            EEOId = ItemEEOForRegion.EEOId,
                            RacesId = ItemRacesForRegion.RacesId,
                            TotalWorkforceMale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForRegion.RacesId && e.EEOId == ItemEEOForRegion.EEOId && e.GenderId == 1).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForRegion.RacesId && e.EEOId == ItemEEOForRegion.EEOId && e.GenderId == 2).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalSelectedEEO = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForRegion.EEOId).Sum(e => e.TotalEmployeeWithSelectedEEOCount),
                        };
                        ListComputeRegionValue.Add(_ComputeRegionValue);
                    }
                }
                _model.ListComputeRegionValue = new List<ComputeRegionValue>();
                _model.ListComputeRegionValue = ListComputeRegionValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOReportbyRegionService", "EEOReportbyRegionService.cs");
                throw;
            }
        }

        public List<SelectListItem> BindEmployeeRegionDropDown(int? OrganizationId, int? FileSubmissionId)
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

                AppUtility.LogMessage(ex, "BindEmployeeRegionDropDown", "EEOReportbyRegionService.cs");
                throw;
            }

        }

    }
}
