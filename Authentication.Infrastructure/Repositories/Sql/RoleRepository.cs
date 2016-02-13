using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Authentication.Domain.Models;
using System.Data;
using Authentication.Data.Interfaces;
using Dapper;

namespace Authentication.Infrastructure.Repositories.Sql
{
  public class RoleRepository : Interfaces.IRoleRepository
  {

    private readonly IDbContext CurrentContext;

    public RoleRepository(IDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public void Dispose()
    {

    }

    public virtual Task<List<Role>> ListAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Role>("select * from auth_Roles", new { }).AsList();
      });
    }

    public virtual Task CreateAsync(Role role)
    {
      if (role == null)
        throw new ArgumentNullException("role");
      var owner = this.FindByNameAsync(role.Name);
      if ((owner == null) || (owner.Result == null))
      {
        return Task.Factory.StartNew(() =>
        {
          role.Id = Guid.NewGuid();
          using (IDbConnection connection = CurrentContext.OpenConnection())
            connection.Execute("insert into auth_Roles(Id, Name) values(@Id, @Name)", role);
        });
      }
      else
      {
        role.Id = owner.Result.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task DeleteAsync(Role role)
    {
      if (role == null)
        throw new ArgumentNullException("role");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_Roles where Id = @Id", new { role.Id });
      });
    }

    public virtual Task<Role> FindByIdAsync(Guid roleId)
    {
      if (roleId == Guid.Empty)
        throw new ArgumentNullException("roleId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Role>("select * from auth_Roles where Id = @Id", new { Id = roleId }).SingleOrDefault();
      });
    }

    public virtual Task<Role> FindByNameAsync(string roleName)
    {
      if (string.IsNullOrWhiteSpace(roleName))
        throw new ArgumentNullException("roleName");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Role>("select * from auth_Roles where lower(Name) = lower(@roleName)", new { roleName }).SingleOrDefault();
      });
    }

    public virtual Task UpdateAsync(Role role)
    {
      if (role == null)
        throw new ArgumentNullException("role");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update auth_Roles set Name = @Name where Id = @Id", role);
      });
    }

    public virtual Task<List<Role>> GetRolesForActionAndMethod(string currentAction, string currentMethod)
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Role>("select * from auth_Roles R INNER JOIN auth_RoleApiPaths RAP ON R.ID=RAP.RoleId WHERE RAP.ActionPath=@CurrentAction AND RAP.ActionMethod=@CurrentMethod", new { CurrentAction = currentAction, CurrentMethod = currentMethod }).AsList();
      });
    }

    public virtual Task<List<RoleApiPath>> ListRoleApiPathsAsync(Guid roleId)
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<RoleApiPath>("select * from auth_RoleApiPaths WHERE RoleId=@RoleId", new { roleId }).AsList();
      });
    }

    public virtual Task CreateRoleApiPathAsync(RoleApiPath rolePath)
    {
      if (rolePath == null)
        throw new ArgumentNullException("rolePath");
      var owner = this.FindRoleApiPathAsync(rolePath.RoleId, rolePath.ActionPath, rolePath.ActionMethod);
      if ((owner == null) || (owner.Result == null))
      {
        return Task.Factory.StartNew(() =>
        {
          rolePath.Id = Guid.NewGuid();
          using (IDbConnection connection = CurrentContext.OpenConnection())
            connection.Execute("insert into auth_RoleApiPaths(Id, RoleId, ActionPath, ActionMethod) values(@Id, @RoleId, @ActionPath, @ActionMethod)", rolePath);
        });
      }
      else
      {
        rolePath.Id = owner.Result.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task<RoleApiPath> FindRoleApiPathAsync(Guid roleId, string actionPath, string actionMethod)
    {
      //if (string.IsNullOrWhiteSpace(roleName))
      //  throw new ArgumentNullException("roleName");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<RoleApiPath>("select * from auth_RoleApiPaths where RoleId=@RoleId AND lower(ActionPath)=lower(@ActionPath) AND lower(ActionMethod)=lower(@ActionMethod)", new { RoleId = roleId, ActionPath = actionPath, ActionMethod = actionMethod }).SingleOrDefault();
      });
    }

    public virtual Task DeleteRoleApiPathAsync(Guid ApiPathId)
    {
      if (ApiPathId == Guid.Empty)
        throw new ArgumentNullException("ApiPathId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_RoleApiPaths where Id = @Id", new { Id = ApiPathId });
      });
    }
  }
}
