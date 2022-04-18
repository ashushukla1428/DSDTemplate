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
using System.IO;
using Newtonsoft;
using System.Data.SqlClient;
using System.Data;

namespace EEONow.Services
{
    public class HangfireService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        private static int TriggerMethod = 0;
        public HangfireService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public void GenerateDashboardData()
        {
            try
            {
                if (TriggerMethod == 0)
                {
                    TriggerMethod = 1;
                    _context.Database.CommandTimeout = 0;
                    var _PendingCsv = _context.GenerateCSVs.Where(e => e.Status == false).Take(Convert.ToInt32(ConfigurationManager.AppSettings["RecordLimits"])).ToList();

                    if (_PendingCsv.Count() > 0 && _PendingCsv != null)
                    {
                        foreach (var _item in _PendingCsv)
                        {
                            try
                            {

                                //GetCSVGraphData(_item.FileSubmission.FileSubmissionId, _item.Organization.OrganizationId);
                                //var _ResultData = _context.getCSVGraphData(_item.FileSubmission.FileSubmissionId, _item.Organization.OrganizationId).ToList();
                                var _ResultData = _context.getCSVGraphData(_item.FileSubmission.FileSubmissionId, _item.Organization.OrganizationId).ToList();


                                //Replace Duplicate employee Having [supervisorFlag]=0


                                var _Positionlist = _ResultData.Select(e => new { e.position }).GroupBy(c => c.position).Where(e => e.Count() > 1)
                                           .Select(grp => grp.Key);
                                foreach (var _SamePositionEmployee in _Positionlist)
                                {
                                    var SamePositionRecords = _ResultData.Where(e => e.position == _SamePositionEmployee).OrderByDescending(e => e.PositionDateOfHire).ToList();
                                    Int32 count = 0;
                                    foreach (var _SamePositionRecords in SamePositionRecords)
                                    {
                                        if (count > 0)
                                        {
                                            _SamePositionRecords.position = _SamePositionRecords.UserID.ToString();
                                        }
                                        count = 1;
                                    }
                                }

                                #region To Generate CSV file code section                  
                                StringBuilder builder = new StringBuilder();
                                List<string> columnNames = new List<string>();
                                List<string> rows = new List<string>();
                                //columnNames.Add("position,parent,level,color,first,middle,last,phone,email,picturePath,gender,race,age,positionTitle,programOffice,salary,eeo4Code,lastPerformanceRating,yearsOfService,agency,address,city,county,state,zipCode,supervisorFlag,vacantPositions,staffHeadCount,spanOfControl,supervisorStaffRatio,levelsUp,levelsDown,raceid,tysid,eeoid,NationalOrigin,OPSPosition,PositionDateOfHire,AgencyDateOfHire,UserID");
                                columnNames.Add("position,parent,level,color,first,middle,last,phone,email,gender,race,age,positionTitle,programOffice,salary,eeo4Code,lastPerformanceRating,agency,address,city,county,state,zipCode,supervisorFlag,staffHeadCount,NumberFTEPositions,NumberOPSPositions,NumberFTEVacantPositions,positionNumber,NumberOPSVacantPositions,NumberFTEFilledPositions,NumberOPSFilledPositions,spanOfControl,supervisorStaffRatio,levelsUP,levelsDown,Status,yearsFRS,yearsAgency,yearsCurrentPosition,raceid,tysid,eeoid,"
                                                + "EEORaceIndex,EEOGenderIndex,srid,vrid,pspid,arid,lprid,NationalOrigin,OPSPosition,StateCumulativeMonthsOfService,PositionDateOfHire,AgencyDateOfHire,UserID,"//);
                                                + "TotalSupervisors,TotalMinoritySupervisors,TotalWomenSupervisors,PercentMinoritySupervisors,PercentWomenSupervisors,"
                                                + "filled,vacant,"
                                                + "vacancydate,"
                                                + "genderFemale,genderMale,"
                                                + "EEOC1,EEOC2,EEOC3,EEOC4,EEOC5,EEOC6,EEOC7,EEOC8,"
                                                + "R1,R2,R3,R4,R5,R6,R7,"
                                                + "LPR1,LPR2,LPR3,LPR4,LPR5,LPR6,"
                                                + "SR1,SR2,SR3,SR4,SR5,SR6,SR7,SR8,SR9,"
                                                + "VR1,VR2,VR3,VR4,VR5,VR6,VR7,VR8,"
                                                + "AR1,AR2,AR3,AR4,AR5,AR6,AR7,AR8,"
                                                + "PYOS1,PYOS2,PYOS3,PYOS4,PYOS5,PYOS6,PYOS7,PYOS8,"
                                                + "AYOS1,AYOS2,AYOS3,AYOS4,AYOS5,AYOS6,AYOS7,AYOS8");

                                builder.Append(string.Join(",", columnNames.ToArray())).Append("\n");

                                rows.AddRange(_ResultData.Select(e => string.Join(",", "\"" + Convert.ToString(e.position) + "\"", "\"" + Convert.ToString(e.parent) + "\"", "\"" + Convert.ToString(e.level) + "\"", "\"" + "color" + "\"", "\"" + Convert.ToString(e.first) + "\"", "\"" + Convert.ToString(e.middle) + "\"", "\"" + Convert.ToString(e.last) + "\"", "\"" + Convert.ToString(e.phone) + "\"", "\"" + Convert.ToString(e.email) + "\"", "\"" + Convert.ToString(e.gender) + "\"", "\"" + Convert.ToString(e.race) + "\"", "\"" + Convert.ToString(e.age) + "\"", "\"" + Convert.ToString(e.positionTitle) + "\"", "\"" + Convert.ToString(e.programOffice) + "\"", "\"" + Convert.ToString(e.salary) + "\"", "\"" + Convert.ToString(e.eeo4Code) + "\"", "\"" + Convert.ToString(e.LastPerformanceRating) + "\"", "\"" + Convert.ToString(e.agency) + "\"", "\"" + Convert.ToString(e.address) + "\"", "\"" + Convert.ToString(e.city) + "\"", "\"" + Convert.ToString(e.county) + "\"", "\"" + Convert.ToString(e.state) + "\"", "\"" + Convert.ToString(e.zipCode) + "\"", "\"" + Convert.ToString(e.supervisorFlag) + "\"", "\"" + Convert.ToString(e.staffHeadCount) + "\"", "\"" + Convert.ToString(e.NumberFTEPositions) + "\"", "\"" + Convert.ToString(e.NumberOPSPositions) + "\"", "\"" + Convert.ToString(e.NumberFTEVacantPositions) + "\"",
                                                                                       "\"" + Convert.ToString(e.positionNumber) + "\"",
                                                                                       "\"" + Convert.ToString(e.NumberOPSVacantPositions) + "\"",
                                                                                       "\"" + Convert.ToString(e.NumberFTEFilledPositions) + "\"",
                                                                                       "\"" + Convert.ToString(e.NumberOPSFilledPositions) + "\"",
                                                                                       "\"" + Convert.ToString(e.spanOfControl) + "\"", "\"" + Convert.ToString(e.supervisorStaffRatio) + "\"", "\"" + Convert.ToString(e.levelsUP) + "\"", "\"" + Convert.ToString(e.levelsDown) + "\"", "\"" + Convert.ToString(e.Status) + "\"", "\"" + Convert.ToString(e.yearsFRS) + "\"", "\"" + Convert.ToString(e.yearsAgency) + "\"", "\"" + Convert.ToString(e.yearsCurrentPosition) + "\"", "\"" + Convert.ToString(e.raceid) + "\"", "\"" + Convert.ToString(e.tysid) + "\"",
                                                                                       "\"" + Convert.ToString(e.eeoid) + "\"",
                                                                                       "\"" + Convert.ToString(e.EEORaceIndex) + "\"",
                                                                                       "\"" + Convert.ToString(e.EEOGenderIndex) + "\"",
                                                                                       "\"" + Convert.ToString(e.srid) + "\"",
                                                                                       "\"" + Convert.ToString(e.vrid) + "\"",
                                                                                       "\"" + Convert.ToString(e.pspid) + "\"",
                                                                                       "\"" + Convert.ToString(e.arid) + "\"",
                                                                                       "\"" + Convert.ToString(e.lprid) + "\"",
                                                                                       "\"" + Convert.ToString(e.NationalOrigin) + "\"", "\"" + Convert.ToString(e.OPSPosition) + "\"", "\"" + Convert.ToString(e.StateCumulativeMonthsOfService) + "\"", "\"" + Convert.ToString(e.PositionDateOfHire) + "\"", "\"" + Convert.ToString(e.AgencyDateOfHire) + "\"", "\"" + Convert.ToString(e.UserID) + "\"",//)));                                                                                       
                                                                                       "\"" + Convert.ToString(e.TotalSupervisors) + "\"", "\"" + Convert.ToString(e.TotalMinoritySupervisors) + "\"",
                                                                                       "\"" + Convert.ToString(e.TotalWomenSupervisors) + "\"", "\"" + Convert.ToString(e.PercentMinoritySupervisors) + "\"",
                                                                                       "\"" + Convert.ToString(e.PercentWomenSupervisors) + "\"",
                                                                                       "\"" + Convert.ToString(e.filled) + "\"", "\"" + Convert.ToString(e.vacant) + "\"",
                                                                                       "\"" + Convert.ToDateTime(e.vacancydate).ToString("MM/dd/yyyy") + "\"",
                                                                                       "\"" + Convert.ToString(e.genderFemale) + "\"", "\"" + Convert.ToString(e.genderMale) + "\"",
                                                                                       "\"" + Convert.ToString(e.EEOC1) + "\"", "\"" + Convert.ToString(e.EEOC2) + "\"", "\"" + Convert.ToString(e.EEOC3) + "\"", "\"" + Convert.ToString(e.EEOC4) + "\"", "\"" + Convert.ToString(e.EEOC5) + "\"", "\"" + Convert.ToString(e.EEOC6) + "\"", "\"" + Convert.ToString(e.EEOC7) + "\"", "\"" + Convert.ToString(e.EEOC8) + "\"",
                                                                                       "\"" + Convert.ToString(e.R1) + "\"", "\"" + Convert.ToString(e.R2) + "\"", "\"" + Convert.ToString(e.R3) + "\"", "\"" + Convert.ToString(e.R4) + "\"", "\"" + Convert.ToString(e.R5) + "\"", "\"" + Convert.ToString(e.R6) + "\"", "\"" + Convert.ToString(e.R7) + "\"",
                                                                                       "\"" + Convert.ToString(e.LPR1) + "\"", "\"" + Convert.ToString(e.LPR2) + "\"", "\"" + Convert.ToString(e.LPR3) + "\"", "\"" + Convert.ToString(e.LPR4) + "\"", "\"" + Convert.ToString(e.LPR5) + "\"", "\"" + Convert.ToString(e.LPR6) + "\"",
                                                                                       "\"" + Convert.ToString(e.SR1) + "\"", "\"" + Convert.ToString(e.SR2) + "\"", "\"" + Convert.ToString(e.SR3) + "\"", "\"" + Convert.ToString(e.SR4) + "\"", "\"" + Convert.ToString(e.SR5) + "\"", "\"" + Convert.ToString(e.SR6) + "\"", "\"" + Convert.ToString(e.SR7) + "\"", "\"" + Convert.ToString(e.SR8) + "\"", "\"" + Convert.ToString(e.SR9) + "\"",
                                                                                       "\"" + Convert.ToString(e.VR1) + "\"", "\"" + Convert.ToString(e.VR2) + "\"", "\"" + Convert.ToString(e.VR3) + "\"", "\"" + Convert.ToString(e.VR4) + "\"", "\"" + Convert.ToString(e.VR5) + "\"", "\"" + Convert.ToString(e.VR6) + "\"", "\"" + Convert.ToString(e.VR7) + "\"", "\"" + Convert.ToString(e.VR8) + "\"",
                                                                                       "\"" + Convert.ToString(e.AR1) + "\"", "\"" + Convert.ToString(e.AR2) + "\"", "\"" + Convert.ToString(e.AR3) + "\"", "\"" + Convert.ToString(e.AR4) + "\"", "\"" + Convert.ToString(e.AR5) + "\"", "\"" + Convert.ToString(e.AR6) + "\"", "\"" + Convert.ToString(e.AR7) + "\"", "\"" + Convert.ToString(e.AR8) + "\"",
                                                                                       "\"" + Convert.ToString(e.PYOS1) + "\"", "\"" + Convert.ToString(e.PYOS2) + "\"", "\"" + Convert.ToString(e.PYOS3) + "\"", "\"" + Convert.ToString(e.PYOS4) + "\"", "\"" + Convert.ToString(e.PYOS5) + "\"", "\"" + Convert.ToString(e.PYOS6) + "\"", "\"" + Convert.ToString(e.PYOS7) + "\"", "\"" + Convert.ToString(e.PYOS8) + "\"",
                                                                                       "\"" + Convert.ToString(e.AYOS1) + "\"", "\"" + Convert.ToString(e.AYOS2) + "\"", "\"" + Convert.ToString(e.AYOS3) + "\"", "\"" + Convert.ToString(e.AYOS4) + "\"", "\"" + Convert.ToString(e.AYOS5) + "\"", "\"" + Convert.ToString(e.AYOS6) + "\"", "\"" + Convert.ToString(e.AYOS7) + "\"", "\"" + Convert.ToString(e.AYOS8) + "\"")));



                                //  rows.AddRange(_ResultData.Select(e => string.Join(",", Convert.ToString(e.position), Convert.ToString(e.parent) == "0" ? "" : Convert.ToString(e.parent), Convert.ToString(e.level), "color" /*e.color*/, Convert.ToString(e.first), Convert.ToString(e.middle), Convert.ToString(e.last), Convert.ToString(e.phone), Convert.ToString(e.email), Convert.ToString(e.picturePath), Convert.ToString(e.gender), Convert.ToString(e.race), Convert.ToString(e.age), Convert.ToString(e.positionTitle), Convert.ToString(e.programOffice), Convert.ToString(e.salary), Convert.ToString(e.eeo4Code), Convert.ToString(e.lastPerformanceRating), Convert.ToString(e.yearsOfService), Convert.ToString(e.agency), Convert.ToString(e.address), Convert.ToString(e.city), Convert.ToString(e.county), Convert.ToString(e.state), Convert.ToString(e.zipCode), Convert.ToString(e.supervisorFlag), Convert.ToString(e.FilledPositions), Convert.ToString(e.staffHeadCount), Convert.ToString(e.spanOfControl), Convert.ToString(e.supervisorStaffRatio), Convert.ToString(e.levelsUP), Convert.ToString(e.levelsDown), Convert.ToString(e.raceid), Convert.ToString(e.tysid), Convert.ToString(e.eeoid), Convert.ToString(e.NationalOrigin), Convert.ToString(e.OPSPosition), Convert.ToString(e.PositionDateOfHire), Convert.ToString(e.AgencyDateOfHire), Convert.ToString(e.UserID))));
                                builder.Append(string.Join("\n", rows.ToArray()));
                                String path = AppDomain.CurrentDomain.BaseDirectory + "/Dashboard/organizationcsv";
                                String FileName = _item.FileName;
                                File.WriteAllText(path + @"/" + FileName + ".csv", builder.ToString());
                                //save for embedded Url
                                //String embeddedpath = AppDomain.CurrentDomain.BaseDirectory + "/EmbeddedGraph/organizationcsv";
                                //File.WriteAllText(embeddedpath + @"/" + FileName + ".csv", builder.ToString());
                                #endregion
                                _item.Status = true;
                                AppUtility.SendFileAvailableNotification(_item.Organization.OrganizationId, _item.FileSubmission.SubmissionDateTime.Value.ToString("MM/dd/yyyy"), Convert.ToInt32(_item.CreatedBy));

                            }
                            catch (Exception ex)
                            {
                                AppUtility.LogMessage(ex, "GenerateDashboardData", "HangfireService.cs");
                                throw;
                            }

                        }
                        _context.SaveChanges();
                    }

                    TriggerMethod = 0;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GenerateDashboardData", "HangfireService.cs");
                throw;
            }

        }

        private string GetPercentage(String UpperValue, String DividentValue)
        {
            try
            {
                var result = Convert.ToDecimal((Convert.ToDecimal(UpperValue) / Convert.ToDecimal(DividentValue)) * 100);
                return string.Format("{0:0.##}", result);

            }
            catch
            {
                return "0%";
            }

        }
        public void GenerateDashboardViaOrganizationId(Int32 _OrganizationId)
        {
            try
            {
                var _PendingCsv = _context.GenerateCSVs.Where(e => e.Status == false && e.Organization.OrganizationId == _OrganizationId).ToList();

                if (_PendingCsv.Count() > 0 && _PendingCsv != null)
                {
                    foreach (var _item in _PendingCsv)
                    {
                        var _ResultData = _context.getCSVGraphData(_item.FileSubmission.FileSubmissionId, _item.Organization.OrganizationId).ToList();
                        #region To Generate CSV file code section                  
                        StringBuilder builder = new StringBuilder();
                        List<string> columnNames = new List<string>();
                        List<string> rows = new List<string>();
                        columnNames.Add("position,parent,level,color,first,middle,last,phone,email,picturePath,gender,race,age,positionTitle,programOffice,salary,eeo4Code,lastPerformanceRating,yearsOfService,agency,address,city,county,state,zipCode,supervisorFlag,staffHeadCount,spanOfControl,supervisorStaffRatio,levelsUp,levelsDown,raceid,tysid,eeoid");
                        builder.Append(string.Join(",", columnNames.ToArray())).Append("\n");
                        // rows.AddRange(_ResultData.Select(e => string.Join(",", e.position.ToString(), e.parent.ToString(), e.level.ToString(), "color" /*e.color*/, e.first.ToString(), e.middle.ToString(), e.last.ToString(), e.phone.ToString(), e.email.ToString(), e.picturePath.ToString(), e.gender.ToString(), e.race.ToString(), e.age.ToString(), e.positionTitle.ToString(), e.programOffice.ToString(), e.salary.ToString(), e.eeo4Code.ToString(), e.lastPerformanceRating.ToString(), e.yearsOfService.ToString(), e.agency.ToString(), e.address.ToString(), e.city.ToString(), e.county.ToString(), e.state.ToString(), e.zipCode.ToString(), e.supervisorFlag.ToString(), e.vacantPositions.ToString(), e.staffHeadCount, e.spanOfControl, e.supervisorStaffRatio.ToString(), e.levelsUP.ToString(), e.levelsDown.ToString(), e.raceid.ToString(), e.tysid.ToString(), e.eeoid.ToString())));
                        builder.Append(string.Join("\n", rows.ToArray()));
                        String path = AppDomain.CurrentDomain.BaseDirectory + "/dashboard/organizationcsv";
                        String FileName = _item.FileName;
                        File.WriteAllText(path + @"/" + FileName + ".csv", builder.ToString());
                        #endregion
                        _item.Status = true;
                    }
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GenerateDashboardViaOrganizationId", "HangfireService.cs");
                throw;
            }
        }
    }
}
