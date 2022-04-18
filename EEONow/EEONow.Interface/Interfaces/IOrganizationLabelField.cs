using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{ 
    public interface IOrganizationLabelFieldService
    { 
        Task<List<OrganizationLabelFieldModel>> GetOrganizationLabelFieldModel(int? organization, int? roleid);
         
        Task<ResponseModel> UpdateOrganizationLabelField(OrganizationLabelFieldModel _model);
 
    }
}
