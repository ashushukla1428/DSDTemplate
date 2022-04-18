using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IALMByEEOCategoryReportService
    {
        ALMByEEOCategoryReportModel GetALMByEEOCategoryReportService(int? USCensusVersionID, bool? searchByWorkSite, int? occupationCodeID, int? stateID, string puma_codes,string employmentstatus);
        ALMByEEOCategoryReportModel GetALMByEEOCategoryExportService(int? USCensusVersionID, bool? searchByWorkSite, int? occupationCodeID, int? stateID, string puma_codes, string employmentstatus);
        List<SelectListItem> GetUSCensusDataVersionDropDown();
        List<SelectListItem> GetStateDropDown();
        List<SelectListItem> GetUSCensusOccupationsDropDown();
        List<SelectListItem> GetUSCensusGeographyTypesDropDown();
        List<SelectListItem> GetPUMACodesDropDown(int? StateID,int? uscensusgeographytypesid);
        List<SelectListItem> GetEmploymentStatusDropDown(int? uscensusgeographytypesid);
    }
}
