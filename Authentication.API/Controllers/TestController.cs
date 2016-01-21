using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Authentication.Data.Interfaces;
using System.Web;

namespace Authentication.API.Controllers
{
    //[RoutePrefix("api/balls")]
    //public class TestController : BaseApiController
    //{
    //  private Infrastructure.UnitOfWork _context;

    //  public TestController(Infrastructure.UnitOfWork context) : base(context)
    //  {
    //    _context = context;
    //  }

    //  [System.Web.Http.AcceptVerbs("GET", "POST")]
    //  [System.Web.Http.HttpGet]
    //  public IEnumerable<string> testing()
    //  {
    //    return new string[] { "value1", "value2" };
    //  }

    //}
  [RoutePrefix("api/balls")]
  public class TestController : ApiController
  {
    private Infrastructure.UnitOfWork _context;

    public TestController(Infrastructure.UnitOfWork context)
    // : base(context)
    {
       _context = context;
    }

    [System.Web.Http.AcceptVerbs("GET", "POST")]
    [System.Web.Http.HttpGet]
    [Route("suckem")]
    public IEnumerable<string> testing()
    {
      return new string[] { "value1", "value2" };
    }

  }
}
