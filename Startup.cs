using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcToDoList.Startup))]
namespace MvcToDoList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
