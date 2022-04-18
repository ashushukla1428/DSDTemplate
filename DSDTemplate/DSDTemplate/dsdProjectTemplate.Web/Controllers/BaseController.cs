using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Controllers
{
    [Authorize]
    [CustomAuthorizeFilter]
    public class BaseController: Controller
    {
    }
}