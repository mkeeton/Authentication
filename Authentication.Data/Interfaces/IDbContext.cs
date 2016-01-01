using System;
using System.Data;

namespace Authentication.Data.Interfaces
{
  public interface IDbContext : IDisposable
  {

    IDbConnection OpenConnection();
    IDbConnection OpenConnection(IDbTransaction transaction);
    IDbTransaction CurrentTransaction { get; }
    string ConnectionString { get; set; }
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();

  }
}
