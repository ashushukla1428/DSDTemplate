using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IEEOGenderCompensationReportService
    {
        EEOGenderCompensationReportModel GetEEOGenderCompensationReportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle,string region);
        EEOGenderCompensationReportModel GetEEOGenderCompensationExportService(int? OrganizationId, int? FileSubmissionId, int? EEOJobCategory, int jobtitlesortorder, string EEOProgramOffice, string EEOPositionTitle, string region);

    }
}
