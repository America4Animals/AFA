using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFA.ServiceHostAndWeb.App_Start;

namespace AFA.ServiceHostAndWeb.Helpers
{
    public static class AppExtensions
    {
        public static bool In(this Env env, params Env[] inAnyEnvs)
        {
            return inAnyEnvs.Any(x => x == env);
        }
    }
}