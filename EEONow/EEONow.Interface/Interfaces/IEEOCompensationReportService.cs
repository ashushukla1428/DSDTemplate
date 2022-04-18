using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IEEOCompensationReportService
    {
        EEOCompensationReportModel GetEEOCompensationReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle ,string region);
        EEOCompensationReportModel GetEEOCompensationExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle, string region);
        List<SelectListItem> BindEEOJobCategoryDropDown(int? OrganizationId);
        List<SelectListItem> BindEEOProgramOfficeDropDown(Int32 OrganizationId, Int32 FileSubmissionId);
        List<SelectListItem> BindEEOPositionTitleDropDown(Int32 OrganizationId, Int32 FileSubmissionId, int EEOEEOJobCategoryId, string EEOProgramOffice, string region);
    }
}
