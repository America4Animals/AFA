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

        public CrueltySpotDto GetById(int id)
        {
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

        public void GetManyAsync<T>(CrueltySpotsDto request, Action<T> callback)
        {
            var client = new WebClient();
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendQueryStringParam(NameParamKey, request.Name);
            _url.AppendQueryStringParam(CityParamKey, request.City);
            _url.AppendQueryStringParam(StateKey, request.StateProvinceAbbreviation);
            _url.AppendQueryStringParam(SortByParamKey, request.SortBy);
            _url.AppendQueryStringParam(SortOrderParamKey, request.SortOrder);
            _url.AppendJsonFormatQueryStringParam();
			//Log.Debug ("URL: ", _url.ToString());

            client.DownloadStringCompleted += (sender, args) =>
            {
                var response = args.Result;
                callback(response.FromJson<T>());
            };

            client.DownloadStringAsync(new Uri(_url.ToString()));
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

        public CrueltySpotResponse Post(CrueltySpotDto request)
        {
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendJsonFormatQueryStringParam();
            return ServiceHelper.PostJson<CrueltySpotResponse>(_url.ToString(), request.ToJson());
        }

        public void PostAsync(CrueltySpotDto crueltySpot, Action<CrueltySpotResponse> callback)
        {
            _url = new StringBuilder();
            _url.Append(GetBaseUrl());
            _url.AppendJsonFormatQueryStringParam();

            string json = crueltySpot.ToJson();
            ServiceHelper.PostJsonAsync(_url.ToString(), json, callback);
        }

        private string GetBaseUrl(string route = RouteBase)
        {
            return AfaApplication.ServiceBaseUrl + route;
        }

    }
}