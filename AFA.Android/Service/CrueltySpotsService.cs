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
using Android.Util;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;

namespace AFA.Android.Service
{
    public class CrueltySpotsService
    {
        private StringBuilder _url;

        public const string RouteBase = "crueltyspots";
        public const string GooglePlacesRoute = "crueltyspots/googleplaces";

        public const string NameParamKey = "name";
        public const string CityParamKey = "city";
        public const string StateKey = "stateProvinceAbbreviation";
        public const string SortByParamKey = "sortBy";
        public const string SortOrderParamKey = "sortOrder";

        //public const string SearchQueryStringParams = "?name={0}&city={1}&stateProvinceAbbreviation={2}";

        //public CrueltySpotsService()
        //{
        //    _url = new StringBuilder();
        //}

        public CrueltySpotDto GetById(int id)
        {
            //var url = String.Format("{0}{1}/{2}{3}", AfaApplication.ServiceBaseUrl, RouteBase, id, AfaApplication.ServiceJsonContentTypeSuffix);
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.Append("/");
            _url.Append(id);
            _url.AppendJsonFormatQueryStringParam();

            using(var client = new WebClient())
            {
                var response = client.DownloadString(_url.ToString());
                return response.FromJson<CrueltySpotResponse>().CrueltySpot;
            }
        }

        public List<CrueltySpotDto> GetMany(CrueltySpotsDto request)
        {
            //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
            var url = new StringBuilder();
            url.Append(GetBaseUrl());
            url.AppendQueryStringParam(NameParamKey, request.Name);
            url.AppendQueryStringParam(CityParamKey, request.City);
            url.AppendQueryStringParam(StateKey, request.StateProvinceAbbreviation);
            url.AppendQueryStringParam(SortByParamKey, request.SortBy);
            url.AppendQueryStringParam(SortOrderParamKey, request.SortOrder);
            url.AppendJsonFormatQueryStringParam();

            using (var client = new WebClient())
            {
                var response = client.DownloadString(url.ToString());
                return response.FromJson<CrueltySpotsResponse>().CrueltySpots;
            }
        }

        public void GetAllAsync<T>(Action<T> callback)
        {
            var client = new WebClient();
            //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendJsonFormatQueryStringParam();

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<T>());
            };
            client.DownloadStringAsync(new Uri(_url.ToString()));
        }

        public void GetAllGooglePlacesAsync(Action<CrueltySpotsGooglePlacesResponse> callback)
        {
            var client = new WebClient();
            //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, GooglePlacesRoute, AfaApplication.ServiceJsonContentTypeSuffix);
            _url = new StringBuilder();
            _url.Append(GetBaseUrl(GooglePlacesRoute));
            _url.AppendJsonFormatQueryStringParam();

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<CrueltySpotsGooglePlacesResponse>());
            };

            client.DownloadStringAsync(new Uri(_url.ToString()));
        }

        //public List<CrueltySpotDto> Search(string name, string city, string stateAbbreviation)
        //{
        //    string queryString = String.Format(SearchQueryStringParams, name, city, stateAbbreviation);
        //    var url = String.Format("{0}{1}{2}{3}", AfaApplication.ServiceBaseUrl, RouteBase, queryString, AfaApplication.ServiceJsonContentTypeSuffixWithExistingQueryString);
            
        //    using (var client = new WebClient())
        //    {
        //        var response = client.DownloadString(url);
        //        return response.FromJson<CrueltySpotsResponse>().CrueltySpots;
        //    }
        //}

        public CrueltySpotResponse Post(CrueltySpotDto request)
        {
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendJsonFormatQueryStringParam();
            //Log.Debug("AFA Posting to ServiceHelper", request.ToJson());
            return ServiceHelper.PostJson<CrueltySpotResponse>(_url.ToString(), request.ToJson());
        }

        public void PostAsync(CrueltySpotDto crueltySpot, Action<CrueltySpotResponse> callback)
        {
            //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendJsonFormatQueryStringParam();

            string json = crueltySpot.ToJson();
            //Log.Debug("Posting JSON", json);
            ServiceHelper.PostJsonAsync(_url.ToString(), json, callback);
        }

        private string GetBaseUrl(string route = RouteBase)
        {
            return AfaApplication.ServiceBaseUrl + route;
        }

    }
}