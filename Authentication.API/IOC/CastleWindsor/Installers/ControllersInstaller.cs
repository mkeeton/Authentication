using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Authentication.Data.Interfaces;
using Authentication.Data.DbContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace Authentication.API.IOC.CastleWindsor.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Classes.FromThisAssembly()
          .BasedOn<ApiController>()
          .LifestyleTransient());
      container.Register(
          Component.For<IDbContext>()
              .UsingFactoryMethod(_ => new DbContextSql(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)).LifestylePerWebRequest()
      );

      //container.Register(
      //          Component.For<Authentication.ISSolutions.Domain.Models.LoginList>()
      //          .UsingFactoryMethod(_ => new Authentication.ISSolutions.Domain.Models.LoginList(360))
      //          .LifeStyle.Singleton
      //);

      container.Register(
                Component.For<IUserStore<Authentication.Domain.Models.User, System.Guid>>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.UserRepository>()
                .LifeStyle.PerWebRequest
      );

      //container.Register(
      //    Component.For<ILoginRepository>()
      //    .ImplementedBy<LoginRepository>()
      //    .LifeStyle.PerWebRequest
      //);

      //container.Register(
      //    Component.For<ISessionRepository>()
      //    .ImplementedBy<SessionRepository>()
      //    .LifeStyle.PerWebRequest
      //);

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
          Component.For<Microsoft.Owin.IOwinContext>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext()).LifestylePerWebRequest()
      );

      //container.Register(
      //    Component.For<ApplicationSignInManager>()
      //        .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationSignInManager>()).LifestylePerWebRequest()
      //);

    }

  }
}