using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IUserSessionRepository
  {
    Task CreateAsync(UserSession userSession);

    Task DeleteAsync(UserSession userSession);
  }
}
