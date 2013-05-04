using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFA.Android.Helpers;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;

namespace AFA.Android.Service
{
    public static class CrueltySpotService
    {
        public static string RouteBase = "crueltyspots";
        public static string GooglePlacesRoute = "crueltyspots/googleplaces";

        public static string SearchQueryStringParams = "?name={0}&city={1}&stateProvinceAbbreviation={2}";

        public static CrueltySpotDto GetById(int id)
        {
            var url = String.Format("{0}{1}/{2}{3}", AfaApplication.ServiceBaseUrl, RouteBase, id, AfaApplication.ServiceJsonContentTypeSuffix);

            using(var client = new WebClient())
            {   
                var response = client.DownloadString(url);
                return response.FromJson<CrueltySpotResponse>().CrueltySpot;
            }
        }

        public static List<CrueltySpotDto> GetAll()
        {
            var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);

            using (var client = new WebClient())
            {
                var response = client.DownloadString(url);
                return response.FromJson<CrueltySpotsResponse>().CrueltySpots;
            }
        }


        public static void GetAllAsync<T>(Action<T> callback)
        {
            var client = new WebClient();
            var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<T>());
            };
            client.DownloadStringAsync(new Uri(url));
        }

        public static void GetAllGooglePlacesAsync(Action<CrueltySpotsGooglePlacesResponse> callback)
        {
            var client = new WebClient();
            var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, GooglePlacesRoute, AfaApplication.ServiceJsonContentTypeSuffix);

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<CrueltySpotsGooglePlacesResponse>());
            };

            client.DownloadStringAsync(new Uri(url));
        }

        public static List<CrueltySpotDto> Search(string name, string city, string stateAbbreviation)
        {
            string queryString = String.Format(SearchQueryStringParams, name, city, stateAbbreviation);
            var url = String.Format("{0}{1}{2}{3}", AfaApplication.ServiceBaseUrl, RouteBase, queryString, AfaApplication.ServiceJsonContentTypeSuffixWithExistingQueryString);
            
            using (var client = new WebClient())
            {
                var response = client.DownloadString(url);
                return response.FromJson<CrueltySpotsResponse>().CrueltySpots;
            }
        }

        public static void PostAsync(CrueltySpotDto crueltySpot, Action<CrueltySpotResponse> callback)
        {
            var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
            string json = crueltySpot.ToJson();
            ServiceHelper.PostJsonAsync(url, json, callback);
        }
    }
}