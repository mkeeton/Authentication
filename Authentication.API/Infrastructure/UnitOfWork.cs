﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Authentication.Data.Interfaces;
using Authentication.Infrastructure.Interfaces;

namespace Authentication.API.Infrastructure
{
  public class UnitOfWork : IUnitOfWork, IDisposable
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
      DbContext.BeginTransaction();
    }

    public void CommitWork()
    {
      DbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
      DbContext.RollbackTransaction();
    }
    //public ISessionRepository SessionManager
    //{
    //  get;
    //  set;
    //}

    //public ILoginRepository LoginManager
    //{
    //  get;
    //  set;
    //}

    public Infrastructure.Managers.ApplicationUserManager UserManager
    {
      get;
      set;
    }

  }
}