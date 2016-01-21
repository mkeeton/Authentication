using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Authentication.API2
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{action}/{id}",
          defaults: new { action = "Home", id = RouteParameter.Optional }
      );
    }
  }
}
