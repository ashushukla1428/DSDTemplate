using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeesModel>> GetEmployeeModel();

        Task<List<EmployeesModel>> GetEmployeeViaOrganizationsModel(int FileSubmissionId, string region);//, int FilterCall);

        Task<List<EmployeeModelForExport>> GetExportEmployeeViaOrganizationsModel(int FileSubmissionId, string region);

        Task<ResponseModel> UpdateEmployee(EmployeesModel _model);
        EmployeeSearchModel BindEmployeeFilterModel();

        Task<List<FileSubmissionModel>> GetFileSubmissionModel();
        List<SelectListItem> GetFileSubmissionViaOrganisation(int? organization);
        Task<List<SelectListItem>> BindPositionTitleDropDown();
        Task<List<SelectListItem>> BindProgramOfficeDropDown();
        Task<List<SelectListItem>> BindEmployeeLevelDropDown(int? organizationID);
        Task<List<SelectListItem>> BindEmployeeActiveColumn();
        Task<List<SelectListItem>> GetSupervisorsViaOrganisation(int organization, int FileSubmissionId);
        Task<List<SelectListItem>> GetSupervisorsViaOrgPrgOffReg(int organization, int FileSubmissionId,string prgoffice,string region);
    }
}
