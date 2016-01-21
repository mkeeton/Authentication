using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Authentication.Domain.Models;

namespace Authentication.API2.Controllers
{
  [RoutePrefix("api/accounts")]
  public class AccountsController : BaseApiController
  {

    public AccountsController(Infrastructure.UnitOfWork unitOfWork)
      : base(unitOfWork)
    {
      this.UnitOfWork = unitOfWork;
    }

    [Authorize(Roles = "Admin")]
    [Route("users")]
    public IHttpActionResult GetUsers()
    {
      //Only SuperAdmin or Admin can delete users (Later when implement roles)
      var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

      return Ok(this.UnitOfWork.UserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{id:guid}", Name = "GetUserById")]
    public async Task<IHttpActionResult> GetUser(string Id)
    {
      //Only SuperAdmin or Admin can delete users (Later when implement roles)
      var user = await this.UnitOfWork.UserStore.FindByIdAsync(new Guid(Id));

      if (user != null)
      {
        return Ok(this.TheModelFactory.Create(user));
      }

      return NotFound();

    }

    [Authorize(Roles = "Admin")]
    [Route("user/{username}")]
    public async Task<IHttpActionResult> GetUserByName(string username)
    {
      //Only SuperAdmin or Admin can delete users (Later when implement roles)
      var user = await this.UnitOfWork.UserManager.FindByNameAsync(username);

      if (user != null)
      {
        return Ok(this.TheModelFactory.Create(user));
      }

      return NotFound();

    }

    [AllowAnonymous]
    [Route("create")]
    public async Task<IHttpActionResult> CreateUser(ViewModels.UserCreateBindingModel createUserModel)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = new User()
      {
        UserName = createUserModel.Username,
        Email = createUserModel.Email,
        //FirstName = createUserModel.FirstName,
        //LastName = createUserModel.LastName,
        //Level = 3,
        //JoinDate = DateTime.Now.Date,
      };


      IdentityResult addUserResult = await this.UnitOfWork.UserManager.CreateAsync(user, createUserModel.Password);

      if (!addUserResult.Succeeded)
      {
        return GetErrorResult(addUserResult);
      }

      string code = await this.UnitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

      var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

      await this.UnitOfWork.UserManager.SendEmailAsync(user.Id,
                                              "Confirm your account",
                                              "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

      Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

      return Created(locationHeader, TheModelFactory.Create(user));

    }

    [AllowAnonymous]
    [HttpGet]
    [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
    public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
    {
      if (userId == null || string.IsNullOrWhiteSpace(code))
      {
        ModelState.AddModelError("", "User Id and Code are required");
        return BadRequest(ModelState);
      }

      IdentityResult result = await this.UnitOfWork.UserManager.ConfirmEmailAsync(new Guid(userId), code);

      if (result.Succeeded)
      {
        return Ok();
      }
      else
      {
        return GetErrorResult(result);
      }
    }

    [Authorize]
    [Route("ChangePassword")]
    public async Task<IHttpActionResult> ChangePassword(ViewModels.ChangePasswordBindingModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      IdentityResult result = await this.UnitOfWork.UserManager.ChangePasswordAsync(new Guid(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);

      if (!result.Succeeded)
      {
        return GetErrorResult(result);
      }

      return Ok();
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{id:guid}")]
    public async Task<IHttpActionResult> DeleteUser(string id)
    {

      //Only SuperAdmin or Admin can delete users (Later when implement roles)

      var appUser = await this.UnitOfWork.UserStore.FindByIdAsync(new Guid(id));

      if (appUser != null)
      {
        IdentityResult result = await this.UnitOfWork.UserManager.DeleteAsync(appUser);

        if (!result.Succeeded)
        {
          return GetErrorResult(result);
        }

        return Ok();

      }

      return NotFound();

    }

    [Authorize(Roles = "Admin")]
    [Route("user/{id:guid}/roles")]
    [HttpPut]
    public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
    {

      var appUser = await this.UnitOfWork.UserManager.FindByIdAsync(new Guid(id));

      if (appUser == null)
      {
        return NotFound();
      }

      var currentRoles = await this.UnitOfWork.UserManager.GetRolesAsync(appUser.Id);

      var rolesNotExists = rolesToAssign.Except(this.UnitOfWork.RoleManager.Roles.Select(x => x.Name)).ToArray();

      if (rolesNotExists.Count() > 0)
      {

        ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
        return BadRequest(ModelState);
      }

      IdentityResult removeResult = await this.UnitOfWork.UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

      if (!removeResult.Succeeded)
      {
        ModelState.AddModelError("", "Failed to remove user roles");
        return BadRequest(ModelState);
      }

      IdentityResult addResult = await this.UnitOfWork.UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

      if (!addResult.Succeeded)
      {
        ModelState.AddModelError("", "Failed to add user roles");
        return BadRequest(ModelState);
      }

      return Ok();

    }

    [Authorize(Roles = "Admin")]
    [Route("user/{id:guid}/assignclaims")]
    [HttpPut]
    public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ViewModels.ClaimBindingModel> claimsToAssign)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = await this.UnitOfWork.UserManager.FindByIdAsync(new Guid(id));

      if (appUser == null)
      {
        return NotFound();
      }

      foreach (ViewModels.ClaimBindingModel claimModel in claimsToAssign)
      {
        if (appUser.Claims.Any(c => c.Type == claimModel.Type))
        {

          await this.UnitOfWork.UserManager.RemoveClaimAsync(new Guid(id), this.UnitOfWork.ClaimStore.CreateClaim(claimModel.Type, claimModel.Value));
        }

        await this.UnitOfWork.UserManager.AddClaimAsync(new Guid(id), this.UnitOfWork.ClaimStore.CreateClaim(claimModel.Type, claimModel.Value));
      }

      return Ok();
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{id:guid}/removeclaims")]
    [HttpPut]
    public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ViewModels.ClaimBindingModel> claimsToRemove)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var appUser = await this.UnitOfWork.UserManager.FindByIdAsync(new Guid(id));

      if (appUser == null)
      {
        return NotFound();
      }

      foreach (ViewModels.ClaimBindingModel claimModel in claimsToRemove)
      {
        if (appUser.Claims.Any(c => c.Type == claimModel.Type))
        {
          await this.UnitOfWork.UserManager.RemoveClaimAsync(new Guid(id), this.UnitOfWork.ClaimStore.CreateClaim(claimModel.Type, claimModel.Value));
        }
      }

      return Ok();
    }

  }
}
