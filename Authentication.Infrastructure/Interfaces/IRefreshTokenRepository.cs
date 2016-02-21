using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IRefreshTokenRepository
  {

    Task<List<RefreshToken>> ListAsync();
    Task<RefreshToken> FindById(Guid tokenId);
    Task<RefreshToken> FindByTokenId(string tokenId);
    Task<RefreshToken> FindByTokenUser(Guid userId);
  }
}
