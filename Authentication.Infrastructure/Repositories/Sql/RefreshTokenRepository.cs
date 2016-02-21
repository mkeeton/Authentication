using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Authentication.Data.Interfaces;
using Authentication.Domain.Models;
using Dapper;

namespace Authentication.Infrastructure.Repositories.Sql
{
  public class RefreshTokenRepository : Interfaces.IRefreshTokenRepository
  {
    private readonly IDbContext CurrentContext;

    public RefreshTokenRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public Task<List<RefreshToken>> ListAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          return connection.Query<RefreshToken>("select * from auth_RefreshTokens", new { }).AsList();
        }
      });
    }

    public Task<RefreshToken> FindById(Guid tokenId)
    {
      if (tokenId == Guid.Empty)
        throw new ArgumentNullException("tokenId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          return connection.Query<RefreshToken>("select * from auth_RefreshTokens where Id = @Id", new { Id = tokenId }).SingleOrDefault();
        }
      });
    }

    public Task<RefreshToken> FindByTokenId(string tokenId)
    {
      if (tokenId == "")
        throw new ArgumentNullException("tokenId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          return connection.Query<RefreshToken>("select * from auth_RefreshTokens where tokenId = @TokenId", new { TokenId = tokenId }).SingleOrDefault();
        }
      });
    }

    public Task<RefreshToken> FindByTokenUser(Guid userId)
    {
      if (userId == Guid.Empty)
        throw new ArgumentNullException("userId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          return connection.Query<RefreshToken>("select * from auth_RefreshTokens where UserId = @UserId", new { UserId = userId }).SingleOrDefault();
        }
      });
    }
  }
}
