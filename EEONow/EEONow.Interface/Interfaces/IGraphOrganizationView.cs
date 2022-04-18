using EEONow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EEONow.Interfaces
{
    public interface IGraphOrganizationViewService
    {
        Task<List<GraphOrganizationViewModel>> GetGraphOrganizationViewModel();
        Task<ResponseModel> UpdateGraphOrganizationView(GraphOrganizationViewModel _model);
        Task<List<SelectListItem>> BindGraphOrganizationViewDropDown();
        String GraphOrganizationViewList(Int32 organisationId, Int32 roleid);
        Task<List<AssigGraphOrganizationViewModel>> GetAssignedGraphOrganizationViewModel(int? organization, int? roleid);
        ResponseModel UpdateAssignedGraphOrganizationView(AssigGraphOrganizationViewModel _model);
        List<SelectListItem> BindGraphOrganizationViewViaOrganisationIdDropDown(int OrganisationId);
    }
}
