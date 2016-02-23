using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Authentication.Domain.Models;
using System.Configuration;
using Authentication.Framework;

namespace Authentication.API.Providers
{
  public class CustomOAuthProvider : OAuthAuthorizationServerProvider
  {

    //Authentication.API.Infrastructure.ExtendedUnitOfWork _unitOfWork;

    Client client = null;
    
    //public Authentication.API.Infrastructure.ExtendedUnitOfWork UnitOfWork
    //{
    //  get
    //  {
    //    if(_unitOfWork==null)
    //    {
    //      _unitOfWork = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<Authentication.API.Infrastructure.ExtendedUnitOfWork>();
    //    }
    //    return _unitOfWork;
    //  }
    //}
    
    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {

      string clientId = string.Empty;
      string clientSecret = string.Empty;
      

      if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
      {
        context.TryGetFormCredentials(out clientId, out clientSecret);
      }

      if (context.ClientId == null)
      {
        //Remove the comments from the below line context.SetError, and invalidate context 
        //if you want to force sending clientId/secrects once obtain access tokens. 
        context.Validated();
        //context.SetError("invalid_clientId", "ClientId should be sent.");
        return Task.FromResult<object>(null);
      }

      Infrastructure.ExtendedUnitOfWork _unitOfWork = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<Authentication.API.Infrastructure.ExtendedUnitOfWork>();

      client = _unitOfWork.ClientStore.FindByURL(context.ClientId).Result;


      if (client == null)
      {
        context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
        return Task.FromResult<object>(null);
      }

      if (!client.Active)
      {
        context.SetError("invalid_clientId", "Client is inactive.");
        return Task.FromResult<object>(null);
      }

      //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

      context.Validated();
      return Task.FromResult<object>(null);
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {

      Infrastructure.ExtendedUnitOfWork _unitOfWork = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<Authentication.API.Infrastructure.ExtendedUnitOfWork>();

      var allowedOrigin = "NONE";

      List<ClientAllowedOrigin> _allowedOrigins;

      if(!(client==null))
      { 
        _allowedOrigins = client.AllowedOrigins;
      }
      else
      {
        _allowedOrigins = await _unitOfWork.ClientStore.ListAllowedOrigins(new Guid());
      }
      string _originPath = context.Request.Headers["Origin"];
      string _originCompare = _originPath.ToLower().Trim();
      if(_originCompare.LastIndexOf("/")!=(_originCompare.Length-1)) _originCompare += "/";
      foreach (ClientAllowedOrigin _origin in _allowedOrigins)
      {
        string _allowedCompare = _origin.AllowedURL.ToLower().Trim();
        if ((_allowedCompare.LastIndexOf("/") != (_allowedCompare.Length - 1)) && (_allowedCompare!="*")) _allowedCompare += "/";
        if ((_origin.AllowedURL.ToLower().Trim() == "*") || (_origin.AllowedURL.ToLower().Trim() == _originCompare))
        {
          allowedOrigin = _originPath;
          break;
        }
      }

      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

      User user = await _unitOfWork.UserManager.FindAsync(context.UserName, context.Password);

      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      //if (!user.EmailConfirmed)
      //{
      //  context.SetError("invalid_grant", "User did not confirm email.");
      //  return;
      //}

      ClaimsIdentity oAuthIdentity = await _unitOfWork.UserManager.GenerateUserIdentityAsync(user, "JWT");
      oAuthIdentity.AddClaims(_unitOfWork.ClaimStore.GetClaims(user));
      //oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));

      AuthenticationProperties prop = new AuthenticationProperties();
      prop.Dictionary.Add("as:issuer", "http://localhost:50378");
      prop.Dictionary.Add("as:user_id", NullHandlers.NES(user.Id));
      prop.Dictionary.Add("as:client_id", ConfigurationManager.AppSettings["as:AudienceId"]);
      prop.Dictionary.Add("as:client_secret", ConfigurationManager.AppSettings["as:AudienceSecret"]);
      var ticket = new AuthenticationTicket(oAuthIdentity, prop);

      context.Validated(ticket);

    }

    public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
    {

      // Change auth ticket for refresh token requests
      var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

      var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
      if (newClaim != null)
      {
        newIdentity.RemoveClaim(newClaim);
      }
      newIdentity.AddClaim(new Claim("newClaim", "newValue"));

      AuthenticationProperties prop = context.Ticket.Properties;
      //prop.Dictionary.Add("as:refresh_client_id", ConfigurationManager.AppSettings["as:AudienceId"]);
      //prop.Dictionary.Add("as:refresh_client_secret", ConfigurationManager.AppSettings["as:AudienceSecret"]);
      var ticket = new AuthenticationTicket(newIdentity, prop);
      
      context.Validated(ticket);

      return Task.FromResult<object>(null);
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      //foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
      //{
      //  context.AdditionalResponseParameters.Add(property.Key, property.Value);
      //}

      return Task.FromResult<object>(null);
    }

    public override Task MatchEndpoint(OAuthMatchEndpointContext context)
    {
      if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
      {
        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
        context.RequestCompleted();
        return Task.FromResult(0);
      }

      return base.MatchEndpoint(context);
    }
  }
}