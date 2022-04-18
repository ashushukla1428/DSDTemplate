using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Models
{
    public class ALMByFederalJobCodesReportModel
    {
        public String DiplatTitleName { get; set; }
        public String DiplatContent { get; set; }
        public List<RacesForALMByFederalJobCodes> ListOfRaces { get; set; }
        public List<ALMByFederalJobCodes> ListALMByFederalJobCodes { get; set; }
        public int? GrandTotal { get; set; }

        public int? mTotalWhite { get; set; }
        public int? fTotalWhite { get; set; }
        public int? mTotalBlackOrAfricanAmerican { get; set; }
        public int? fTotalBlackOrAfricanAmerican { get; set; }
        public int? hTotalMale { get; set; }
        public int? hTotalFemale { get; set; }
        public int? mTotalAsian { get; set; }
        public int? fTotalAsian { get; set; }
        public int? mTotalAmericanIndianORAlaskaNative { get; set; }
        public int? fTotalAmericanIndianORAlaskaNative { get; set; }
        public int? mTotalNativeHawaiianOrOtherPacificIslander { get; set; }
        public int? fTotalNativeHawaiianOrOtherPacificIslander { get; set; }
        public int? mTotalTwoOrMoreRaces { get; set; }
        public int? fTotalTwoOrMoreRaces { get; set; }
    }
    public class RacesForALMByFederalJobCodes
    {
        public int RacesId { get; set; }
        public string RacesName { get; set; }
    }
    public class ALMByFederalJobCodes
    {
        public String EEOCategoryNbr { get; set; }
        public String EEOCategoryDesc { get; set; }
        public int? hFemale { get; set; }
        public int? hMale { get; set; }
        public int? mWhite { get; set; }
        public int? fWhite { get; set; }
        public int? mBlackOrAfricanAmerican { get; set; }
        public int? fBlackOrAfricanAmerican { get; set; }
        public int? mNativeHawaiianOrOtherPacificIslander { get; set; }
        public int? fNativeHawaiianOrOtherPacificIslander { get; set; }
        public int? mAsian { get; set; }
        public int? fAsian { get; set; }
        public int? mAmericanIndianORAlaskaNative { get; set; }
        public int? fAmericanIndianORAlaskaNative { get; set; }
        public int? mTwoOrMoreRaces { get; set; }
        public int? fTwoOrMoreRaces { get; set; }
        public int? Total { get; set; }

    }


}
