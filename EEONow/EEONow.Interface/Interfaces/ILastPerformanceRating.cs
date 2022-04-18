using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface ILastPerformanceRating
    {
        Task<List<LastPerformanceRatingModel>> GetLastPerformanceRatingModel();
        Task<ResponseModel> CreateLastPerformanceRating(LastPerformanceRatingModel _model);
        Task<ResponseModel> UpdateLastPerformanceRating(LastPerformanceRatingModel _model);
        Task<List<SelectListItem>> BindLastPerformanceRatingDropDown(int? organizationID);
    }
}
