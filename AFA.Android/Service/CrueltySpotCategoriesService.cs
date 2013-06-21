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
    public class CrueltySpotCategoriesService
    {
        private StringBuilder _url;

        public const string RouteBase = "crueltyspotcategories";

        public void GetAllAsync(Action<CrueltySpotCategoriesResponse> callback)
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

        public void GetByIdAsync<T>(int id, Action<T> callback)
        {
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.Append("/");
            _url.Append(id);
            _url.AppendJsonFormatQueryStringParam();

            var client = new WebClient();
            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<T>());
            };

            client.DownloadStringAsync(new Uri(_url.ToString()));
        }

        private string GetBaseUrl(string route = RouteBase)
        {
            return AfaApplication.ServiceBaseUrl + route;
        }
    }
}