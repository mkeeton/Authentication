using System;
using System.Data;
using System.Data.SqlClient;

namespace Authentication.Data.DbContext
{
  public class DbContextSql : Interfaces.IDbContext
  {

    string _connectionString;
    IDbTransaction _transaction = null;

    public DbContextSql()
    {
    }

    public DbContextSql(string connectionString)
    {
      _connectionString = connectionString;
    }

    public string ConnectionString
    {
      get
      {
        return _connectionString;
      }
      set
      {
        _connectionString = value;
      }
    }

    public IDbConnection OpenConnection()
    {
      IDbConnection connection = new SqlConnection(_connectionString);
      connection.Open();
      return connection;
    }

    public IDbConnection OpenConnection(IDbTransaction transaction)
    {
      if (transaction == null || transaction.Connection == null)
      {
        return OpenConnection();
      }
      else
      {
        return transaction.Connection;
      }
    }

    public IDbTransaction CurrentTransaction
    {
      get
      {
        return _transaction;
      }
    }

    public void BeginTransaction()
    {
      _transaction = OpenConnection().BeginTransaction();
    }

    public void CommitTransaction()
    {
      _transaction.Commit();
      _transaction = null;
    }

    public void RollbackTransaction()
    {
      _transaction.Rollback();
      _transaction = null;
    }

    public void Dispose()
    {

    }
  }
}
