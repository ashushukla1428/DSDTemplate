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
    public class ALMByFederalJobCodesReportService : IALMByFederalJobCodesReportService
    {
        private readonly EEONowEntity _context;
        IRepository _repository;
        public ALMByFederalJobCodesReportService()
        {
            _repository = new Repository();
            _context = new EEONowEntity();
        }
        public ALMByFederalJobCodesReportModel GetALMByFederalJobCodesReportService(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs)
        {
            try
            {
                ALMByFederalJobCodesReportModel _model = new ALMByFederalJobCodesReportModel();

                _model.DiplatTitleName = _context.USCensus_DataVersion.Where(e => e.USCensusVersionID == uSCensusVersionID).FirstOrDefault().Description;
                if (searchByWorkSite == 1)
                {
                    _model.DiplatContent = "Worksite Geography - 5-year ACS data presents data according to where people worked at the time of survey. These tables provide the number of people who were employed “at work,” that is, those who did any work at all during the reference week as paid employees, worked in their own business or profession, worked on their own farm, or worked 15 hours or more as unpaid workers on a family farm or in a family business in a given county or place.";
                }
                else
                {
                    _model.DiplatContent = "Residence Geography - 5-year ACS data presents data according to where people lived, regardless of where they worked. These tables include people who were employed at work; employed but not at work, because they were temporarily absent due to illness, bad weather, industrial dispute, vacation, or other personal reasons; and the unemployed, who were actively looking for work in the last four weeks and available to start a job, no work experience in the last 5 years or most recent job was in a military-specific occupation.";
                }
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == 1 && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForALMByFederalJobCodes { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForALMByFederalJobCodes>();
                _model.ListOfRaces.AddRange(ListOfRaces);
                _context.Database.CommandTimeout = 0;
                var _rptALMByFederalJobCodes = _context.rptGetALM_ByFederalJobCodes(uSCensusVersionID, stateID, eEOOccupationCodeID, eEOCategoryCodeNbr, searchByWorkSite == 1 ? true : false, eSRCodes.Length > 0 ? eSRCodes : null, pUMA_CODES.Length > 0 ? pUMA_CODES : null, majorOccupationGroup,minorOccupationGroupID,boardingOccupationGroupID, occupationIDs.Length > 0 ? occupationIDs : null).ToList();                
                var listofStaff = _rptALMByFederalJobCodes.Select(e => new ALMByFederalJobCodes
                {
                    EEOCategoryNbr = e.EEOJobCode,
                    EEOCategoryDesc = e.EEOJobDescription,
                    hFemale = e.hFemale,
                    hMale = e.hMale,
                    mWhite = e.mWhite,
                    fWhite = e.fWhite,
                    mBlackOrAfricanAmerican = e.mBlackOrAfricanAmerican,
                    fBlackOrAfricanAmerican = e.fBlackOrAfricanAmerican,
                    mNativeHawaiianOrOtherPacificIslander = e.mNativeHawaiianOrOtherPacificIslander,
                    fNativeHawaiianOrOtherPacificIslander = e.fNativeHawaiianOrOtherPacificIslander,
                    mAsian = e.mAsian,
                    fAsian = e.fAsian,
                    mAmericanIndianORAlaskaNative = e.mAmericanIndianORAlaskaNative,
                    fAmericanIndianORAlaskaNative = e.fAmericanIndianORAlaskaNative,
                    mTwoOrMoreRaces = e.mTwoOrMoreRaces,
                    fTwoOrMoreRaces = e.fTwoOrMoreRaces,
                    Total = (e.hFemale + e.hMale + e.mWhite + e.fWhite + e.mBlackOrAfricanAmerican + e.fBlackOrAfricanAmerican
                           + e.mNativeHawaiianOrOtherPacificIslander + e.fNativeHawaiianOrOtherPacificIslander + e.mAsian + e.fAsian + e.mAmericanIndianORAlaskaNative + e.fAmericanIndianORAlaskaNative
                           + e.mTwoOrMoreRaces + e.fTwoOrMoreRaces)
                }).ToList();
                _model.GrandTotal = listofStaff.Sum(e => e.Total) == 0 ? 1 : listofStaff.Sum(e => e.Total);

                _model.mTotalWhite = _rptALMByFederalJobCodes.Sum(e => e.mWhite);
                _model.fTotalWhite = _rptALMByFederalJobCodes.Sum(e => e.fWhite);
                _model.mTotalBlackOrAfricanAmerican = _rptALMByFederalJobCodes.Sum(e => e.mBlackOrAfricanAmerican);
                _model.fTotalBlackOrAfricanAmerican = _rptALMByFederalJobCodes.Sum(e => e.fBlackOrAfricanAmerican);
                _model.hTotalMale = _rptALMByFederalJobCodes.Sum(e => e.hMale);
                _model.hTotalFemale = _rptALMByFederalJobCodes.Sum(e => e.hFemale);
                _model.mTotalAsian = _rptALMByFederalJobCodes.Sum(e => e.mAsian);
                _model.fTotalAsian = _rptALMByFederalJobCodes.Sum(e => e.fAsian);
                _model.mTotalAmericanIndianORAlaskaNative = _rptALMByFederalJobCodes.Sum(e => e.mAmericanIndianORAlaskaNative);
                _model.fTotalAmericanIndianORAlaskaNative = _rptALMByFederalJobCodes.Sum(e => e.fAmericanIndianORAlaskaNative);
                _model.mTotalNativeHawaiianOrOtherPacificIslander = _rptALMByFederalJobCodes.Sum(e => e.mNativeHawaiianOrOtherPacificIslander);
                _model.fTotalNativeHawaiianOrOtherPacificIslander = _rptALMByFederalJobCodes.Sum(e => e.fNativeHawaiianOrOtherPacificIslander);
                _model.mTotalTwoOrMoreRaces = _rptALMByFederalJobCodes.Sum(e => e.mTwoOrMoreRaces);
                _model.fTotalTwoOrMoreRaces = _rptALMByFederalJobCodes.Sum(e => e.fTwoOrMoreRaces);

                _model.ListALMByFederalJobCodes = listofStaff;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetALMByFederalJobCodesReportService", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }
        public ALMByFederalJobCodesReportModel GetALMByFederalJobCodesExportService(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs)
        {
            try
            {
                ALMByFederalJobCodesReportModel _model = new ALMByFederalJobCodesReportModel();

                _model.DiplatTitleName = _context.USCensus_DataVersion.Where(e => e.USCensusVersionID == uSCensusVersionID).FirstOrDefault().Description;
                if (searchByWorkSite == 1)
                {
                    _model.DiplatContent = "Worksite Geography - 5-year ACS data presents data according to where people worked at the time of survey. These tables provide the number of people who were employed “at work,” that is, those who did any work at all during the reference week as paid employees, worked in their own business or profession, worked on their own farm, or worked 15 hours or more as unpaid workers on a family farm or in a family business in a given county or place.";
                }
                else
                {
                    _model.DiplatContent = "Residence Geography - 5-year ACS data presents data according to where people lived, regardless of where they worked. These tables include people who were employed at work; employed but not at work, because they were temporarily absent due to illness, bad weather, industrial dispute, vacation, or other personal reasons; and the unemployed, who were actively looking for work in the last four weeks and available to start a job, no work experience in the last 5 years or most recent job was in a military-specific occupation.";
                }
                var ListOfRaces = _context.Races.Where(e => e.Organization.OrganizationId == 1 && e.Active == true).OrderBy(e => e.RaceNumber).Select(e => new RacesForALMByFederalJobCodes { RacesId = e.RaceId, RacesName = e.Name }).ToList();
                _model.ListOfRaces = new List<RacesForALMByFederalJobCodes>();
                _model.ListOfRaces.AddRange(ListOfRaces);
                _context.Database.CommandTimeout = 0;
                var _rptALMByFederalJobCodes = _context.rptGetALM_ByFederalJobCodes(uSCensusVersionID, stateID, eEOOccupationCodeID, eEOCategoryCodeNbr, searchByWorkSite == 1 ? true : false, eSRCodes.Length > 0 ? eSRCodes : null, pUMA_CODES.Length > 0 ? pUMA_CODES : null, majorOccupationGroup, minorOccupationGroupID, boardingOccupationGroupID, occupationIDs.Length > 0 ? occupationIDs : null).ToList();
                var listofStaff = _rptALMByFederalJobCodes.Select(e => new ALMByFederalJobCodes
                {
                    EEOCategoryNbr = e.EEOJobCode,
                    EEOCategoryDesc = e.EEOJobDescription,
                    hFemale = e.hFemale,
                    hMale = e.hMale,
                    mWhite = e.mWhite,
                    fWhite = e.fWhite,
                    mBlackOrAfricanAmerican = e.mBlackOrAfricanAmerican,
                    fBlackOrAfricanAmerican = e.fBlackOrAfricanAmerican,
                    mNativeHawaiianOrOtherPacificIslander = e.mNativeHawaiianOrOtherPacificIslander,
                    fNativeHawaiianOrOtherPacificIslander = e.fNativeHawaiianOrOtherPacificIslander,
                    mAsian = e.mAsian,
                    fAsian = e.fAsian,
                    mAmericanIndianORAlaskaNative = e.mAmericanIndianORAlaskaNative,
                    fAmericanIndianORAlaskaNative = e.fAmericanIndianORAlaskaNative,
                    mTwoOrMoreRaces = e.mTwoOrMoreRaces,
                    fTwoOrMoreRaces = e.fTwoOrMoreRaces,
                    Total = (e.hFemale + e.hMale + e.mWhite + e.fWhite + e.mBlackOrAfricanAmerican + e.fBlackOrAfricanAmerican
                           + e.mNativeHawaiianOrOtherPacificIslander + e.fNativeHawaiianOrOtherPacificIslander + e.mAsian + e.fAsian + e.mAmericanIndianORAlaskaNative + e.fAmericanIndianORAlaskaNative
                           + e.mTwoOrMoreRaces + e.fTwoOrMoreRaces)
                }).ToList();
                _model.GrandTotal = listofStaff.Sum(e => e.Total) == 0 ? 1 : listofStaff.Sum(e => e.Total);

                _model.mTotalWhite = _rptALMByFederalJobCodes.Sum(e => e.mWhite);
                _model.fTotalWhite = _rptALMByFederalJobCodes.Sum(e => e.fWhite);
                _model.mTotalBlackOrAfricanAmerican = _rptALMByFederalJobCodes.Sum(e => e.mBlackOrAfricanAmerican);
                _model.fTotalBlackOrAfricanAmerican = _rptALMByFederalJobCodes.Sum(e => e.fBlackOrAfricanAmerican);
                _model.hTotalMale = _rptALMByFederalJobCodes.Sum(e => e.hMale);
                _model.hTotalFemale = _rptALMByFederalJobCodes.Sum(e => e.hFemale);
                _model.mTotalAsian = _rptALMByFederalJobCodes.Sum(e => e.mAsian);
                _model.fTotalAsian = _rptALMByFederalJobCodes.Sum(e => e.fAsian);
                _model.mTotalAmericanIndianORAlaskaNative = _rptALMByFederalJobCodes.Sum(e => e.mAmericanIndianORAlaskaNative);
                _model.fTotalAmericanIndianORAlaskaNative = _rptALMByFederalJobCodes.Sum(e => e.fAmericanIndianORAlaskaNative);
                _model.mTotalNativeHawaiianOrOtherPacificIslander = _rptALMByFederalJobCodes.Sum(e => e.mNativeHawaiianOrOtherPacificIslander);
                _model.fTotalNativeHawaiianOrOtherPacificIslander = _rptALMByFederalJobCodes.Sum(e => e.fNativeHawaiianOrOtherPacificIslander);
                _model.mTotalTwoOrMoreRaces = _rptALMByFederalJobCodes.Sum(e => e.mTwoOrMoreRaces);
                _model.fTotalTwoOrMoreRaces = _rptALMByFederalJobCodes.Sum(e => e.fTwoOrMoreRaces);

                _model.ListALMByFederalJobCodes = listofStaff;
                return _model;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetALMByFederalJobCodesExportService", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetUSCensusDataVersionDropDown()
        {
            try
            {
                var _USCensus_DataVersion = _context.USCensus_DataVersion.ToList();
                var _ListUSCensus_DataVersion = new List<SelectListItem>();
                _ListUSCensus_DataVersion.AddRange(_USCensus_DataVersion.Select(g => new SelectListItem { Text = g.Description.ToString(), Value = g.USCensusVersionID.ToString() }).ToList());
                return _ListUSCensus_DataVersion;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUSCensusDataVersionDropDown", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetStateDropDown()
        {
            try
            {
                var _State = _context.States.Where(e => e.Active == true).ToList();
                var _ListState = new List<SelectListItem>();
                _ListState.AddRange(_State.Select(g => new SelectListItem { Text = g.Description.ToString(), Value = g.StateId.ToString() }).OrderBy(e => e.Text).ToList());
                return _ListState;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetStateDropDown", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetUSCensusOccupationsDropDown()
        {
            try
            {
                var _USCensus_EEO_OccupationCodes = _context.USCensus_EEO_OccupationCodes.ToList();
                var _ListUSCensus_EEO_OccupationCodes = new List<SelectListItem>();
                _ListUSCensus_EEO_OccupationCodes.AddRange(_USCensus_EEO_OccupationCodes.Select(g => new SelectListItem { Text = g.Code + " - " + g.Description.ToString(), Value = g.EEOOccupationCodeID.ToString() }).ToList());
                return _ListUSCensus_EEO_OccupationCodes;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUSCensusOccupationsDropDown", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetUSCensusGeographyTypesDropDown()
        {
            try
            {
                var _USCensus_GeographyTypes = _context.USCensus_GeographyTypes.ToList();
                var _ListUSCensus_GeographyTypes = new List<SelectListItem>();
                _ListUSCensus_GeographyTypes.AddRange(_USCensus_GeographyTypes.Select(g => new SelectListItem { Text = g.Description.ToString(), Value = g.GeographyTypeId.ToString() }).ToList());
                return _ListUSCensus_GeographyTypes;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetUSCensusGeographyTypesDropDown", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetPUMACodesDropDown(int? StateID, int? uscensusgeographytypesid)
        {
            try
            {
                if (StateID == null||uscensusgeographytypesid == null)
                {
                    var _ListUSCensus_PUMAWorksiteCodes = new List<SelectListItem>();
                    return _ListUSCensus_PUMAWorksiteCodes;
                }
                else
                {
                    if (uscensusgeographytypesid == 2)
                    {
                        var _USCensus_PUMAResidenceCodes = _context.USCensus_PUMAResidenceCodes.Where(e => e.State.StateId == StateID).ToList();
                       
                        var _ListUSCensus_PUMAResidenceCodes = new List<SelectListItem>();
                        _ListUSCensus_PUMAResidenceCodes.AddRange(_USCensus_PUMAResidenceCodes.Select(g => new SelectListItem { Text = g.PUMADescription.ToString(), Value = g.PUMACode.ToString() }).ToList());
                        return _ListUSCensus_PUMAResidenceCodes;
                    }
                    else
                    {
                        var _USCensus_PUMAWorksiteCodes = _context.USCensus_PUMAWorksiteCodes.Where(e => e.State.StateId == StateID).ToList();
                      
                        var _ListUSCensus_PUMAWorksiteCodes = new List<SelectListItem>();
                        _ListUSCensus_PUMAWorksiteCodes.AddRange(_USCensus_PUMAWorksiteCodes.Select(g => new SelectListItem { Text = g.PUMADescription.ToString(), Value = g.PUMACode.ToString() }).ToList());
                        return _ListUSCensus_PUMAWorksiteCodes;
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetPUMACodesDropDown", "ALMByEEOCategoryReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetEmploymentStatusDropDown(int? uscensusgeographytypesid)
        {
            try
            {
                if (uscensusgeographytypesid == null)
                {
                    var _ListUSCensus_PUMAWorksiteCodes = new List<SelectListItem>();
                    return _ListUSCensus_PUMAWorksiteCodes;
                }
                else
                {
                    var _EmploymentStatus = _context.USCensus_GeographyTypes.Where(e => e.GeographyTypeId == uscensusgeographytypesid).FirstOrDefault().USCensus_EmployeeStatusRecode.ToList();//USCensus_EmployeeStatusRecode.Where(e=>e.USCensus_DataVersion.USCensus_GeographyTypes).ToList();
                    var _ListEmploymentStatus = new List<SelectListItem>();
                    _ListEmploymentStatus.AddRange(_EmploymentStatus.Select(g => new SelectListItem { Text = g.Description.ToString(), Value = g.Code.ToString() }).ToList());
                    return _ListEmploymentStatus;
                }
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEmploymentStatusDropDown", "ALMByEEOCategoryReportService.cs");
                throw;
            }
        }



        public List<SelectListItem> GetMajorOccupationalGroup()
        {
            try
            {                
                var _Model = _context.USCensus_MajorOccupationGroups.Where(e => e.Active == true).ToList();
                var _ListModel = new List<SelectListItem>();
                _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.MajorOccupationGroupID.ToString() }).ToList());
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMajorOccupationalGroup", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetMinorOccupationalGroup(int MajorId)
        {
            try
            {
                var _Model = _context.USCensus_MajorOccupationGroups.Where(e => e.MajorOccupationGroupID == MajorId).FirstOrDefault().USCensus_MinorOccupationGroups.ToList();
                var _ListModel = new List<SelectListItem>();
                _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.MinorOccupationGroupID.ToString() }).ToList());
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetMinorOccupationalGroup", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetBoardingOccupationalGroup(int MinorId)
        {
            try
            {
                var _Model = _context.USCensus_MinorOccupationGroups.Where(e => e.MinorOccupationGroupID == MinorId).FirstOrDefault().USCensus_BoardingOccupationGroups.ToList();
                var _ListModel = new List<SelectListItem>();
                _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.BoardingOccupationGroupID.ToString() }).ToList());
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetBoardingOccupationalGroup", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }

        public List<SelectListItem> GetOccupations(int BoardingId)
        {
            try
            {
                var _ListModel = new List<SelectListItem>();
                if (BoardingId > 0)
                {
                    var _Model = _context.USCensus_BoardingOccupationGroups.Where(e => e.BoardingOccupationGroupID == BoardingId).FirstOrDefault().USCensus_Occupations.ToList();

                    _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.OccupationID.ToString() }).ToList());
                }
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOccupations", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }
        public List<SelectListItem> GetOccupationswithEEo(int EEOCategoriesId)
        {
            try
            {

                var _Model = _context.USCensus_Occupations_EEOJobClasificationCategories.Where(e => e.EEOJobClasificationCategoryID == EEOCategoriesId).Select(e=>e.USCensus_Occupations).ToList();
                var _ListModel = new List<SelectListItem>();
                _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.OccupationID.ToString() }).ToList());
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetOccupations", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }


        public List<SelectListItem> GetEEOCategories(int CensusOccupationsId)
        {
            try
            {
                var _Model = _context.USCensus_EEO_OccupationCodes.Where(e => e.EEOOccupationCodeID == CensusOccupationsId).FirstOrDefault().USCensus_EEOJobClasificationCategories.ToList();
                var _ListModel = new List<SelectListItem>();
                _ListModel.AddRange(_Model.Select(g => new SelectListItem { Text = g.Code.ToString() + " - " + g.Description, Value = g.EEOJobClasificationCategoryID.ToString() }).ToList());
                return _ListModel;
            }
            catch (Exception ex)
            {
                AppUtility.LogMessage(ex, "GetEEOCategories", "ALMByFederalJobCodesReportService.cs");
                throw;
            }
        }
        //employmentstatus
    }
}
