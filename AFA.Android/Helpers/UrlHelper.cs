using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Helpers
{
    public static class UrlHelper
    {
        /// <summary>
        /// Will append key/value querystring param to a specified url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AppendQueryStringParam(this StringBuilder url, string key, string value)
        {
            if (url.Length > 0 && !String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value))
            {
                url.Append(!url.ToString().Contains("?") ? "?" : "&");
                url.Append(key);
                url.Append("=");
                url.Append(value);
            }
        }

        public static void AppendJsonFormatQueryStringParam(this StringBuilder url)
        {
            url.AppendQueryStringParam("format", "json");
        }
    }
}