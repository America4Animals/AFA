using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFA.Android.Helpers;
using AFA.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;
using Android.Util;

namespace AFA.Android.Service
{
    public static class CrueltySpotCategoriesService
    {
        public static string RouteBase = "crueltyspotcategories";

        public static void GetAllAsync(Action<CrueltySpotCategoriesResponse> callback)
        {
            var client = new WebClient();
            //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
            var url = new StringBuilder();
            url.Append(String.Format("{0}{1}", AfaApplication.ServiceBaseUrl, RouteBase));
            url.AppendJsonFormatQueryStringParam();

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<CrueltySpotCategoriesResponse>());
            };
            client.DownloadStringAsync(new Uri(url.ToString()));
        }
    }
}