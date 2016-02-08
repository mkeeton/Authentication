using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Authentication.API.Models
{
  public class UserListViewModel
  {

    public Guid Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
  }

  public class UserDetailsViewModel
  {
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<UserRoleViewModel> AssignedRoles { get; set;}

    public List<UserRoleViewModel> AvailableRoles { get; set;}
  }

  public class UserRoleViewModel
  {
    public Guid RoleId { get; set;}

    public string RoleName { get; set;}

    public string RoleDescription { get; set;}
  }

  public class RoleViewModel
  {
    public Guid Id { get; set; }

    public string RoleName { get; set; }

    public string RoleDescription { get; set; }

    public List<Models.ApiViewModel> AssignedApis { get; set; }

    public List<Models.ApiViewModel> AvailableApis { get; set; }
  }

  public class ApiViewModel
  {
    public string Path { get; set;}

    public string HttpMethod { get; set;}
  }
}