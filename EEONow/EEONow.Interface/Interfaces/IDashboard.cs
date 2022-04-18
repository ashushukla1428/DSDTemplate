using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IDashboard
    {
        String GetdbCurrentUserFile();
        Dictionary<string, string> GetdbCurrentOrganisationColorCode();

        Dictionary<string, string> GetdbDynamicColorChanger(String Selectedvalue);
        List<String> GetdbCsvFileList(string year);

        Dictionary<string, string> GetDBLegendList(string Selectedvalue, string filename);

        Dictionary<string, string> GetLegendCollections(string filename);

        Dictionary<string, string> GetUserLabel(int organisastionId);

        OrhChartDashborad BindOrhChartDashborad();

        OrhChartDashborad GetFileSubmissionDetail(int FileSubmissionId);

        string GetdbCurrentSelectedYear();
        List<string> GetdbSelectedYearList();
        //Dictionary<string, Dictionary<string, string>> GetDBLegendList(string Selectedvalue, string filename);
    }
}
