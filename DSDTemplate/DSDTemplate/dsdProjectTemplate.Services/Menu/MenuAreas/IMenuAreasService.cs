using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu.MenuAreas
{
    public interface IMenuAreasService
    {
        List<SelectListItem> GetAreas();
    }
}
