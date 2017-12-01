using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MidlandsFly.Startup))]
namespace MidlandsFly
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
