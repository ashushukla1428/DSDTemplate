using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IALMViaRacesEeoService
    {
        ALMViaRacesEeoModel GetAvailableLaborMarketService(Int32 OrganizationId, Int32 FileSubmissionId);

        ViewIndexReportModel GetIndexReport(int organisastionId, int FileSubmissionID, String position);
        ALMViaRacesEeoModel GetAvailableLaborMarketReportByEmpId(Int32 OrganizationId, Int32 FileSubmissionId, string empPosition);

    }
}
