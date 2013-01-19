using AFA.Web.Helpers;
using ServiceStack.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.ServiceClient.Web;

namespace AFA.Web.Controllers
{
    public class RestController : Controller
    {
        protected IRestClient _client;

        public RestController(IRestClient client)
        {
            _client = new JsonServiceClient(GlobalHelper.GetServiceUrl());
        }

    }
}
