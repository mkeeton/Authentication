using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Models;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IClientRepository
  {

    Task<List<Client>> ListAsync();
    Task<Client> FindById(Guid Id);
    Task<Client> FindByClientAppId(string clientId);
    Task<Client> FindByURL(string uRL);

    Task<List<ClientAllowedOrigin>> ListAllowedOrigins(Client client);
    Task<List<ClientAllowedOrigin>> ListAllowedOrigins(Guid clientId);
  }
}
