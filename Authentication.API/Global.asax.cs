using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Routing;

namespace Authentication.API
{
  public class Global : System.Web.HttpApplication
  {

    private static IWindsorContainer container;

    protected void Application_Start(object sender, EventArgs e)
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);
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

    protected void Session_Start(object sender, EventArgs e)
    {

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_Error(object sender, EventArgs e)
    {

    }

    protected void Session_End(object sender, EventArgs e)
    {

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