using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Service;

namespace AFA.Web.Controllers
{
    public class UsersController : RestController
    {
        public UsersController(IRestClient client) : base(client) { }

        public ActionResult Index()
        {
            return View();
        }

    }
}
