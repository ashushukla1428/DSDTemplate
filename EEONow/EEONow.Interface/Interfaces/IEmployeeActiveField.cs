using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IEmployeeActiveFieldService
    { 
        Task<List<EmployeeActiveFieldModel>> GetEmployeeActiveFieldModel(int? organization, int? roleid);
         
        Task<ResponseModel> UpdateEmployeeActiveField(EmployeeActiveFieldModel _model);
 
    }
}
