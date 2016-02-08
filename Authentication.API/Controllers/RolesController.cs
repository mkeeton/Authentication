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
    public Models.RoleViewModel Get(string id)
    {
      Role _role = UnitOfWork.RoleStore.FindByIdAsync(new Guid(id)).Result;
      List<Models.ApiViewModel> _assignedApis = new List<Models.ApiViewModel>();
      List<Models.ApiViewModel> _availableApis = new List<Models.ApiViewModel>();
      System.Collections.ObjectModel.Collection<System.Web.Http.Description.ApiDescription> _apis = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
      foreach (System.Web.Http.Description.ApiDescription _api in _apis)
      {
        if (_api.ActionDescriptor.GetFilterPipeline().Where(f => f.Instance is System.Web.Http.Filters.IAuthorizationFilter).Any())
        { 
          _availableApis.Add(new Models.ApiViewModel(){Path=_api.RelativePath, HttpMethod=_api.HttpMethod.ToString()});
        }
      }
      return new Models.RoleViewModel(){ Id=_role.Id, RoleName=_role.Name, RoleDescription=_role.Description, AssignedApis=_assignedApis, AvailableApis=_availableApis};
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
