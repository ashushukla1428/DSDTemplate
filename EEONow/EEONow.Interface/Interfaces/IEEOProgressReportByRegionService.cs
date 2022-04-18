using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IEEOProgressReportByRegionService
    {
        EEOProgressReportByRegionModel GetEEOProgressReportByRegion(int? OrganizationId, int? FileSubmissionId,string region, string eeoprogramoffice, string begindate, string enddate,string supervisor);
        EEOProgressReportByRegionModel GetEEOExportbyProgressRegion(int? OrganizationId, int? FileSubmissionId,string region, string eeoprogramoffice, string begindate, string enddate,string supervisor);
        List<SelectListItem> BindEmployeeProgressRegionDropDown(int? OrganizationId, int? FileSubmissionId);

    }
}
