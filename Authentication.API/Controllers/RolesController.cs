using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Authentication.Domain.Models;
using System.Threading.Tasks;
using Authentication.Framework;

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
    public async Task<IHttpActionResult> Get(string id)
    {
      Guid _roleId = NullHandlers.NGUID(id);
      Role _role = null;
      if (_roleId != Guid.Empty)
      {
        _role = await UnitOfWork.RoleStore.FindByIdAsync(_roleId);
      }
      if (_role == null)
      {
        _role = new Role();
      }
      List<Models.ApiViewModel> _assignedApis = new List<Models.ApiViewModel>();
      List<Models.ApiViewModel> _availableApis = new List<Models.ApiViewModel>();
      List<Models.ClientPathViewModel> _availableClientPaths = new List<Models.ClientPathViewModel>();
      List<Models.ClientPathViewModel> _assignedClientPaths = new List<Models.ClientPathViewModel>();
      foreach (RoleApiPath _apiPath in await UnitOfWork.RoleStore.ListRoleApiPathsAsync(_role.Id))
      {
        _assignedApis.Add(new Models.ApiViewModel() { Id = _apiPath.Id, Path = _apiPath.ActionPath, HttpMethod = _apiPath.ActionMethod });
      }
      System.Collections.ObjectModel.Collection<System.Web.Http.Description.ApiDescription> _apis = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;
      foreach (System.Web.Http.Description.ApiDescription _api in _apis)
      {
        if (_api.ActionDescriptor.GetFilterPipeline().Where(f => f.Instance is Attributes.AuthorizeUserAttribute).Any())
        {
          if (!(_assignedApis.Where(f => f.Path == _api.RelativePath && f.HttpMethod == _api.HttpMethod.ToString()).Any()))
          {
            _availableApis.Add(new Models.ApiViewModel() { Path = _api.RelativePath, HttpMethod = _api.HttpMethod.ToString() });
          }
        }
      }
      return Ok(new Models.RoleViewModel() { Id = _role.Id, RoleName = _role.Name, RoleDescription = _role.Description, AssignedApis = _assignedApis, AvailableApis = _availableApis, AvailableClientPaths = _availableClientPaths, AssignedClientPaths = _assignedClientPaths });
    }

    // POST api/values
    public async Task<IHttpActionResult> Post([FromBody]Models.RoleViewModel value)
    {
      Role _role = null;
      if (NullHandlers.NGUID(value.Id) != Guid.Empty)
      {
        _role = await UnitOfWork.RoleStore.FindByIdAsync(value.Id);
      }
      if (_role == null)
      {
        _role = new Role();
      }
      _role.Name = value.RoleName;
      _role.Description = value.RoleDescription;
      UnitOfWork.BeginWork();
      if (_role.Id == Guid.Empty)
      {
        await UnitOfWork.RoleStore.CreateAsync(_role);
      }
      else
      {
        await UnitOfWork.RoleStore.UpdateAsync(_role);
      }
      foreach (Models.ApiViewModel _assignedApi in value.AssignedApis)
      {
        if (_assignedApi.Id == Guid.Empty)
        {
          RoleApiPath _newPath = new RoleApiPath() { RoleId = _role.Id, ActionPath = _assignedApi.Path, ActionMethod = _assignedApi.HttpMethod };
          await UnitOfWork.RoleStore.CreateRoleApiPathAsync(_newPath);
        }
      }
      foreach (Models.ApiViewModel _availableApi in value.AvailableApis)
      {
        if (_availableApi.Id != Guid.Empty)
        {
          await UnitOfWork.RoleStore.DeleteRoleApiPathAsync(_availableApi.Id); ;
        }
      }
      foreach (Models.ClientPathViewModel _assignedClientPath in value.AssignedClientPaths)
      {
        if (_assignedClientPath.Id == Guid.Empty)
        {

        }
      }
      foreach (Models.ClientPathViewModel _availableClientPath in value.AvailableClientPaths)
      {
        if (_availableClientPath.Id != Guid.Empty)
        {

        }
      }
      UnitOfWork.CommitWork();
      return Ok();
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
