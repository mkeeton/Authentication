using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Authentication.Infrastructure.Interfaces;
using Authentication.Domain.Models;

namespace Authentication.API.Attributes
{
  public class AuthorizeUserAttribute : AuthorizeAttribute
  {

    //// Custom property
    //public string AccessLevel { get; set; }

    //protected override bool IsAuthorized(HttpActionContext httpContext)
    //{
    //  var isAuthorized = base.IsAuthorized(httpContext);
    //  if (!isAuthorized)
    //  {
    //    return false;
    //  }


    //  httpContext
    //  //string privilegeLevels = string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB

    //  //if (privilegeLevels.Contains(this.AccessLevel))
    //  //{
    //    return true;
    //  //}
    //  //else
    //  //{
    //  //  return false;
    //  //}
    //}

    public override void OnAuthorization(HttpActionContext actionContext)
    {
      string _currentAction = actionContext.Request.RequestUri.AbsolutePath;
      string _currentMethod = actionContext.Request.Method.ToString();
      var repository = Authentication.API.WebApiApplication.GetContainer().Kernel.Resolve<IRoleRepository>();
      List<Role> _roles = repository.GetRolesForActionAndMethod(_currentAction, _currentMethod).Result;
      foreach (Role _role in _roles)
      {
        if (base.Roles.Trim() != "")
        {
          base.Roles += ",";
        }
        base.Roles += _role.Name;
      }
    }
  }
}