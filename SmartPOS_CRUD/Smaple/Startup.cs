using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Smaple.Startup))]
namespace Smaple
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
