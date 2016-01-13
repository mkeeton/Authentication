using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Authentication.API.Controllers
{
    public class BaseApiController : ApiController
    {

      private Infrastructure.UnitOfWork _unitOfWork;
      private Factories.ModelFactory _modelFactory;

      protected Infrastructure.UnitOfWork UnitOfWork
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

      protected Factories.ModelFactory TheModelFactory
      {
        get
        {
          if (_modelFactory == null)
          {
            _modelFactory = new Factories.ModelFactory(this.Request, this.UnitOfWork.UserManager);
          }
          return _modelFactory;
        }
      }

      public BaseApiController(Infrastructure.UnitOfWork unitOfWork)
      {
        _unitOfWork = unitOfWork;
      }

      protected IHttpActionResult GetErrorResult(IdentityResult result)
      {
        if (result == null)
        {
          return InternalServerError();
        }

        if (!result.Succeeded)
        {
          if (result.Errors != null)
          {
            foreach (string error in result.Errors)
            {
              ModelState.AddModelError("", error);
            }
          }

          if (ModelState.IsValid)
          {
            // No ModelState errors are available to send, so just return an empty BadRequest.
            return BadRequest();
          }

          return BadRequest(ModelState);
        }

        return null;
      }
    }
}
