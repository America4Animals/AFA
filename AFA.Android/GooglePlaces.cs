using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFA.Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ServiceStack.Text;

namespace AFA.Android.GooglePlacesApi
{
    public class GooglePlaces
    {
        private const string PlacesSearchUrl =
            "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&sensor=false&key={2}&types={3}&rankby=distance";

        // Google API Key
        private const string ApiKey = "AIzaSyB8V0OaG2ojKBPATLwJVaH2EztPQtLhg5M"; // place your API key here

        private double _latitude;
        private double _longitude;
        private double _radius;

        public void Search(double latitude, double longitude, List<string> types, Action<PlacesList> callback)
        {
            var client = new WebClient();
            var typesParam = new StringBuilder();
            foreach (var type in types)
            {
                if (typesParam.Length > 0)
                {
                    typesParam.Append("|");
                }
                typesParam.Append(type);
            }

            string url = string.Format(PlacesSearchUrl, latitude, longitude, ApiKey, typesParam);
            Log.Info("PlacesUrl", url);

            client.DownloadStringCompleted += (sender, args) =>
            {
                var placeList = args.Result;
                callback(placeList.FromJson<PlacesList>());
            };

            client.DownloadStringAsync(new Uri(url));
        }

        //public List<Place> GetSortedListByDistance(List<Place> places, double lat, double lng)
        //{
        //    var geoHelper = new GeoHelper();
        //    foreach (var place in places)
        //    {
        //        place.Distance = geoHelper.Distance(lat, lng, place.geometry.location.lat, place.geometry.location.lng, 'M');
        //    }
        //    return places.OrderBy(p => p.Distance).ToList();
        //}
    }

    public class Place
    {
        public string id { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string icon { get; set; }
        public string vicinity { get; set; }
        public Geometry geometry { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }

        public double Distance { get; set; }
    }

    public class PlacesList
    {
        public string status { get; set; }
        public List<Place> results { get; set; }
    }

    public class PlaceDetails
    {
        public string status { get; set; }
        public Place result { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
}