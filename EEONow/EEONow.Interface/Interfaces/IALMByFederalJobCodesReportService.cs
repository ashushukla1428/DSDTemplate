using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IALMByFederalJobCodesReportService
    {
        ALMByFederalJobCodesReportModel GetALMByFederalJobCodesReportService(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs);
        ALMByFederalJobCodesReportModel GetALMByFederalJobCodesExportService(int? uSCensusVersionID, int? stateID, int? eEOOccupationCodeID, int? eEOCategoryCodeNbr, int? searchByWorkSite, string eSRCodes, string pUMA_CODES, int? majorOccupationGroup, int? minorOccupationGroupID, int? boardingOccupationGroupID, string occupationIDs);
        List<SelectListItem> GetUSCensusDataVersionDropDown();
        List<SelectListItem> GetStateDropDown();
        List<SelectListItem> GetUSCensusOccupationsDropDown();
        List<SelectListItem> GetUSCensusGeographyTypesDropDown();
        List<SelectListItem> GetPUMACodesDropDown(int? StateID,int? uscensusgeographytypesid);
        List<SelectListItem> GetEmploymentStatusDropDown(int? uscensusgeographytypesid);



        List<SelectListItem> GetOccupations(int BoardingId);
        List<SelectListItem> GetBoardingOccupationalGroup(int MinorId);
        List<SelectListItem> GetMinorOccupationalGroup(int MajorId);
        List<SelectListItem> GetMajorOccupationalGroup();
        List<SelectListItem> GetEEOCategories(int CensusOccupationsId);
        List<SelectListItem> GetOccupationswithEEo(int EEOCategoriesId);

    }
}
