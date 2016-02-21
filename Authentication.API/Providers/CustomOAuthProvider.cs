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

namespace Authentication.API.Providers
{
  public class CustomOAuthProvider : OAuthAuthorizationServerProvider
  {

    Authentication.API.Infrastructure.ExtendedUnitOfWork _unitOfWork;

    //public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    //{
    //  context.Validated();
    //  return Task.FromResult<object>(null);
    //}
    Client client = null;
    
    public Authentication.API.Infrastructure.ExtendedUnitOfWork UnitOfWork
    {
      get
      {
        if(_unitOfWork==null)
        {
          _unitOfWork = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<Authentication.API.Infrastructure.ExtendedUnitOfWork>();
        }
        return _unitOfWork;
      }
    }
    
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

      client = UnitOfWork.ClientStore.FindByURL(context.ClientId).Result;


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

      context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

      context.Validated();
      return Task.FromResult<object>(null);
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {

      var allowedOrigin = "NONE";

      List<ClientAllowedOrigin> _allowedOrigins;

      if(!(client==null))
      { 
        _allowedOrigins = client.AllowedOrigins;
      }
      else
      {
        _allowedOrigins = await UnitOfWork.ClientStore.ListAllowedOrigins(new Guid());
      }
      foreach (ClientAllowedOrigin _origin in _allowedOrigins)
      {
        if ((_origin.AllowedURL.ToLower().Trim() == "*") || (_origin.AllowedURL.ToLower().Trim() == (context.Request.Headers["Origin"]).ToLower().Trim()))
        {
          allowedOrigin = context.Request.Headers["Origin"];
          break;
        }
      }

      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

      var userManager = UnitOfWork.UserManager;

      User user = await userManager.FindAsync(context.UserName, context.Password);

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

      ClaimsIdentity oAuthIdentity = await userManager.GenerateUserIdentityAsync(user, "JWT");
      oAuthIdentity.AddClaims(_unitOfWork.ClaimStore.GetClaims(user));
      //oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));
      var currentSession = new UserSession(){UserId=user.Id};
      await UnitOfWork.UserSessionStore.CreateAsync(currentSession);
      oAuthIdentity.AddClaim(new Claim("authSessionId",currentSession.Id.ToString()));
      AuthenticationProperties prop = new AuthenticationProperties();
      prop.Dictionary.Add("as:issuer", "http://localhost:50378");
      prop.Dictionary.Add("as:client_id", ConfigurationManager.AppSettings["as:AudienceId"]);
      prop.Dictionary.Add("as:client_secret", ConfigurationManager.AppSettings["as:AudienceSecret"]);
      var ticket = new AuthenticationTicket(oAuthIdentity, prop);

      context.Validated(ticket);

    }

    public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
    {
      //var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
      //var currentClient = context.ClientId;

      //if (originalClient != currentClient)
      //{
      //  context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
      //  return Task.FromResult<object>(null);
      //}

      // Change auth ticket for refresh token requests
      var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

      var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
      if (newClaim != null)
      {
        newIdentity.RemoveClaim(newClaim);
      }
      newIdentity.AddClaim(new Claim("newClaim", "newValue"));

      AuthenticationProperties prop = context.Ticket.Properties;
      prop.Dictionary.Add("as:issuer", "http://localhost:50378");
      prop.Dictionary.Add("as:client_id", ConfigurationManager.AppSettings["as:AudienceId"]);
      prop.Dictionary.Add("as:client_secret", ConfigurationManager.AppSettings["as:AudienceSecret"]);
      var ticket = new AuthenticationTicket(newIdentity, prop);
      
      context.Validated(ticket);

      return Task.FromResult<object>(null);
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
      {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }

      return Task.FromResult<object>(null);
    }
  }
}