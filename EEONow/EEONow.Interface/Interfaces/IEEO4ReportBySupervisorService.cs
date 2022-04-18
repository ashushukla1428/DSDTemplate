using EEONow.Models;
using EEONow.Models.Models.EEO4Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Interface.Interfaces
{
    public interface IEEO4ReportBySupervisorService
    {

        EEO4ReportViewModel GetEEO4ReportBySupervisor(Int32 OrganizationId, Int32 FileSubmissionId, string empPosition);
        List<string> GetEEO4ReportSalaryRangeList();
    }
}
