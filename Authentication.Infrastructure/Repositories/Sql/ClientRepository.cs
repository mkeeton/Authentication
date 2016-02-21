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
  public class ClientRepository : Interfaces.IClientRepository
  {

    private readonly IDbContext CurrentContext;

    public ClientRepository(IDbContext context)
    {
      if (context==null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public Task<List<Client>> ListAsync()
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          List<Client> _clientList = (connection.Query<Client>("select * from auth_Clients", new { }).AsList());
          foreach(Client _client in _clientList)
          {
            _client.AllowedOrigins = ListAllowedOrigins(_client).Result;
          }
          return _clientList;
        }
      });
    }

    public Task<Client> FindById(Guid clientId)
    {
      if (clientId == Guid.Empty)
        throw new ArgumentNullException("clientId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        { 
          Client _client = connection.Query<Client>("select * from auth_Clients where Id = @Id", new { Id = clientId }).SingleOrDefault();
          _client.AllowedOrigins = ListAllowedOrigins(_client).Result;
          return _client;
        }
      });
    }

    public Task<Client> FindByClientAppId(string clientAppId)
    {
      if (clientAppId == "")
        throw new ArgumentNullException("id");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          Client _client = connection.Query<Client>("select * from auth_Clients where ClientId = @ClientId", new { ClientId = clientAppId }).SingleOrDefault();
          _client.AllowedOrigins = ListAllowedOrigins(_client).Result;
          return _client;
        }
      });
    }

    public Task<Client> FindByURL(string clientURL)
    {
      if (clientURL == "")
        throw new ArgumentNullException("id");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
        {
          Client _client = connection.Query<Client>("select * from auth_Clients where URL = @ClientURL", new { ClientURL = clientURL }).SingleOrDefault();
          _client.AllowedOrigins = ListAllowedOrigins(_client).Result;
          return _client;
        }
      });
    }

    public Task<List<ClientAllowedOrigin>> ListAllowedOrigins(Client client)
    {
      return ListAllowedOrigins(client.Id);
    }
    public Task<List<ClientAllowedOrigin>> ListAllowedOrigins(Guid clientId)
    {
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<ClientAllowedOrigin>("select * from auth_ClientAllowedOrigins WHERE ClientId=@ClientId", new { ClientId=clientId }).AsList();
      });
    }
  }
}
