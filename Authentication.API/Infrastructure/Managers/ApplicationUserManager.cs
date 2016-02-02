using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Authentication.API.Models;
using System.Security.Claims;
using Authentication.Domain.Models;
using Authentication.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Authentication.API.Infrastructure.Managers
{
  public class ApplicationUserManager : UserManager<User, Guid>
  {
    public ApplicationUserManager(IUserStore<User, Guid> store)
      : base(store)
    {
      // Configure validation logic for usernames
      this.UserValidator = new UserValidator<User, Guid>(this)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };
      // Configure validation logic for passwords
      this.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = false,
        RequireDigit = true,
        RequireLowercase = true,
        RequireUppercase = true,
      };
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
      var repository = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<IUserRepository>();
      var manager = new ApplicationUserManager(repository);

      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user, string authenticationType)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await this.CreateIdentityAsync(user, authenticationType);
      // Add custom user claims here
      return userIdentity;
    }

    //public async Task<IEnumerable<Claim>> GetClaims(User user)
    //{
    // // return await this.Store.
    //}
  }
}