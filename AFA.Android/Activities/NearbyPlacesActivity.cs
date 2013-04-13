using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
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

        private GPSTracker _gpsTracker;
        private GooglePlacesApi.GooglePlaces _googlePlaces;
        private ProgressDialog _loading;

        private ListView _placesListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Places);

            _gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
            _googlePlaces = new GooglePlacesApi.GooglePlaces();
            _loading = LoadingDialogManager.ShowLoadingDialog(this);
            Log.Debug("Position Lat:", _gpsTracker.Latitude.ToString());
            Log.Debug("Position Lng:", _gpsTracker.Longitude.ToString());

            _placesListView = FindViewById<ListView>(Resource.Id.Places);

            DoPlacesSearch();

            var searchButton = FindViewById<ImageButton>(Resource.Id.searchButton);
            searchButton.Click += (sender, args) =>
                                      {
                                          _loading.Show();
                                          var placeNameInput = FindViewById<EditText>(Resource.Id.placeNameSearch);
                                          var locationNameInput = FindViewById<EditText>(Resource.Id.locationSearch);

                                          var placeNameSpecified = !String.IsNullOrWhiteSpace(placeNameInput.Text);
                                          var locationNameSpecified = !String.IsNullOrWhiteSpace(locationNameInput.Text);

                                          if (!placeNameSpecified && !locationNameSpecified)
                                          {
                                              // Nothing specified, do default search
                                              DoPlacesSearch();
                                          }
                                          else if (locationNameSpecified)
                                          {
                                              _googlePlaces.Search(this, locationNameInput.Text, PlaceTypes, placeNameInput.Text, SearchCallback);
                                          }
                                          else
                                          {
                                              _googlePlaces.Search(_gpsTracker.Latitude, _gpsTracker.Longitude,
                                                                   PlaceTypes, placeNameInput.Text, SearchCallback);
                                          }
                                      };
        }

        private void DoPlacesSearch()
        {
            _googlePlaces.Search(_gpsTracker.Latitude, _gpsTracker.Longitude, PlaceTypes, SearchCallback);
        }

        private void SearchCallback(GooglePlacesApi.PlacesList placesList)
        {
            RunOnUiThread(() =>
                              {
                                  // ToDo: Check Status and handle non-OK

                                  var placeResults = placesList.results;
                                  _placesListView.Adapter = new PlacesListAdapter(this, placeResults);

                                  _placesListView.ItemClick += (sender, e) =>
                                                                   {
                                                                       var place = placeResults[e.Position];
                                                                       var intent = new Intent(this,
                                                                                               typeof (
                                                                                                   ReportCrueltyActivity
                                                                                                   ));
                                                                       intent.PutExtra(AppConstants.PlaceNameKey,
                                                                                       place.name);
                                                                       intent.PutExtra(AppConstants.PlaceVicinityKey,
                                                                                       place.vicinity);
                                                                       intent.PutExtra(AppConstants.PlaceReferenceKey,
                                                                                       place.reference);
                                                                       StartActivity(intent);
                                                                   };

                                  _loading.Hide();
                              });
        }
    }
}