using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IEEOJobCategoryService
    {
        Task<List<EEOJobCategoriesModel>> GetEEOJobCategoryModel();
        Task<ResponseModel> CreateEEOJobCategory(EEOJobCategoriesModel _model);
        Task<ResponseModel> UpdateEEOJobCategory(EEOJobCategoriesModel _model);
        Task<List<SelectListItem>> BindEEOJobCategoryDropDown(int? organizationID);
    }
}
