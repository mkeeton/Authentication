using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Http.Dispatcher;

namespace Authentication.API
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    private static IWindsorContainer container;

    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      BootstrapContainer();
    }

    private static void BootstrapContainer()
    {
      container = new WindsorContainer()
          .Install(FromAssembly.This());
      var controllerFactory = new IOC.CastleWindsor.Factories.WindsorControllerFactory(container.Kernel);
      ControllerBuilder.Current.SetControllerFactory(controllerFactory);
      GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new IOC.CastleWindsor.Factories.WindsorHttpControllerActivator(container));
    }

    protected void Application_End(object sender, EventArgs e)
    {
      container.Dispose();
    }

    public static IWindsorContainer GetContainer()
    {
      return container;
    }
  }
}
