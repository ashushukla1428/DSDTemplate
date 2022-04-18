using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IJobsByEEOCategoryReportService
    {
        JobsByEEOCategoryReportModel GetJobsByEEOCategoryReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice ,string region);
        JobsByEEOCategoryReportModel GetJobsByEEOCategoryExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, string EEOProgramOffice, string region);        
        List<SelectListItem> BindEEOJobCategoryDropDown(int? OrganizationId);
        List<SelectListItem> BindEEOProgramOfficeDropDown(Int32 OrganizationId, Int32 FileSubmissionId);

    }
}
