using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartupAttribute(typeof(Authentication.API2.AuthConfig))]
namespace Authentication.API2
{
  public class AuthConfig
  {
    public void Configuration(IAppBuilder app)
    {
      HttpConfiguration httpConfig = new HttpConfiguration();

      ConfigureOAuth(app);

      ConfigureOAuthTokenGeneration(app);

      ConfigureOAuthTokenConsumption(app);

      ConfigureWebApi(httpConfig);

      app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

      app.UseWebApi(httpConfig);

    }

    private void ConfigureOAuth(IAppBuilder app)
    {
      // Configure the db context and user manager to use a single instance per request
      //app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<Infrastructure.Managers.ApplicationUserManager>(Infrastructure.Managers.ApplicationUserManager.Create);
    }

    private void ConfigureOAuthTokenGeneration(IAppBuilder app)
    {


      // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here

      //OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
      //{
      //  //For Dev enviroment only (on production should be AllowInsecureHttp = false)
      //  AllowInsecureHttp = true,
      //  TokenEndpointPath = new PathString("/oauth/token"),
      //  AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
      //  //Provider = new Providers.CustomOAuthProvider(Authentication.API.Global.GetContainer().Kernel.Resolve<Infrastructure.Managers.ApplicationUserManager>()),
      //  Provider = new Providers.CustomOAuthProvider(HttpContext.Current.GetOwinContext().GetUserManager<Infrastructure.Managers.ApplicationUserManager>()),
      //  AccessTokenFormat = new Providers.CustomJwtFormat("http://localhost:50378")
      //};

      OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
      {
        //For Dev enviroment only (on production should be AllowInsecureHttp = false)
        AllowInsecureHttp = true,
        TokenEndpointPath = new PathString("/oauth/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        Provider = new Providers.CustomOAuthProvider(),
        AccessTokenFormat = new Providers.CustomJwtFormat("http://localhost:59822")
      };

      // OAuth 2.0 Bearer Access Token Generation
      app.UseOAuthAuthorizationServer(OAuthServerOptions);

    }

    private void ConfigureOAuthTokenConsumption(IAppBuilder app)
    {

      var issuer = "http://localhost:50378";
      string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
      byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

      // Api controllers with an [Authorize] attribute will be validated with JWT
      app.UseJwtBearerAuthentication(
          new JwtBearerAuthenticationOptions
          {
            AuthenticationMode = AuthenticationMode.Active,
            AllowedAudiences = new[] { audienceId },
            IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
          });
    }

    private void ConfigureWebApi(HttpConfiguration config)
    {
      var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
      jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
  }
}