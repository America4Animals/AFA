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
using Android.Locations;

namespace AFA.Android.GooglePlacesApi
{
    public class GooglePlaces
    {
        private const string PlacesSearchUrl =
            "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&sensor=false&key={2}&types={3}&rankby=distance";

        private const string PlacesSearchByNameUrl =
            "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&sensor=false&key={2}&types={3}&rankby=distance&name={4}";

        private const string PlaceDetailsUrl =
            "https://maps.googleapis.com/maps/api/place/details/json?sensor=false&key={0}&reference={1}";

        // Google API Key
        private const string ApiKey = "AIzaSyB8V0OaG2ojKBPATLwJVaH2EztPQtLhg5M"; // place your API key here

        private double _latitude;
        private double _longitude;
        private double _radius;

        /// <summary>
        /// Search for nearby places
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="types"></param>
        /// <param name="callback"></param>
        public void Search(double latitude, double longitude, List<string> types, Action<PlacesList> callback)
        {
            Search(latitude, longitude, string.Join("|", types), callback);
        }

        /// <summary>
        /// Search for nearby places
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="types">Pipe-delimited list of place types</param>
        /// <param name="callback"></param>
        public void Search(double latitude, double longitude, string types, Action<PlacesList> callback)
        {
            var client = new WebClient();

            string url = string.Format(PlacesSearchUrl, latitude, longitude, ApiKey, types);
            Log.Info("PlacesUrl", url);

            client.DownloadStringCompleted += (sender, args) =>
            {
                var placeList = args.Result;
                callback(placeList.FromJson<PlacesList>());
            };

            client.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        /// Search for nearby place with specific name
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="types"></param>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void Search(double latitude, double longitude, string types, string name, Action<PlacesList> callback)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var client = new WebClient();
                string url = string.Format(PlacesSearchByNameUrl, latitude, longitude, ApiKey, types, name);

                Log.Info("PlacesByNameUrl", url);

                client.DownloadStringCompleted += (sender, args) =>
                                                      {
                                                          var placeList = args.Result;
                                                          callback(placeList.FromJson<PlacesList>());
                                                      };

                client.DownloadStringAsync(new Uri(url));
            }
            else
            {
                // Name not specified, do search without specifying a name
                Search(latitude, longitude, types, callback);
            }
        }

        /// <summary>
        /// Search for a place with a specified name and location
        /// </summary>
        /// <param name="context"></param>
        /// <param name="locationName"></param>
        /// <param name="types"></param>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void Search(Context context, string locationName, string types, string name, Action<PlacesList> callback)
        {
            var geocoder = new Geocoder(context);
            Log.Debug("SearchByLocation", locationName);
            var locations = geocoder.GetFromLocationName(locationName, 1);
            if (locations.Any())
            {
                var location = locations.First();
                var lat = location.Latitude;
                var lng = location.Longitude;

                Search(lat, lng, types, name, callback);
            }
            else
            {
                Log.Debug("SearchByLocation", "No results found");
                // Couldn't find a location for the specified locationName, callback with empty collection
                callback(new PlacesList
                             {
                                 status = "",
                                 results = new List<Place>()
                             });
            }
        }

        /// <summary>
        /// Get details about a place
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="callback"></param>
        public void GetDetails(string reference, Action<PlaceDetails> callback)
        {
            var client = new WebClient();
            string url = string.Format(PlaceDetailsUrl, ApiKey, reference);
            Log.Info("PlaceDetailsUrl", url);

            client.DownloadStringCompleted += (sender, args) =>
                                                  {
                                                      var placeDetails = args.Result;
                                                      callback(placeDetails.FromJson<PlaceDetails>());
                                                  };

            client.DownloadStringAsync(new Uri(url));
        }

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

        public List<Address_Component> address_components { get; set; }
        public string website { get; set; }

        public double Distance { get; set; }

        // accessors
        public string Address
        {
            get
            {
                if (address_components == null)
                {
                    return null;
                }

                var address = new StringBuilder();

                var streetNumber =
                    address_components.FirstOrDefault(
                        ac => ac.types.Contains(AddressType.street_number.ToString()));

                var streetName =
                    address_components.FirstOrDefault(ac => ac.types.Contains(AddressType.route.ToString()));

                if (streetName != null)
                {
                    if (streetNumber != null)
                    {
                        address.Append(streetNumber.long_name);
                        address.Append(" ");
                    }

                    address.Append(streetName.long_name);
                }

                if (address.Length == 0)
                {
                    return null;
                }

                return address.ToString();
            }
        }

        public string City
        {
            get
            {
                if (address_components == null)
                {
                    return null;
                }

                var city =
                    address_components.FirstOrDefault(ac => ac.types.Contains(AddressType.locality.ToString()));

                if (city == null)
                {
                    return null;
                }

                return city.long_name;
            }
        }

        public string StateOrProvince
        {
            get
            {
                if (address_components == null)
                {
                    return null;
                }

                var stateOrProvince =
                    address_components.FirstOrDefault(ac => ac.types.Contains(AddressType.administrative_area_level_1.ToString()));

                if (stateOrProvince == null)
                {
                    return null;
                }

                return stateOrProvince.short_name;
            }
        }

        public string Country
        {
            get
            {
                if (address_components == null)
                {
                    return null;
                }

                var country =
                    address_components.FirstOrDefault(ac => ac.types.Contains(AddressType.country.ToString()));

                if (country == null)
                {
                    return null;
                }

                return country.long_name;
            }
        }

        public string PostalCode
        {
            get
            {
                if (address_components == null)
                {
                    return null;
                }

                var postalCode =
                    address_components.FirstOrDefault(ac => ac.types.Contains(AddressType.postal_code.ToString()));

                if (postalCode == null)
                {
                    return null;
                }

                return postalCode.long_name;
            }
        }
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
        public decimal lat { get; set; }
        public decimal lng { get; set; }
    }

    public class Address_Component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public enum AddressType
    {
        street_number,
        route,
        locality,
        administrative_area_level_1,
        country,
        postal_code
    }
}