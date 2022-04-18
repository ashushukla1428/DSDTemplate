using EEONow.Context.EntityContext;
using EEONow.Interface.Interfaces;
using EEONow.Interfaces;
using EEONow.Models;
using EEONow.Models.Models.EEO4Report;
using EEONow.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Services.Services
{
    public class EEO4ReportBySupervisorService: IEEO4ReportBySupervisorService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public EEO4ReportBySupervisorService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
     
        public EEO4ReportViewModel GetEEO4ReportBySupervisor(Int32 OrganizationId, Int32 FileSubmissionId, string empPosition="")
        {
            try
            {
                
                // int EmployeeId = _context.Employees.Where(e => e.PositionNumber == empPosition && e.FileSubmission.FileSubmissionId==FileSubmissionId && e.Organization.OrganizationId==OrganizationId ).FirstOrDefault().EmployeeId;

                EEO4ReportViewModel _model = new EEO4ReportViewModel();

                string ManagerName = _context.Organizations.Where(e => e.OrganizationId == OrganizationId).Select(e => e.Name).FirstOrDefault();


                _model.ManagerName = ManagerName;

                var ListRacesForALM = _context.Races.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new RacesForALM { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListRacesForALM = new List<RacesForALM>();
                _model.ListRacesForALM.AddRange(ListRacesForALM);
                var ListEEOForALM = _context.EEOJobCategories.Where(e => e.Organization.OrganizationId == OrganizationId && e.Active == true).Select(e => new EEOForALM { EEOId = e.EEOJobCategoryId, EEOName = e.Name }).ToList();
                _model.ListEEOForALM = new List<EEOForALM>();
                _model.ListEEOForALM.AddRange(ListEEOForALM);



                //List <EmployeeForALM> employeeList = _context.getEEO1ReportDataBySupervisorID(FileSubmissionId, OrganizationId, EmployeeId)
                //                .Select(e => new EmployeeForALM { RacesId = (int)e.RaceId, EEOId = (int)e.EEOCodeId, EmployeeId = (int)e.EEOCodeId, GenderId = (int)e.GenderID }).ToList();
                var fileSubmissionID = new SqlParameter("@fileSubmissionID", FileSubmissionId);
                var organizationID = new SqlParameter("@organizationID", OrganizationId);
                var supervisorID = new SqlParameter("@positionNumber", empPosition);
                var employeeList = _context.Database.SqlQuery<EmployeeForALM>("getEEO4ReportDataBySupervisorID @fileSubmissionID,@organizationID,@positionNumber", fileSubmissionID, organizationID, supervisorID)
                                     .Select(e => new EmployeeForALM { RacesId = (int)e.RacesId, EEOId = (int)e.EEOId
                                     ,Salary=e.Salary,
                                         EmployeeId = (int)e.EmployeeId, GenderId = (int)e.GenderId }).ToList();
                //List<EmployeeForALM> employeeList = _context.Employees.Where(e => e.Organization != null && e.Race != null && e.Gender != null && e.Organization.OrganizationId == OrganizationId && e.FileSubmission.FileSubmissionId == FileSubmissionId)
                //              .Select(e => new EmployeeForALM { RacesId = e.Race.RaceId, EEOId = e.EEOJobCategory.EEOJobCategoryId, EmployeeId = e.EmployeeId, GenderId = e.Gender.Name.ToLower() == "male" ? 1 : 2 }).ToList();

                
                #region Salary Range
                var SalaryBetween0K_n_15K = from c in employeeList
                                            where c.Salary >= 00 && c.Salary <= Convert.ToDecimal(15999.99)
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

                var SalaryBetween16K_n_19K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(16000.00) && c.Salary <= Convert.ToDecimal(19999.99)
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
                var SalaryBetween20K_n_24K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(20000.00) && c.Salary <= Convert.ToDecimal(24999.99)
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
                var SalaryBetween25K_n_32K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(25000.00) && c.Salary <= Convert.ToDecimal(32999.99)
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
                var SalaryBetween33K_n_42K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(33000.00) && c.Salary <= Convert.ToDecimal(42999.99)
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
                var SalaryBetween43K_n_54K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(43000.00) && c.Salary <= Convert.ToDecimal(54999.99)
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
                var SalaryBetween55K_n_69K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(55000.00) && c.Salary <= Convert.ToDecimal(69999.99)
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
                var SalaryGreater_Than_70K = from c in employeeList
                                             where c.Salary >= Convert.ToDecimal(70000.00)
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
                #endregion
                var _orgGenders = _context.Genders.Where(c => c.Organization.OrganizationId == OrganizationId);
                int _maleId = _orgGenders.Where(c => c.Name.ToUpper() == "MALE").FirstOrDefault().GenderId;
                int _femaleId = _orgGenders.Where(c => c.Name.ToUpper() == "FEMALE").FirstOrDefault().GenderId;
                List<EEO4ComputeALMValue> ListComputeALMValue = new List<EEO4ComputeALMValue>();
                foreach (var ItemEEOForALM in ListEEOForALM)
                {
                    foreach (var ItemRacesForALM in ListRacesForALM)
                    {
                        EEO4ComputeALMValue _ComputeALMValue = new EEO4ComputeALMValue
                        {
                            EEOId = ItemEEOForALM.EEOId,
                            RacesId = ItemRacesForALM.RacesId,

                            TotalWorkforceMale_Between0K_n_15K = SalaryBetween0K_n_15K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between0K_n_15K = SalaryBetween0K_n_15K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between16K_n_19K = SalaryBetween16K_n_19K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between16K_n_19K = SalaryBetween16K_n_19K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between20K_n_24K = SalaryBetween20K_n_24K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between20K_n_24K = SalaryBetween20K_n_24K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between25K_n_32K = SalaryBetween25K_n_32K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between25K_n_32K = SalaryBetween25K_n_32K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between33K_n_42K = SalaryBetween33K_n_42K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between33K_n_42K = SalaryBetween33K_n_42K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between43K_n_54K = SalaryBetween43K_n_54K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between43K_n_54K = SalaryBetween43K_n_54K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Between55K_n_69K = SalaryBetween55K_n_69K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Between55K_n_69K = SalaryBetween55K_n_69K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),

                            TotalWorkforceMale_Greater_Than_70K = SalaryGreater_Than_70K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _maleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),
                            TotalWorkforceFemale_Greater_Than_70K = SalaryGreater_Than_70K.Where(e => e.RacesId == ItemRacesForALM.RacesId && e.EEOId == ItemEEOForALM.EEOId && e.GenderId == _femaleId).Select(e => e.TotalWorkforceCount).FirstOrDefault(),


                        };
                        ListComputeALMValue.Add(_ComputeALMValue);
                    }
                }

                _model.TotalWorkforce = (SalaryBetween0K_n_15K.Sum(c=>c.TotalWorkforceCount) + SalaryBetween16K_n_19K.Sum(c => c.TotalWorkforceCount) 
                    + SalaryBetween20K_n_24K.Sum(c => c.TotalWorkforceCount)
                + SalaryBetween25K_n_32K.Sum(c => c.TotalWorkforceCount) + SalaryBetween33K_n_42K.Sum(c => c.TotalWorkforceCount)
                + SalaryBetween43K_n_54K.Sum(c => c.TotalWorkforceCount) + SalaryBetween55K_n_69K.Sum(c => c.TotalWorkforceCount)
                + SalaryGreater_Than_70K.Sum(c => c.TotalWorkforceCount));
                _model.ListComputeALMValue = new List<EEO4ComputeALMValue>();
                _model.ListComputeALMValue = ListComputeALMValue;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetAvailableLaborMarketService", "ALMViaRacesEeoService.cs");
                throw;
            }
        }
        public List<string> GetEEO4ReportSalaryRangeList()
        {
            List<string> _salaryRangeList = new List<string>();
            _salaryRangeList.Add("0.0-15.9");
            _salaryRangeList.Add("16.0-19.9");
            _salaryRangeList.Add("20.0-24.9");
            _salaryRangeList.Add("25.0-32.9");
            _salaryRangeList.Add("33.0-42.9");
            _salaryRangeList.Add("43.0-54.9");
            _salaryRangeList.Add("55.0-69.9");
            _salaryRangeList.Add("70.0-PLUS");
           return _salaryRangeList;
         
        }
    }
}
