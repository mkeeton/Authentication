using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.AspNet.Identity;
using Authentication.Data.Interfaces;
using Authentication.Domain.Models;

namespace Authentication.Infrastructure
{
  public class UnitOfWork : Interfaces.IUnitOfWork
  {
    public IDbContext DbContext { get; set; }

    //public LoginList CurrentLogins { get; set; }

    public UnitOfWork(IDbContext context)//, LoginList currentLogins)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.DbContext = context;
      //CurrentLogins = currentLogins;
    }

    public void Dispose()
    {

    }

    public void BeginWork()
    {
      //DbContext.BeginTransaction();
    }

    public void CommitWork()
    {
      //DbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
      //DbContext.RollbackTransaction();
    }

    public Interfaces.IUserRepository UserStore
    {
      get;
      set;
    }

    public Interfaces.IRoleRepository RoleStore
    {
      get;
      set;
    }

    public Interfaces.IClaimRepository ClaimStore
    {
      get;
      set;
    }

    public Interfaces.IUserSessionRepository UserSessionStore
    {
      get;
      set;
    }
  }
}
