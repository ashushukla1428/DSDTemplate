using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImportTest.Startup))]
namespace ImportTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
