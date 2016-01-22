using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;
using Microsoft.AspNet.Identity;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IRoleRepository : IRoleStore<Role,Guid>
  {

  }
}
