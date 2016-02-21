using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Authentication.Domain.Models;
using Authentication.Framework;

namespace Authentication.API.Providers
{
  public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
  {

    Authentication.API.Infrastructure.ExtendedUnitOfWork _unitOfWork;

    public Authentication.API.Infrastructure.ExtendedUnitOfWork UnitOfWork
    {
      get
      {
        if (_unitOfWork == null)
        {
          _unitOfWork = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<Authentication.API.Infrastructure.ExtendedUnitOfWork>();
        }
        return _unitOfWork;
      }
    }

    public async Task CreateAsync(AuthenticationTokenCreateContext context)
    {
      Guid userid = NullHandlers.NGUID(context.Ticket.Properties.Dictionary["as:user_id"]);

      if (userid==Guid.Empty)
      {
        return;
      }

      var refreshTokenId = Guid.NewGuid().ToString("n");

      var refreshTokenLifeTime = 30;

      var token = new RefreshToken()
      {
        Id = Infrastructure.Encryption.GetHash(refreshTokenId),
        UserId = userid,
        IssuedUtc = DateTime.UtcNow,
        ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
      };

      context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
      context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

      token.ProtectedTicket = context.SerializeTicket();

      var result = await UnitOfWork.RefreshTokenStore.CreateAsync(token);

      if (result)
      {
        context.SetToken(refreshTokenId);
      }
    }

    public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
    {

      var allowedOrigin = "NONE";

      List<ClientAllowedOrigin> _allowedOrigins;

      _allowedOrigins = await UnitOfWork.ClientStore.ListAllowedOrigins(new Guid());

      foreach (ClientAllowedOrigin _origin in _allowedOrigins)
      {
        if ((_origin.AllowedURL.ToLower().Trim() == "*") || (_origin.AllowedURL.ToLower().Trim() == (context.Request.Headers["Origin"]).ToLower().Trim()))
        {
          allowedOrigin = context.Request.Headers["Origin"];
          break;
        }
      }

      //var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

      string hashedTokenId = Infrastructure.Encryption.GetHash(context.Token);

        var refreshToken = await UnitOfWork.RefreshTokenStore.FindById(hashedTokenId);

        if (refreshToken != null)
        {
          //Get protectedTicket from refreshToken class
          context.DeserializeTicket(refreshToken.ProtectedTicket);
          var result = await UnitOfWork.RefreshTokenStore.DeleteAsync(hashedTokenId);
        }
    }

    public void Create(AuthenticationTokenCreateContext context)
    {
      throw new NotImplementedException();
    }

    public void Receive(AuthenticationTokenReceiveContext context)
    {
      throw new NotImplementedException();
    }
  }
}