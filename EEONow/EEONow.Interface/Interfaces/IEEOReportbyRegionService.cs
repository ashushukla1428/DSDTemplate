using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IEEOReportbyRegionService
    {
        EEOReportbyRegionModel GetEEOReportbyRegionService(int? OrganizationId, int? FileSubmissionId,string region);
        EEOReportbyRegionModel GetEEOExportbyRegionService(int? OrganizationId, int? FileSubmissionId,string region);
        List<SelectListItem> BindEmployeeRegionDropDown(int? OrganizationId, int? FileSubmissionId);

    }
}
