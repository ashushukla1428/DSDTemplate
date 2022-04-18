using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IGendersService
    { 
        Task<List<GenderModel>> GetGenders();         
        Task<ResponseModel> UpdateGender(GenderModel model);                   
    }
}
