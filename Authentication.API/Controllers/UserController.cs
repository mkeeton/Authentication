using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Authentication.Domain.Models;
using System.Threading.Tasks;

namespace Authentication.API.Controllers
{
    [Authorize(Roles="Admin,User Manager")]
    [RoutePrefix("api/Users")]
    public class UserController : ApiController
    {
      
    private Infrastructure.ExtendedUnitOfWork _unitOfWork;

    protected Infrastructure.ExtendedUnitOfWork UnitOfWork
    {
      get
      {
        return _unitOfWork;
      }
      set
      {
        _unitOfWork = value;
      }
    }

    public UserController(Infrastructure.ExtendedUnitOfWork unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }
      
      // GET api/values
      [EnableQuery()]
      public IQueryable<Models.UserListViewModel> Get()
      {
        List<Models.UserListViewModel> _users = new List<Models.UserListViewModel>();
        
        foreach(User _user in UnitOfWork.UserStore.ListAsync().Result)
        {
          _users.Add(new Models.UserListViewModel() { Id = _user.Id, Email = _user.Email, FirstName = _user.FirstName, LastName = _user.LastName });
        }
        return _users.AsQueryable();
      }

      // GET api/values/5
      public Models.UserDetailsViewModel Get(string id)
      {
        User _user = UnitOfWork.UserStore.FindByIdAsync(new Guid(id)).Result;
        List<Models.UserRoleViewModel> _assignedRoles = new List<Models.UserRoleViewModel>();
        List<Models.UserRoleViewModel> _availableRoles = new List<Models.UserRoleViewModel>();
        foreach (Role _role in UnitOfWork.UserStore.AssignedRolesForUserAsync(_user).Result)
        {
          _assignedRoles.Add(new Models.UserRoleViewModel() { RoleId = _role.Id, RoleName = _role.Name, RoleDescription = _role.Description });
        }
        foreach (Role _role in UnitOfWork.UserStore.AvailableRolesForUserAsync(_user).Result)
        {
          _availableRoles.Add(new Models.UserRoleViewModel(){RoleId=_role.Id, RoleName=_role.Name, RoleDescription=_role.Description});
        }
        return new Models.UserDetailsViewModel() { Id = _user.Id, Email = _user.Email, FirstName = _user.FirstName, LastName = _user.LastName, AssignedRoles = _assignedRoles, AvailableRoles = _availableRoles };
      }

      // POST api/values
      public async Task<IHttpActionResult> Post([FromBody]Models.UserDetailsViewModel value)
      {
        UnitOfWork.BeginWork();
        try
        {
          User _user = UnitOfWork.UserStore.FindByIdAsync(value.Id).Result;
          if(_user!=null)
          {
            _user.Email = value.Email;
            _user.UserName = value.Email;
            _user.FirstName = value.FirstName;
            _user.LastName = value.LastName;
            await UnitOfWork.UserStore.UpdateAsync(_user);
            foreach(Models.UserRoleViewModel _role in value.AssignedRoles)
            {
              await UnitOfWork.UserStore.AddToRoleAsync(_user,_role.RoleName);
            }
            foreach (Models.UserRoleViewModel _role in value.AvailableRoles)
            {
              await UnitOfWork.UserStore.RemoveFromRoleAsync(_user, _role.RoleName);
            }
            UnitOfWork.CommitWork();
            return Ok();
          }
          else
          {
            return BadRequest("The user was not found.");
          }
        }
        catch(Exception ex)
        {
          UnitOfWork.RollbackWork();
          return BadRequest("An internal server error occured.");
        }

      }

      // PUT api/values/5
      public void Put(int id, [FromBody]string value)
      {
      }

      // DELETE api/values/5
      public void Delete(int id)
      {
      }
    }
}
