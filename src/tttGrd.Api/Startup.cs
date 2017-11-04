using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
[assembly: OwinStartup(typeof(tttGrd.Api.Startup))]

namespace tttGrd.Api
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Any connection or hub wire up and configuration should go here
      app.UseCors(CorsOptions.AllowAll);
      app.MapSignalR();
    }
  }
}