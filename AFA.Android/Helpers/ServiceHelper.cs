using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFA.ServiceModel.DTOs;
using Android.Util;
using ServiceStack.Text;

namespace AFA.Android.Helpers
{
    public static class ServiceHelper
    {
        public static void PostJsonAsync<T>(string url, string data, Action<T> callback)
        {
            var bytes = Encoding.Default.GetBytes(data);

            using (var client = new WebClient())
            {
                client.UploadDataCompleted += (sender, args) =>
                                                  {
                                                      var response = Encoding.Default.GetString(args.Result);
                                                      callback(response.FromJson<T>());
                                                  };

                client.Headers.Add("Content-Type", "application/json");
                client.UploadDataAsync(new Uri(url), bytes);
            }
        }
    }
}