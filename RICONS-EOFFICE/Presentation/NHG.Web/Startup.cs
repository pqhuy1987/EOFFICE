using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(NHG.Web.Startup))]
namespace NHG.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
