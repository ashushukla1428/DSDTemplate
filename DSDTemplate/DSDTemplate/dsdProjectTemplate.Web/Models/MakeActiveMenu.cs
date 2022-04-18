using System;
using System.Web;
using System.Web.Mvc;

namespace dsdProjectTemplate.Web.Models
{
    public static class MakeActiveMenu
    {
        public static string MakeActive(string controller)
        {
            string result = "active";
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string controllerName = urlHelper.RequestContext.RouteData.Values["controller"].ToString();

            if (!controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
            {
                result = null;
            }

            return result;
        }
    }
}