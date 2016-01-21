using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;
using System.Security.Claims;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IClaimRepository : IRepository
  {
    IEnumerable<Claim> GetClaims(User user);

    Claim CreateClaim(string type, string value);
  }
}
