using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

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

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Claim> Claims
    {
      get
      {
        return new List<Claim>();
      }
    }

    public ICollection<Login> Logins
    {
      get
      {
        return new List<Login>();
      }
    }

    public class SearchParameters
    {
      public string UserName { get; set; }
      public string Password { get; set; }
    }
  }
}
