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
using System.Web.Mvc;
using Authentication.Data.Interfaces;
using Authentication.Data.DbContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Authentication.Infrastructure.Interfaces;
using Authentication.Infrastructure.Repositories.Sql;

namespace Authentication.API.IOC.CastleWindsor.Installers
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
                .ImplementedBy<UserRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<IRoleRepository>()
          .ImplementedBy<RoleRepository>()
          .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IUserSessionRepository>()
                .ImplementedBy<UserSessionRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IClaimRepository>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.Sql.ClaimRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IClientRepository>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.Sql.ClientRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IRefreshTokenRepository>()
                .ImplementedBy<Authentication.Infrastructure.Repositories.Sql.RefreshTokenRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<Infrastructure.ExtendedUnitOfWork>()
                .ImplementedBy<Infrastructure.ExtendedUnitOfWork>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<Infrastructure.Managers.ApplicationUserManager>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext().GetUserManager<Infrastructure.Managers.ApplicationUserManager>()).LifestylePerWebRequest()
      );

      container.Register(
          Component.For<ISecureDataFormat<AuthenticationTicket>>()
              .UsingFactoryMethod(_ => new Providers.CustomJwtFormat("")).LifestylePerWebRequest()
      );

      //container.Register(
      //    Component.For<ISecureDataFormat<AuthenticationTicket>>()
      //    .ImplementedBy<Providers.CustomJwtFormat>()
      //    .LifeStyle.PerWebRequest
      //);


      container.Register(
                Component.For<IDataSerializer<AuthenticationTicket>>()
                .ImplementedBy<TicketSerializer>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
          Component.For<IDataProtector>()
              .UsingFactoryMethod(_ => new DpapiDataProtectionProvider().Create("ASP.NET Identity")).LifestylePerWebRequest()
      );

      //container.Register(
      //          Component.For<Infrastructure.Managers.ApplicationRoleManager>()
      //          .ImplementedBy<Infrastructure.Managers.ApplicationRoleManager>()
      //          .LifeStyle.PerWebRequest
      //);

      container.Register(
          Component.For<Microsoft.Owin.IOwinContext>()
              .UsingFactoryMethod(_ => HttpContext.Current.GetOwinContext()).LifestylePerWebRequest()
      );

    }

  }
}