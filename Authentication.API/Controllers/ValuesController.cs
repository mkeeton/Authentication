﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;

namespace Authentication.API.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [EnableQuery()]
        public IQueryable<string> Get()
        {
            return (new string[] { "value1", "value2", "Screw You", "It Works!!" }).AsQueryable();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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
