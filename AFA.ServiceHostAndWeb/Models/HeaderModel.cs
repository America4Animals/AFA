using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Models
{
    public class HeaderModel
    {
        public bool IsLoggedIn { get; set; }
        public string DisplayName { get; set; }
    }
}