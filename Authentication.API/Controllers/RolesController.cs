using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Authentication.Domain.Models;

namespace Authentication.API.Controllers
{
  [Authorize]
  [RoutePrefix("api/Roles")]
  public class RolesController : ApiController
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

    public RolesController(Infrastructure.ExtendedUnitOfWork unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }

    // GET api/values
    [EnableQuery()]
    public IQueryable<Role> Get()
    {
      List<Role> _roles = UnitOfWork.RoleStore.ListAsync().Result;
      return _roles.AsQueryable();
    }

    // GET api/values/5
    public Role Get(string id)
    {
      Role _role = UnitOfWork.RoleStore.FindByIdAsync(new Guid(id)).Result;
      return _role;
    }

    // POST api/values
    public void Post([FromBody]string value)
    {
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
