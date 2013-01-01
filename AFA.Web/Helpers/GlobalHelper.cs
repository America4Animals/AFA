using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AFA.Web.Helpers
{
    public static class GlobalHelper
    {
        public static string GetServiceUrl()
        {
            return ConfigurationManager.AppSettings["baseServiceUrl"];
        }
    }
}