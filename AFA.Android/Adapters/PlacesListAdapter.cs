using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android
{
    public class PlacesListAdapter : BaseAdapter<GooglePlacesApi.Place>
    {
        private readonly Activity _context;
        private readonly IList<GooglePlacesApi.Place> _places;

        public PlacesListAdapter(Activity context, IList<GooglePlacesApi.Place> places)
        {
            _context = context;
            _places = places;
        }

        public override GooglePlacesApi.Place this[int position]
        {
            get { return _places[position]; }
        }

        public override int Count
        {
            get { return _places.Count; }
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.PlaceListItem, null);

            var place = _places[position];
            view.FindViewById<TextView>(Resource.Id.Name).Text = place.name;
            view.FindViewById<TextView>(Resource.Id.Address).Text = place.vicinity;
            var distanceLabel = view.FindViewById<TextView>(Resource.Id.Distance);
            var afaApplication = (AfaApplication)_context.ApplicationContext;
            var gpsTracker = ((AfaApplication)_context.ApplicationContext).GetGpsTracker(_context);
            var myLat = gpsTracker.Latitude;
            var myLng = gpsTracker.Longitude;
            var placeLat = place.geometry.location.lat;
            var placeLng = place.geometry.location.lng;

            var geoHelper = new GeoHelper();
            double distance = geoHelper.Distance(myLat, myLng, Convert.ToDouble(placeLat), Convert.ToDouble(placeLng), 'M');
            distanceLabel.Text = distance.ToString("N2") + " miles";
            return view;
        }
    }
}