using System;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Unity.WebApi;
using tttGrd.Api.Persistence;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using tttGrd.Api.Hubs;

namespace tttGrd.Api
{
  public class SignalRUnityDependencyResolver : DefaultDependencyResolver
  {
    private IUnityContainer _container;
    public SignalRUnityDependencyResolver(IUnityContainer container)
    {
      _container = container;
    }

    public override object GetService(Type serviceType)
    {
      if (_container.IsRegistered(serviceType)) return _container.Resolve(serviceType);
      else return base.GetService(serviceType);
    }
    public override IEnumerable<object> GetServices(Type serviceType)
    {
      if (_container.IsRegistered(serviceType)) return _container.ResolveAll(serviceType);
      else return base.GetServices(serviceType);
    }
  }

  public class UnityHubActivator : IHubActivator
  {
    private readonly IUnityContainer _container;

    public UnityHubActivator(IUnityContainer container)
    {
      _container = container;
    }

    public IHub Create(HubDescriptor descriptor)
    {
      if (descriptor == null)
      {
        throw new ArgumentNullException("descriptor");
      }

      if (descriptor.HubType == null)
      {
        return null;
      }

      object hub = _container.Resolve(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
      return hub as IHub;
    }
  }

  public class UnityConfiguration
  {
    #region Unity Container
    private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
    {
      var container = new UnityContainer();
      RegisterTypes(container);
      return container;
    });

    public static IUnityContainer GetConfiguredContainer()
    {
      return container.Value;
    }
    #endregion

    public static void RegisterTypes(IUnityContainer container)
    {
      container.RegisterType<GameHub, GameHub>(new ContainerControlledLifetimeManager());
      container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());
    }
  }

  public static class UnityConfig
  {
    public static void RegisterComponents()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();

      GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
      GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(container));
      container.RegisterType<IDatabaseRepository, DatabaseRepository>(new ContainerControlledLifetimeManager());
      container.RegisterType<IKeyGenerator, KeyGenerator>(new ContainerControlledLifetimeManager());
      container.RegisterType<IGamerRepository, GamerRepository>(new ContainerControlledLifetimeManager());
      container.RegisterType<IVault, Vault>(new ContainerControlledLifetimeManager());
      container.RegisterType<IAgniKaiRepository, AgniKaiRepository>(new ContainerControlledLifetimeManager());
    }
  }
}