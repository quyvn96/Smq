﻿using Smq.Service;
using Smq.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Smq.Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
        public HomeController(IErrorService errorService)
            : base(errorService)
        {
            this._errorService = errorService;
        }
        [Route("TestMethod")]
        [HttpGet]
        public string TestMethod()
        {
            return "Hello, SMQ Member. ";
        }
    }
}
