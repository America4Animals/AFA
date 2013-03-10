using AFA.ServiceHostAndWeb.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Helpers
{
    public static class SystemMessages
    {
        static SystemMessages()
        {
            StatusCodeMessages = new Dictionary<string, string>();
            StatusCodeMessages.Add(StatusCode.AddCrueltySpotSuccess.ToString(), "Successfully added cruelty spot");
        }

        public static string GetMessage(string code)
        {
            if (StatusCodeMessages.ContainsKey(code))
            {
                return StatusCodeMessages[code];
            }
            return "";
        }

        public static Dictionary<string, string> StatusCodeMessages { get; set; }  
    }
}