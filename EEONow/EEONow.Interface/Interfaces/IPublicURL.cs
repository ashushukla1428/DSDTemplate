using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IPublicURL
    {




        Dictionary<string, string> GetUserLabel(int organisastionId, int RoleId);






        string GetdbCurrentSelectedYear(Int32 OrgainisationId); 




        String GetdbCurrentUserFile(Int32 OrgainisationId);
        Dictionary<string, string> GetdbCurrentOrganisationColorCode(Int32 OrgainisationId, int RoleId);
        Dictionary<string, string> GetdbDynamicColorChanger(Int32 OrgainisationId,String Selectedvalue);
        List<String> GetdbCsvFileList(string year,Int32 OrgainisationId); 
        Dictionary<string, string> GetDBLegendList(Int32 _organizationId, string Selectedvalue, string filename);
        Dictionary<Int32, Int32> GetOrgainisationIdViaToken(String Token);
        List<PublicURLModel> GetPublicURL();
        Dictionary<string, string> GetLegendCollections(Int32 OrgainisationId,string filename);

        List<string> GetdbSelectedYearList(Int32 OrgainisationId);


        bool ReGenerateKey(int OrgainisationId);
        bool ActivateUrl(int OrgainisationId);

    }
}
