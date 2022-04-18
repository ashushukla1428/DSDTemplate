using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEONow.Models.Models.EEO4Report
{
    public class EEO4ReportViewModel
    {
        public String ManagerName { get; set; }
        public int TotalWorkforce { get; set; }
        public List<EEOForALM> ListEEOForALM { get; set; }
        public List<RacesForALM> ListRacesForALM { get; set; }
        public List<EEO4ComputeALMValue> ListComputeALMValue { get; set; }
    }
    public class EEO4ComputeALMValue
    {
        public int RacesId { get; set; }
        public int EEOId { get; set; }

        //0 -> 15999.99
        public int TotalWorkforceMale_Between0K_n_15K { get; set; }
        public int TotalWorkforceFemale_Between0K_n_15K { get; set; }

        // 16000.00 -> 19999.99
        public int TotalWorkforceMale_Between16K_n_19K { get; set; }
        public int TotalWorkforceFemale_Between16K_n_19K { get; set; }

        // 20000.00 -> 24999.99
        public int TotalWorkforceMale_Between20K_n_24K { get; set; }
        public int TotalWorkforceFemale_Between20K_n_24K { get; set; }
        // 25000.00 -> 32999.99
        public int TotalWorkforceMale_Between25K_n_32K { get; set; }
        public int TotalWorkforceFemale_Between25K_n_32K { get; set; }
        // 33000.00 -> 42999.99
        public int TotalWorkforceMale_Between33K_n_42K { get; set; }
        public int TotalWorkforceFemale_Between33K_n_42K { get; set; }
        // 43000.00 -> 54999.99
        public int TotalWorkforceMale_Between43K_n_54K { get; set; }
        public int TotalWorkforceFemale_Between43K_n_54K { get; set; }
        // 55000.00 -> 69999.99
        public int TotalWorkforceMale_Between55K_n_69K { get; set; }
        public int TotalWorkforceFemale_Between55K_n_69K { get; set; }
        //70000.00 -> --------
        public int TotalWorkforceMale_Greater_Than_70K { get; set; }
        public int TotalWorkforceFemale_Greater_Than_70K { get; set; }
    }

}
