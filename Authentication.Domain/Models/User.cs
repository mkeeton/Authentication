using System;
using Microsoft.AspNet.Identity;

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

    public class SearchParameters
    {
      public string UserName { get; set; }
      public string Password { get; set; }
    }
  }
}
