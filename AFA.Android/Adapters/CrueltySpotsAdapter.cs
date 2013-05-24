using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Adapters
{
    public class CrueltySpotsAdapter : BaseAdapter<CrueltySpotDto>
        
    {
        private readonly Activity _context;
        private readonly IList<CrueltySpotDto> _crueltySpots;

        public CrueltySpotsAdapter(Activity context, IList<CrueltySpotDto> crueltySpots)
        {
            _context = context;
            _crueltySpots = crueltySpots;
        }

        public override CrueltySpotDto this[int position]
        {
            get { return _crueltySpots[position]; }
        }

        public override int Count
        {
            get { return _crueltySpots.Count; }
        }

        public override long GetItemId(int position)
        {
            return _crueltySpots[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CrueltySpotListItem, null);

            var crueltySpot = _crueltySpots[position];
            view.FindViewById<TextView>(Resource.Id.Name).Text = crueltySpot.Name;
            view.FindViewById<TextView>(Resource.Id.Address).Text = crueltySpot.Address;
            var resourceId = _context.Resources.GetIdentifier(crueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable",
                                                              _context.PackageName);
            view.FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);

            var distanceLabel = view.FindViewById<TextView>(Resource.Id.Distance);
            var gpsTracker = ((AfaApplication)_context.ApplicationContext).GetGpsTracker(_context);
            var myLat = gpsTracker.Latitude;
            var myLng = gpsTracker.Longitude;

            var geoHelper = new GeoHelper();
            double distance = geoHelper.Distance(myLat, myLng, crueltySpot.Latitude, crueltySpot.Longitude, 'M');
            distanceLabel.Text = distance.ToString("N2") + " miles";

            return view;
        }
    }
}