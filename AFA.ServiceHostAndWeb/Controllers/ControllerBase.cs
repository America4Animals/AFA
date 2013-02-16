using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceModel;
using ServiceStack.CacheAccess;
using ServiceStack.Mvc;
using ServiceStack.Mvc.MiniProfiler;
using ServiceStack.WebHost.Endpoints;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class ControllerBase : ServiceStackController<CustomUserSession> {}
}
