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
        private const string PlaceTypes =
            "airport|amusement_park|aquarium|art_gallery|bakery|bar|beauty_salon|book_store|bowling_alley|bus_station|cafe|casino|church|city_hall|clothing_store|convenience_store|courthouse|department_store|doctor|embassy|establishment|food|funeral_home|gas_station|general_contractor|grocery_or_supermarket|hardware_store|health|hindu_temple|home_goods_store|hospital|liquor_store|local_government_office|lodging|meal_delivery|meal_takeaway|movie_theater|museum|night_club|park|pet_store|pharmacy|physiotherapist|place_of_worship|police|restaurant|rv_park|school|shoe_store|shopping_mall|spa|stadium|store|subway_station|train_station|university|veterinary_care|zoo";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Places);

            var gpsTracker = ((AfaApplication) ApplicationContext).GetGpsTracker(this);
            var googlePlaces = new GooglePlacesApi.GooglePlaces();
            var loading = ProgressDialog.Show(this, "Retrieving Nearby Places", "Please wait...", true);
            Log.Debug("Position Lat:", gpsTracker.Latitude.ToString());
            Log.Debug("Position Lng:", gpsTracker.Longitude.ToString());

            var placesListView = FindViewById<ListView>(Resource.Id.Places);

            googlePlaces.Search(gpsTracker.Latitude, gpsTracker.Longitude, PlaceTypes, placesList => RunOnUiThread(() =>
            {
                // ToDo: Check Status and handle non-OK

                var placeResults = placesList.results;
                placesListView.Adapter = new PlacesListAdapter(this, placeResults);

                placesListView.ItemClick += (sender, e) =>
                {
                    var place = placeResults[e.Position];
                    var intent = new Intent(this, typeof(ReportCrueltyActivity));
                    intent.PutExtra("placeName", place.name);
                    intent.PutExtra("placeVicinity", place.vicinity);
                    intent.PutExtra("placeReference", place.reference);
                    StartActivity(intent);
                };

                loading.Hide();
            }));

            
        }
    }
}