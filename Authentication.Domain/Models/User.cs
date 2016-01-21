using System;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Collections.Generic;

namespace Authentication.Domain.Models
{
  public class User : IUser<Guid>
  {
    public Guid Id { get; set; }

    public string UserName { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string SecurityStamp { get; set; }

    public string Email { get; set; }
    
    public bool EmailConfirmed { get; set; }

    public ICollection<Claim> Claims
    {
      get
      {
        return new List<Claim>();
      }
    }

    public class SearchParameters
    {
      public string UserName { get; set; }
      public string Password { get; set; }
    }

  }
}
