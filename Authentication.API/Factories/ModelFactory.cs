using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;
using System.Web.Http.Routing;

using Authentication.Domain.Models;

namespace Authentication.API.Factories
{
  public class ModelFactory
  {

    private UrlHelper _UrlHelper;
    private Infrastructure.Managers.ApplicationUserManager _AppUserManager;

    public ModelFactory(HttpRequestMessage request, Infrastructure.Managers.ApplicationUserManager appUserManager)
    {
      _UrlHelper = new UrlHelper(request);
      _AppUserManager = appUserManager;
    }

    public ViewModels.UserBindingModel Create(User appUser)
    {
      return new ViewModels.UserBindingModel
      {
        //Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
        //Id = appUser.Id,
        //UserName = appUser.UserName,
        //FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
        //Email = appUser.Email,
        //EmailConfirmed = appUser.EmailConfirmed,
        //Level = appUser.Level,
        //JoinDate = appUser.JoinDate,
        //Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
        //Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
      };

    }

    //public ViewModels.RoleBindingModel Create(IdentityRole appRole)
    //{

    //  return new ViewModels.RoleBindingModel
    //  {
    //    Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
    //    Id = appRole.Id,
    //    Name = appRole.Name
    //  };

    //}
  }
}