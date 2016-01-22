using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Authentication.Domain.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces;

namespace Authentication.API2.Infrastructure.Managers
{
  public class ApplicationUserManager : UserManager<User,Guid>
  {
    public ApplicationUserManager(IUserStore<User,Guid> store)
      : base(store)
    {
      // Configure validation logic for usernames
      this.UserValidator = new UserValidator<User,Guid>(this)
      {
        AllowOnlyAlphanumericUserNames = true,
        RequireUniqueEmail = true
      };

      // Configure validation logic for passwords
      this.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = true,
        RequireDigit = false,
        RequireLowercase = true,
        RequireUppercase = true,
      };

      this.EmailService = new Infrastructure.Services.EmailService();
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {

      var repository = Authentication.API2.WebApiApplication.GetContainer().Kernel.Resolve<IUserRepository>();
      var manager = new ApplicationUserManager(repository);

      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider = new DataProtectorTokenProvider<User,Guid>(dataProtectionProvider.Create("ASP.NET Identity"))
        {
          //Code for email confirmation and reset password life time
          TokenLifespan = TimeSpan.FromHours(6)
        };
      }

      return manager;
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
    {
      return await GenerateUserIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user, string authenticationType)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await CreateIdentityAsync(user, authenticationType);
      // Add custom user claims here
      return userIdentity;
    }
  }
}