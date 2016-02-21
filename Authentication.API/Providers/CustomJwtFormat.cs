using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace Authentication.API.Providers
{
  public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
  {

    private string _issuer = string.Empty;

    public CustomJwtFormat(string issuer)
    {
      _issuer = issuer;
    }

    public string Protect(AuthenticationTicket data)
    {
      if (data == null)
      {
        throw new ArgumentNullException("data");
      }

      if(_issuer=="")
      {
        _issuer = data.Properties.Dictionary["as:issuer"];
      }

      string audienceId = data.Properties.Dictionary["as:client_id"];//ConfigurationManager.AppSettings["as:AudienceId"];

      string symmetricKeyAsBase64 = data.Properties.Dictionary["as:client_secret"]; //ConfigurationManager.AppSettings["as:AudienceSecret"];

      var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

      var signingKey = new HmacSigningCredentials(keyByteArray);

      var issued = data.Properties.IssuedUtc;

      var expires = data.Properties.ExpiresUtc;

      var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

      var handler = new JwtSecurityTokenHandler();

      var jwt = handler.WriteToken(token);

      return jwt;
    }

    public AuthenticationTicket Unprotect(string protectedText)
    {

      throw new NotImplementedException();

    }
  }
}