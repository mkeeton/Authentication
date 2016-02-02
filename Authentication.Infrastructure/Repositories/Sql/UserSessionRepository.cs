using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;
using System.Data;
using Authentication.Data.Interfaces;
using Dapper;

namespace Authentication.Infrastructure.Repositories.Sql
{
  public class UserSessionRepository : Interfaces.IUserSessionRepository
  {

    private readonly IDbContext CurrentContext;

    public UserSessionRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public void Dispose()
    {

    }

    public virtual Task CreateAsync(UserSession userSession)
    {
      if (userSession == null)
        throw new ArgumentNullException("role");

      return Task.Factory.StartNew(() =>
      {
        userSession.Id = Guid.NewGuid();
        userSession.StartDate = DateTime.Now;
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("insert into auth_CurrentSessions(Id, UserId, StartDate) values(@Id, @UserId, @StartDate)", userSession);
      });
    }

    public virtual Task DeleteAsync(UserSession userSession)
    {
      if (userSession == null)
        throw new ArgumentNullException("userSession");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("delete from auth_CurrentSessions where Id = @Id", new { userSession.Id });
      });
    }

    public virtual Task<UserSession> FindByIdAsync(Guid sessionId)
    {
      if (sessionId == Guid.Empty)
        throw new ArgumentNullException("sessionId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<UserSession>("select * from auth_CurrentSessions where Id = @Id", new { Id = sessionId }).SingleOrDefault();
      });
    }

    public virtual Task<UserSession> FindByUserIdAsync(Guid userId)
    {
      if (userId==Guid.Empty)
        throw new ArgumentNullException("userId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<UserSession>("select * from auth_CurrentSessions where UserId=@UserId", new { userId }).FirstOrDefault();
      });
    }

    public virtual Task UpdateAsync(UserSession userSession)
    {
      if (userSession == null)
        throw new ArgumentNullException("userSession");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update auth_CurrentSessions set UserId = @UserId where Id = @Id", userSession);
      });
    }

  }
}
