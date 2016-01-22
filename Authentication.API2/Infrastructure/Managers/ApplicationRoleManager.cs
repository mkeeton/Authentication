using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Authentication.Domain.Models;

namespace Authentication.API2.Infrastructure.Managers
{
  public class ApplicationRoleManager : RoleManager<Role,Guid>
  {
    public ApplicationRoleManager(IRoleStore<Role,Guid> roleStore)
      : base(roleStore)
    {
    }

  }
}