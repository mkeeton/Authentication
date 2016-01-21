using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Authentication.Infrastructure.Interfaces;
using System.Web.Mvc;
using Authentication.Data.Interfaces;
using Authentication.Data.DbContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Authentication.API2.IOC.CastleWindsor.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {

      container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));

      container.Register(Classes.FromThisAssembly()
          .BasedOn<Controller>()
          .LifestyleTransient()
      );

      container.Register(Classes.FromThisAssembly()
          .BasedOn<ApiController>()
          .LifestyleTransient()
      );

      container.Register(
          Component.For<IDbContext>()
              .UsingFactoryMethod(_ => new DbContextSql(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<IUserRepository>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.UserRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IClaimRepository>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.ClaimRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<Infrastructure.UnitOfWork>()
                .ImplementedBy<Infrastructure.UnitOfWork>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<Infrastructure.Managers.ApplicationUserManager>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<Infrastructure.Managers.ApplicationUserManager>()).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<Infrastructure.Managers.ApplicationRoleManager>()
                .ImplementedBy<Infrastructure.Managers.ApplicationRoleManager>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<Microsoft.Owin.IOwinContext>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext()).LifestylePerWebRequest()
      );

    }

  }
}