using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Authentication.Infrastructure;
using Authentication.Data.Interfaces;

namespace Authentication.API.Infrastructure
{
  public class ExtendedUnitOfWork : UnitOfWork
  {

    public ExtendedUnitOfWork(IDbContext context)
      : base(context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.DbContext = context;
    }

    public Infrastructure.Managers.ApplicationUserManager UserManager
    {
      get;
      set;
    }
  }
}