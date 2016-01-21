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

namespace Authentication.API2.Providers
{
  public class CustomOAuthProvider : OAuthAuthorizationServerProvider
  {

    public CustomOAuthProvider()
    {
    }

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      context.Validated();
      return Task.FromResult<object>(null);
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {

      var allowedOrigin = "*";

      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

      var userManager = context.OwinContext.GetUserManager<Infrastructure.Managers.ApplicationUserManager>();

      User user = await userManager.FindAsync(context.UserName, context.Password);

      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      if (!user.EmailConfirmed)
      {
        context.SetError("invalid_grant", "User did not confirm email.");
        return;
      }

      ClaimsIdentity oAuthIdentity = await userManager.GenerateUserIdentityAsync(user, "JWT");
      //oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
      //oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));

      var ticket = new AuthenticationTicket(oAuthIdentity, null);

      context.Validated(ticket);

    }
  }
}