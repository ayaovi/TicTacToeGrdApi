using Microsoft.Practices.Unity;
using System.Web.Http;
using tttGrd.Api.Persistence;
using Unity.WebApi;

namespace tttGrd.Api
{
  public static class UnityConfig
  {
    public static void RegisterComponents()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();

      GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
      container.RegisterType<IDatabaseRepository, DatabaseRepository>();
      container.RegisterType<IKeyGenerator, KeyGenerator>();
      container.RegisterType<IGamerRepository, GamerRepository>();
      container.RegisterType<IVault, Vault>();
      container.RegisterType<IAgniKaiRepository, AgniKaiRepository>();
    }
  }
}