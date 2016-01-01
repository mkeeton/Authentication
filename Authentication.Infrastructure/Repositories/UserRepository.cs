using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Authentication.Domain.Models;
using Authentication.Data.Interfaces;
using System.Data;
using Dapper;

namespace Authentication.Infrastructure.Repositories
{
  public class UserRepository : IUserStore<User,Guid>
 {
    private readonly IDbContext CurrentContext;

    public UserRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public void Dispose()
    {

    }

    #region IUserStore
    public virtual Task CreateAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");
      var owner = this.FindByNameAsync(user.UserName);
      if((owner==null)||(owner.Result==null))
      {
        return Task.Factory.StartNew(() =>
        {
          user.Id = Guid.NewGuid();
          using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("insert into auth_Users(Id, UserName, PasswordHash, SecurityStamp, Email, EmailConfirmed) values(@Id, @userName, @passwordHash, @securityStamp, @email, @emailConfirmed)", user);
        });
      }
      else
      {
        user.Id = owner.Result.Id;
        return Task.FromResult(0);
      }
    }

    public virtual Task DeleteAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_Users where Id = @Id", new { user.Id });
      });
    }

    public virtual Task<User> FindByIdAsync(Guid userId)
    {
      if (userId==Guid.Empty)
        throw new ArgumentNullException("userId");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where Id = @Id", new { Id = userId }).SingleOrDefault();
      });
    }

    public virtual Task<User> FindByNameAsync(string userName)
    {
      if (string.IsNullOrWhiteSpace(userName))
        throw new ArgumentNullException("userName");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where lower(UserName) = lower(@userName)", new { userName }).SingleOrDefault();
      });
    }

    public virtual Task UpdateAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update auth_Users set UserName = @userName, PasswordHash = @passwordHash, SecurityStamp = @securityStamp where Id = @Id", user);
      });
    }
    #endregion

    #region IUserPasswordStore
    public virtual Task<string> GetPasswordHashAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.PasswordHash);
    }

    public virtual Task<bool> HasPasswordAsync(User user)
    {
      return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public virtual Task SetPasswordHashAsync(User user, string passwordHash)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.PasswordHash = passwordHash;

      return Task.FromResult(0);
    }

    #endregion

    #region IUserSecurityStampStore
    public virtual Task<string> GetSecurityStampAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.SecurityStamp);
    }

    public virtual Task SetSecurityStampAsync(User user, string stamp)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.SecurityStamp = stamp;

      return Task.FromResult(0);
    }

    #endregion

    public Task<User> FindByEmailAsync(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentNullException("email");

      return Task.Factory.StartNew(() =>
      {
        using(IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<User>("select * from auth_Users where lower(Email) = lower(@Email)", new { email }).SingleOrDefault();
      });
    }

    public Task<string> GetEmailAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailAsync(User user, string email)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.Email= email;

      return Task.FromResult(0);
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed)
    {
      if (user == null)
        throw new ArgumentNullException("user");

      user.EmailConfirmed = confirmed;
      return Task.FromResult(0);
    }
  }
}
