using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pickup_Management.Startup))]
namespace Pickup_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
