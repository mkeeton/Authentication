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

    Task<List<Role>> ListAsync();

    Task<List<Role>> GetRolesForActionAndMethod(string currentAction, string currentMethod);

    Task<List<RoleApiPath>> ListRoleApiPathsAsync(Guid roleId);

    Task CreateRoleApiPathAsync(RoleApiPath rolePath);

    Task<RoleApiPath> FindRoleApiPathAsync(Guid roleId, string actionPath, string actionMethod);

    Task DeleteRoleApiPathAsync(Guid ApiPathId);
  }
}
