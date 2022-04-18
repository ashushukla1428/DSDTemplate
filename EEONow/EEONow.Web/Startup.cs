using EEONow.Services;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
//using HangFire.Models;
using Microsoft.Owin;
using Owin;
using System;
using System.Web.Mvc;
using System.Web.UI;

[assembly: OwinStartupAttribute(typeof(EEONow.Web.Startup))]
namespace EEONow.Web
{
    
    //[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(System.Configuration.ConfigurationManager.ConnectionStrings["HangFireString"].ConnectionString);
            HangfireService _HangfireService = new HangfireService();
            RecurringJob.AddOrUpdate("Generate Dashboard CSV files", () => _HangfireService.GenerateDashboardData(), Cron.MinuteInterval(3));
             
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DSDWebFilter() }
            });
            //app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
        public class DSDWebFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.
                var owinContext = new OwinContext(context.GetOwinEnvironment());

                // Allow  authenticated users who have "DefinedSoftwareAdministrator" role to see the Dashboard
                //if (owinContext.Authentication.User.IsInRole("DefinedSoftwareAdministrator"))
                //{
                return owinContext.Authentication.User.Identity.IsAuthenticated;
                //}
               // return false;
            }
        }

    }
}
