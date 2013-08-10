using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Utility;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Parse;
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

        public const string NameField = "name";
        public const string CityField = "city";
        public const string StateProvinceAbbreviationField = "stateProvinceAbbreviation";


        public async Task<CrueltySpot> GetByIdAsync(string id, bool includeCategory)
        {
            //var query = ParseObject.GetQuery(ParseHelper.CrueltySpotClassName);
            //if (includeCategory)
            //{
            //    query.Include("crueltySpotCategory");
            //}

            //var crueltySpotParse = await query.GetAsync(id);
            //return await ConvertToPoco(crueltySpotParse, includeCategory);
            var query = new ParseQuery<CrueltySpot>();
            if (includeCategory)
            {
				query = query.Include("crueltySpotCategory");
            }
            var result = await query.GetAsync(id);
            return result;
        }

        //public List<CrueltySpotDto> GetMany(CrueltySpotsDto request)
        //{
        //    //var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
        //    var url = new StringBuilder();
        //    url.Append(GetBaseUrl());
        //    url.AppendQueryStringParam(NameParamKey, request.Name);
        //    url.AppendQueryStringParam(CityParamKey, request.City);
        //    url.AppendQueryStringParam(StateKey, request.StateProvinceAbbreviation);
        //    url.AppendQueryStringParam(SortByParamKey, request.SortBy);
        //    url.AppendQueryStringParam(SortOrderParamKey, request.SortOrder);
        //    url.AppendJsonFormatQueryStringParam();

        //    using (var client = new WebClient())
        //    {
        //        var response = client.DownloadString(url.ToString());
        //        return response.FromJson<CrueltySpotsResponse>().CrueltySpots;
        //    }
        //}

        public async Task<List<CrueltySpot>> GetMany(CrueltySpot request, bool retrieveCategoryTypes)
        {
//            //var query = ParseObject.GetQuery(ParseHelper.CrueltySpotClassName);

////            if (!String.IsNullOrEmpty(request.Name))
////            {
////                query.WhereEqualTo(NameField, request.Name);
////            }
////
////            if (!string.IsNullOrEmpty(request.City))
////            {
////                query.WhereEqualTo(CityField, request.City);
////            }
////
////            if (!string.IsNullOrEmpty(request.StateProvinceAbbreviation))
////            {
////                query.WhereEqualTo(StateProvinceAbbreviationField, request.StateProvinceAbbreviation);
////            }

////			var query = ParseObject.GetQuery(ParseHelper.CrueltySpotClassName)
////				.WhereEqualTo(NameField, request.Name)
////				.WhereEqualTo(CityField, request.City)
////				.WhereEqualTo(StateProvinceAbbreviationField, request.StateProvinceAbbreviation);

////			var query = from crueltySpot in ParseObject.GetQuery(ParseHelper.CrueltySpotClassName)
////				where crueltySpot.Get<string>(NameField).ToLower().Equals(request.Name.ToLower())
////					&& crueltySpot.Get<string>(CityField).ToLower().Equals(request.City.ToLower())
////					&& crueltySpot.Get<string>(StateProvinceAbbreviationField).ToLower().Equals(request.StateProvinceAbbreviation.ToLower())
////					select crueltySpot;

//            var query = from crueltySpot in ParseObject.GetQuery(ParseHelper.CrueltySpotClassName)
//                where crueltySpot.Get<string>(NameField).Equals(request.Name)
//                    && crueltySpot.Get<string>(CityField).Equals(request.City)
//                    && crueltySpot.Get<string>(StateProvinceAbbreviationField).Equals(request.StateProvinceAbbreviation)
//                    select crueltySpot;

//            var crueltySpotsParse = await query.FindAsync();

//            var crueltySpots = new List<CrueltySpot>();
//            foreach (var crueltySpotParse in crueltySpotsParse)
//            {
//                var crueltySpot = await ConvertToPoco(crueltySpotParse, retrieveCategoryTypes);
//                crueltySpots.Add(crueltySpot);
//            }

//            return crueltySpots;

            var query = new ParseQuery<CrueltySpot>()
                .WhereEqualTo(NameField, request.Name)
                .WhereEqualTo(CityField, request.City)
                .WhereEqualTo(StateProvinceAbbreviationField, request.StateProvinceAbbreviation);
            var results = await query.FindAsync();
            return results.ToList();
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



        //public void GetAllAsync<T>(Action<T> callback)
        //{
        //    var client = new WebClient();
        //    var url = String.Format("{0}{1}{2}", AfaApplication.ServiceBaseUrl, RouteBase, AfaApplication.ServiceJsonContentTypeSuffix);
        //    _url = new StringBuilder();
        //    _url.Append(GetBaseUrl());
        //    _url.AppendJsonFormatQueryStringParam();

        //    client.DownloadStringCompleted += (sender, args) =>
        //    {
        //        var response = args.Result;
        //        callback(response.FromJson<T>());
        //    };
        //    client.DownloadStringAsync(new Uri(_url.ToString()));
        //}

		public async Task<List<CrueltySpot>> GetAllAsync(bool retrieveCategoryTypes)
        {
            //var query = ParseObject.GetQuery(ParseHelper.CrueltySpotClassName);
            //if (retrieveCategoryTypes) {
            //    query.Include ("crueltySpotCategory");
            //}

            //IEnumerable<ParseObject> crueltySpotsParse = await query.FindAsync();

            //var crueltySpots = new List<CrueltySpot>();
            //foreach (var crueltySpotParse in crueltySpotsParse)
            //{
            //    var crueltySpot = await ConvertToPoco(crueltySpotParse, retrieveCategoryTypes);
            //    crueltySpots.Add(crueltySpot);
            //}

            //return crueltySpots;

		    //var query = new ParseQuery<CrueltySpot>().Include("crueltySpotCategory");
			var query = new ParseQuery<CrueltySpot> ();
            
            if (retrieveCategoryTypes)
            {
				query = query.Include("crueltySpotCategory");
            }

		    var result = await query.FindAsync();
		    return result.ToList();
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

        //public void PostAsync(CrueltySpotDto crueltySpot, Action<CrueltySpotResponse> callback)
        //{
        //    _url = new StringBuilder();
        //    _url.Append(GetBaseUrl());
        //    _url.AppendJsonFormatQueryStringParam();

        //    string json = crueltySpot.ToJson();
        //    ServiceHelper.PostJsonAsync(_url.ToString(), json, callback);
        //}

        public async Task<string> SaveAsync(CrueltySpot crueltySpot)
        {
            //var crueltySpotCategoriesService = new CrueltySpotCategoriesService();
            ////var crueltySpotCategoryParse = await crueltySpotCategoriesService.GetParseObjectByIdAsync(crueltySpot.CrueltySpotCategoryId);
            //var crueltySpotParse = new ParseObject(ParseHelper.CrueltySpotClassName);
            //crueltySpotParse[NameField] = crueltySpot.Name;
            //crueltySpotParse["description"] = crueltySpot.Description;
            //crueltySpotParse["address"] = crueltySpot.Address;
            //crueltySpotParse[CityField] = crueltySpot.City;
            //crueltySpotParse[StateProvinceAbbreviationField] = crueltySpot.StateProvinceAbbreviation;
            //crueltySpotParse["zipcode"] = crueltySpot.Zipcode;
            //crueltySpotParse["phoneNumber"] = crueltySpot.PhoneNumber;
            //crueltySpotParse["email"] = crueltySpot.Email;
            //crueltySpotParse["webpageUrl"] = crueltySpot.WebpageUrl;
            //crueltySpotParse["location"] = new ParseGeoPoint(crueltySpot.Latitude, crueltySpot.Longitude);
            //crueltySpotParse["googlePlaceId"] = crueltySpot.GooglePlaceId;
            //crueltySpotParse["nonGooglePlaceAddressHash"] = crueltySpot.NonGooglePlaceAddressHash;
            ////crueltySpotParse["crueltySpotCategory"] = crueltySpotCategoryParse;
            //crueltySpotParse["crueltySpotCategory"] = ParseObject.CreateWithoutData(ParseHelper.CrueltySpotCategoryClassName, crueltySpot.CrueltySpotCategoryId);
            //await crueltySpotParse.SaveAsync();
            //return crueltySpotParse.ObjectId;

            crueltySpot.Location = new ParseGeoPoint(crueltySpot.Latitude, crueltySpot.Longitude);
            //crueltySpot.CrueltySpotCategory = ParseObject.CreateWithoutData<CrueltySpotCategory>(crueltySpot.CrueltySpotCategoryId);
            await crueltySpot.SaveAsync();
            return crueltySpot.ObjectId;
        }

        private string GetBaseUrl(string route = RouteBase)
        {
            return AfaApplication.ServiceBaseUrl + route;
        }

        //private async Task<CrueltySpot> ConvertToPoco(ParseObject crueltySpotParse, bool retrieveCategoryTypes)
        //{
        //    var crueltySpot = new CrueltySpot();
        //    crueltySpot.Id = crueltySpotParse.ObjectId;

        //    string name;
        //    string description;
        //    string address;
        //    string city;
        //    string stateProvinceAbbreviation;
        //    string zipcode;
        //    string phoneNumber;
        //    string email;
        //    string webpageUrl;
        //    string googlePlaceId;
        //    string nonGooglePlaceAddressHash;

        //    crueltySpotParse.TryGetValue(NameField, out name);
        //    crueltySpotParse.TryGetValue("description", out description);
        //    crueltySpotParse.TryGetValue("address", out address);
        //    crueltySpotParse.TryGetValue(CityField, out city);
        //    crueltySpotParse.TryGetValue(StateProvinceAbbreviationField, out stateProvinceAbbreviation);
        //    crueltySpotParse.TryGetValue("zipcode", out zipcode);
        //    crueltySpotParse.TryGetValue("phoneNumber", out phoneNumber);
        //    crueltySpotParse.TryGetValue("email", out email);
        //    crueltySpotParse.TryGetValue("webpageUrl", out webpageUrl);
        //    crueltySpotParse.TryGetValue("googlePlaceId", out googlePlaceId);
        //    crueltySpotParse.TryGetValue("nonGooglePlaceAddressHash", out nonGooglePlaceAddressHash);

        //    var parseGeoPoint = crueltySpotParse.Get<ParseGeoPoint>("location");
        //    crueltySpot.Latitude = parseGeoPoint.Latitude;
        //    crueltySpot.Longitude = parseGeoPoint.Longitude;

        //    crueltySpot.Name = name;
        //    crueltySpot.Description = description;
        //    crueltySpot.Address = address;
        //    crueltySpot.City = city;
        //    crueltySpot.StateProvinceAbbreviation = stateProvinceAbbreviation;
        //    crueltySpot.Zipcode = zipcode;
        //    crueltySpot.PhoneNumber = phoneNumber;
        //    crueltySpot.Email = email;
        //    crueltySpot.WebpageUrl = webpageUrl;
        //    crueltySpot.GooglePlaceId = googlePlaceId;
        //    crueltySpot.NonGooglePlaceAddressHash = nonGooglePlaceAddressHash;

        //    if (retrieveCategoryTypes)
        //    {
        //        var crueltySpotCategoriesService = new CrueltySpotCategoriesService();
        //        var crueltySpotCategoryParse = crueltySpotParse.Get<ParseObject>("crueltySpotCategory");
        //        await crueltySpotCategoryParse.FetchIfNeededAsync();
        //        //crueltySpot.CrueltySpotCategory = crueltySpotCategoriesService.ConvertToPoco(crueltySpotCategoryParse);
        //        //crueltySpot.CrueltySpotCategoryId = crueltySpot.CrueltySpotCategory.Id;
        //        crueltySpot.CrueltySpotCategoryId = crueltySpot.CrueltySpotCategory.ObjectId;
        //    }

        //    return crueltySpot;
        //}

    }
}