using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IAvailableLaborMarketService
    {
        AvailableLaborMarketFileVersionModel GetAvailableLaborMarketDataViaFileVersion(Int32 AvailableLaborMarketFileVersionId);
        AvailableLaborMarketFileVersionModel GetAvailableLaborMarketData();
        ResponseModel AddNewLaborMarketData();
        ResponseModel SaveAvailableLaborMarketData(AvailableLaborMarketFileVersionModel _AvailableLaborMarketFileVersion);
        String PDFAvailableLaborMarketData(AvailableLaborMarketFileVersionModel _AvailableLaborMarketFileVersion);
        List<SelectListItem> GetFileVersionMarketData();
        void MarkasCurrentLaborMarketData(int AvailableLaborMarketFileVersionId);         
    }
}
