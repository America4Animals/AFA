using System.Web;
using System.Web.Mvc;

namespace AFA.ServiceHostAndWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}