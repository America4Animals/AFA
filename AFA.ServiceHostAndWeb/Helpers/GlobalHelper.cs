using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceInterface;
using ServiceStack;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;

namespace AFA.ServiceHostAndWeb.Helpers
{
    public static class GlobalHelper
    {
        // Using DI to do this
        //public static T GetService<T>() where T : ServiceStack.ServiceInterface.Service
        //{
        //    var service = AppHostBase.Resolve<T>();
        //    service.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
        //    return service;
        //}
    }
}