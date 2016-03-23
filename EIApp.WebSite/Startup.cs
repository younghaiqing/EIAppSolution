using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EIApp.WebSite.Startup))]
namespace EIApp.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
