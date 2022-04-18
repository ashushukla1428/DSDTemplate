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
    public class ALMViaRacesEeoService : IALMViaRacesEeoService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public ALMViaRacesEeoService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public ALMViaRacesEeoModel GetAvailableLaborMarketService(Int32 OrganizationId, Int32 FileSubmissionId)
        {
            try
            {

                ALMViaRacesEeoModel _model = new ALMViaRacesEeoModel();

                //  var SupervisorDetail = _context.Employees.Where(e => e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId && e.SupervisorPositionNumber == "0").FirstOrDefault();

                //string ManagerName = SupervisorDetail.FirstName == "NULL" ? "" : SupervisorDetail.FirstName+" ";
                //ManagerName += SupervisorDetail.MiddleName == "NULL" ? " " : SupervisorDetail.MiddleName+" ";
                //ManagerName += SupervisorDetail.LastName == "NULL" ? " ": SupervisorDetail.LastName;


                string ManagerName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();


                _model.ManagerName = ManagerName;

                var ListRacesForALM = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForALM { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForALM = new List<RacesForALM>();
                _model.ListRacesForALM.AddRange(ListRacesForALM);
                var ListEEOForALM = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForALM { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForALM = new List<EEOForALM>();
                _model.ListEEOForALM.AddRange(ListEEOForALM);
                List<EmployeeForALM> employeeList = _context.Employees.Where(e => e.Organization != null && e.Race != null && e.Gender != null && e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId)
                                .Select(e => new EmployeeForALM { RacesId = e.Race.RaceId, EEOId = e.EEOJobCategory.EEOJobCategoryId, EmployeeId = e.EmployeeId, GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2 }).ToList();

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

                List<CountedALM> CountedALMlist = DbAvailableLaborMarketFileVersion.AvailableLaborMarketDatas.Select(r => new CountedALM { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList();
                //List<CountedALM> CountedALMlist = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true)
                //                               .Select(e => e.AvailableLaborMarketDatas.Select(r => new CountedALM { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList()).FirstOrDefault();

                var TotalALM = from c in CountedALMlist
                               group c by new
                               {
                                   c.EEOId,
                                   c.RacesId
                               } into gcs
                               select new EmployeeWithCountedALM()
                               {
                                   EEOId = gcs.Key.EEOId,
                                   RacesId = gcs.Key.RacesId,
                                   TotalMale = gcs.Sum(x => x.MaleCount),
                                   TotalFemale = gcs.Sum(x => x.FemaleCount)
                               };

                var ALMWithSelectedEEO = from c in CountedALMlist
                                         group c by new
                                         {
                                             c.EEOId
                                         } into gcs
                                         select new EmployeeWithCountedALM()
                                         {
                                             EEOId = gcs.Key.EEOId,
                                             TotalMale = gcs.Sum(x => x.MaleCount),
                                             TotalFemale = gcs.Sum(x => x.FemaleCount)
                                         };

                var _orgGenders = _context.Genders.Where(c => c.Organization.OrganizationId == OrganizationId);
                int _maleId = _orgGenders.Where(c => c.Name.ToUpper() == "MALE").FirstOrDefault().GenderId;
                int _femaleId = _orgGenders.Where(c => c.Name.ToUpper() == "FEMALE").FirstOrDefault().GenderId;
                List<ComputeALMValue> ListComputeALMValue = new List<ComputeALMValue>();
                foreach (var ItemEEOForALM in ListEEOForALM)
                {
                    foreach (var ItemRacesForALM in ListRacesForALM)
                    {
                        ComputeALMValue _ComputeALMValue = new ComputeALMValue
                        {
                            EEOId = ItemEEOForALM.EEOId,
                            RacesId = ItemRacesForALM.RacesId,
                            TotalWorkforceMale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            //TotalSelectedEEOFemale = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId && e.GenderId == 2).Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),
                            //TotalSelectedEEOMale = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId && e.GenderId == 1).Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),

                            TotalSelectedEEO = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Sum(e => e.TotalEmployeeWithSelectedEEOCount),//.Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),


                            AMLMale = TotalALM.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalMale).FirstOrDefault(),
                            AMLFemale = TotalALM.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalFemale).FirstOrDefault(),
                            //AMLSelectedEEOMale = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalMale).FirstOrDefault(),
                            //AMLSelectedEEOFemale = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalFemale).FirstOrDefault(),
                            AMLSelectedEEO = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => (e.TotalMale + e.TotalFemale)).FirstOrDefault(),

                        };
                        ListComputeALMValue.Add(_ComputeALMValue);
                    }
                }
                _model.ListComputeALMValue = new List<ComputeALMValue>();
                _model.ListComputeALMValue = ListComputeALMValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAvailableLaborMarketService", "ALMViaRacesEeoService.cs");
                throw;
            }
        }

        public ViewIndexReportModel GetIndexReport(int organisastionId, int FileSubmissionID, string position)
        {
            try
            {
                var _spResult = _context.rptEEOIndexReport(organisastionId, FileSubmissionID, position).ToList();
              //  var _spResult = _context.rptEEOIndexReport(1, 8, "71879").ToList();

                ViewIndexReportModel _Result = new ViewIndexReportModel
                {
                    EmployeeName = _spResult.FirstOrDefault().EmployeeName,
                    EEOGenderIndexRating = _spResult.Where(e=>e.IndexType== "GenderIndex").FirstOrDefault().EEOIndexRating,
                    EEORaceIndexRating = _spResult.Where(e => e.IndexType == "RaceIndex").FirstOrDefault().EEOIndexRating,
                    GenderIndexList = _spResult.Where(e => e.IndexType == "GenderIndex").Select( e=> new IndexTypeModel {ALMPercentage=e.ALMPercentage,CurrentPercentage=e.CurrentPercent,CurrentCount=e.CurrentCount,TypeName=e.TypeName }).ToList(),
                    RaceIndexList= _spResult.Where(e => e.IndexType == "RaceIndex").Select(e => new IndexTypeModel { ALMPercentage=e.ALMPercentage,CurrentPercentage=e.CurrentPercent,CurrentCount=e.CurrentCount,TypeName=e.TypeName }).ToList(),
                }; 
                return _Result;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetIndexReport", "ALMViaRacesEeoService.cs");
                throw;
            }
        }
        public ALMViaRacesEeoModel GetAvailableLaborMarketReportByEmpId(Int32 OrganizationId, Int32 FileSubmissionId, string empPosition)
        {
            try
            {
               // int EmployeeId = _context.Employees.Where(e => e.PositionNumber == empPosition && e.FileSubmission.FileSubmissionId==FileSubmissionId && e.Organization.OrganizationId==OrganizationId ).FirstOrDefault().EmployeeId;

                ALMViaRacesEeoModel _model = new ALMViaRacesEeoModel();

                string ManagerName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();


                _model.ManagerName = ManagerName;

                var ListRacesForALM = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForALM { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForALM = new List<RacesForALM>();
                _model.ListRacesForALM.AddRange(ListRacesForALM);
                var ListEEOForALM = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForALM { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForALM = new List<EEOForALM>();
                _model.ListEEOForALM.AddRange(ListEEOForALM);

               

                //List <EmployeeForALM> employeeList = _context.getEEO1ReportDataBySupervisorID(FileSubmissionId, OrganizationId, EmployeeId)
                //                .Select(e => new I cn EmployeeForALM { RacesId = (int)e.RaceId, EEOId = (int)e.EEOCodeId, EmployeeId = (int)e.EEOCodeId, GenderId = (int)e.GenderID }).ToList();
                var fileSubmissionID = new SqlParameter("@fileSubmissionID", FileSubmissionId);
                var organizationID = new SqlParameter("@organizationID", OrganizationId);
                var supervisorID = new SqlParameter("@positionNumber", empPosition);
                var employeeList = _context.Database.SqlQuery<EmployeeForALM>("getEEO1ReportDataBySupervisorID @fileSubmissionID,@organizationID,@positionNumber", fileSubmissionID, organizationID, supervisorID)
                                     .Select(e => new EmployeeForALM { RacesId = (int)e.RacesId, EEOId = (int)e.EEOId, EmployeeId = (int)e.EmployeeId, GenderId = (int)e.GenderId }).ToList();
                //List<EmployeeForALM> employeeList = _context.Employees.Where(e => e.Organization != null && e.Race != null && e.Gender != null && e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId)
                //              .Select(e => new EmployeeForALM { RacesId = e.Race.RaceId, EEOId = e.EEOJobCategory.EEOJobCategoryId, EmployeeId = e.EmployeeId, GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2 }).ToList();

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

                List<CountedALM> CountedALMlist = DbAvailableLaborMarketFileVersion.AvailableLaborMarketDatas.Select(r => new CountedALM { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList();
                //List<CountedALM> CountedALMlist = _context.AvailableLaborMarketFileVersions.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true)
                //                               .Select(e => e.AvailableLaborMarketDatas.Select(r => new CountedALM { EEOId = r.EEOJobCategory.EEOJobCategoryId, RacesId = r.Race.RaceId, FemaleCount = r.FemaleValue, MaleCount = r.MaleValue }).ToList()).FirstOrDefault();

                var TotalALM = from c in CountedALMlist
                               group c by new
                               {
                                   c.EEOId,
                                   c.RacesId
                               } into gcs
                               select new EmployeeWithCountedALM()
                               {
                                   EEOId = gcs.Key.EEOId,
                                   RacesId = gcs.Key.RacesId,
                                   TotalMale = gcs.Sum(x => x.MaleCount),
                                   TotalFemale = gcs.Sum(x => x.FemaleCount)
                               };

                var ALMWithSelectedEEO = from c in CountedALMlist
                                         group c by new
                                         {
                                             c.EEOId
                                         } into gcs
                                         select new EmployeeWithCountedALM()
                                         {
                                             EEOId = gcs.Key.EEOId,
                                             TotalMale = gcs.Sum(x => x.MaleCount),
                                             TotalFemale = gcs.Sum(x => x.FemaleCount)
                                         };


                List<ComputeALMValue> ListComputeALMValue = new List<ComputeALMValue>();
                var _orgGenders = _context.Genders.Where(c => c.Organization.OrganizationId == OrganizationId);
                int _maleId = _orgGenders.Where(c => c.Name.ToUpper() == "MALE").FirstOrDefault().GenderId;
                int _femaleId = _orgGenders.Where(c => c.Name.ToUpper() == "FEMALE").FirstOrDefault().GenderId;

                foreach (var ItemEEOForALM in ListEEOForALM)
                {
                    foreach (var ItemRacesForALM in ListRacesForALM)
                    {
                        ComputeALMValue _ComputeALMValue = new ComputeALMValue
                        {
                            EEOId = ItemEEOForALM.EEOId,
                            RacesId = ItemRacesForALM.RacesId,
                            TotalWorkforceMale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale = TotalWorkforce.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            //TotalSelectedEEOFemale = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId && e.GenderId == 2).Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),
                            //TotalSelectedEEOMale = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId && e.GenderId == 1).Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),

                            TotalSelectedEEO = EmployeeWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Sum(e => e.TotalEmployeeWithSelectedEEOCount),//.Select(e => e.TotalEmployeeWithSelectedEEOCount).FirstOrDefault(),


                            AMLMale = TotalALM.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalMale).FirstOrDefault(),
                            AMLFemale = TotalALM.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalFemale).FirstOrDefault(),
                            //AMLSelectedEEOMale = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalMale).FirstOrDefault(),
                            //AMLSelectedEEOFemale = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => e.TotalFemale).FirstOrDefault(),
                            AMLSelectedEEO = ALMWithSelectedEEO.Where(e => e.EEOId == ItemEEOForALM.EEOId).Select(e => (e.TotalMale + e.TotalFemale)).FirstOrDefault(),

                        };
                        ListComputeALMValue.Add(_ComputeALMValue);
                    }
                }
                _model.ListComputeALMValue = new List<ComputeALMValue>();
                _model.ListComputeALMValue = ListComputeALMValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAvailableLaborMarketService", "ALMViaRacesEeoService.cs");
                throw;
            }
        }
    }
}
