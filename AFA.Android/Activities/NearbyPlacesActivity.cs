using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA_Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Activities
{
    [Activity(Label = "Nearby Places")]
    public class NearbyPlacesActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Places);

            var gpsTracker = ((AfaApplication) ApplicationContext).GetGpsTracker(this);
            var googlePlaces = new GooglePlacesApi.GooglePlaces();
            var loading = ProgressDialog.Show(this, "Retrieving Nearby Places", "Please wait...", true);
            Log.Debug("Position Lat:", gpsTracker.Latitude.ToString());
            Log.Debug("Position Lng:", gpsTracker.Longitude.ToString());

            var types = new List<string>();
            types.Add("amusement_park");
            types.Add("aquarium");
            types.Add("veterinary_care");
            types.Add("zoo");

            googlePlaces.Search(gpsTracker.Latitude, gpsTracker.Longitude, types, placesList => RunOnUiThread(() =>
            {
                var placesListView = FindViewById<ListView>(Resource.Id.Places);
                //var sortedResults = googlePlaces.GetSortedListByDistance(placesList.results, gpsTracker.Latitude,
                //                                                     gpsTracker.Longitude);
               
                //placesListView.Adapter = new PlacesListAdapter(this, sortedResults);

                placesListView.Adapter = new PlacesListAdapter(this, placesList.results);
                loading.Hide();
            }));
        }
    }
}