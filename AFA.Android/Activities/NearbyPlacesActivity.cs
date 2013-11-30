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
//using ServiceStack.Text;

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
        private List<GooglePlacesApi.Place> _placeResults;
        private string _nextPageToken;

        private Button _loadMoreButton;
        private Button _addNewPlaceButton;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Places);

            _gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
			_googlePlaces = new GooglePlacesApi.GooglePlaces(AfaConfig.GoogleApiKey);
		    _loading = DialogManager.ShowLoadingDialog(this, "Retrieving Nearby Places");

            Log.Debug("Position Lat:", _gpsTracker.Latitude.ToString());
            Log.Debug("Position Lng:", _gpsTracker.Longitude.ToString());

            _placesListView = FindViewById<ListView>(Resource.Id.Places);

            var layout = new LinearLayout(this);
            _loadMoreButton = new Button(this);
            _loadMoreButton.Text = "Load More Results";
            _loadMoreButton.LayoutParameters = new TableLayout.LayoutParams(TableLayout.LayoutParams.WrapContent,
                                                                            TableLayout.LayoutParams.WrapContent, 1f);
            layout.AddView(_loadMoreButton);
            _addNewPlaceButton = new Button(this);
            _addNewPlaceButton.Text = "Add New";
            _addNewPlaceButton.LayoutParameters = new TableLayout.LayoutParams(TableLayout.LayoutParams.WrapContent,
                                                                            TableLayout.LayoutParams.WrapContent, 1f);
            layout.AddView(_addNewPlaceButton);
            _placesListView.AddFooterView(layout);
            
            _loadMoreButton.Click += (sender, args) => FetchMoreResults();

            _addNewPlaceButton.Click += (sender, args) =>
                                            {
                                                var intent = new Intent(this, typeof (AddPlaceActivity));
                                                StartActivity(intent);
                                            };

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

                                  _placeResults = placesList.results;
                                  _nextPageToken = placesList.next_page_token;
                                  _loadMoreButton.Visibility = String.IsNullOrEmpty(_nextPageToken) ? ViewStates.Gone : ViewStates.Visible;

                                  _placesListView.Adapter = new PlacesListAdapter(this, _placeResults);
                                  _placesListView.ItemClick += PlacesListViewItemClick;
                                  _loading.Hide();
                              });
        }

        private void FetchMoreCallback(GooglePlacesApi.PlacesList placesList)
        {
            RunOnUiThread(() =>
            {
                // ToDo: Check Status and handle non-OK

                _placeResults.AddRange(placesList.results);
                _nextPageToken = placesList.next_page_token;
                _loadMoreButton.Visibility = String.IsNullOrEmpty(_nextPageToken) ? ViewStates.Invisible : ViewStates.Visible;

                // Get ListView current position - used to maintain scroll position
                int currentPosition = _placesListView.FirstVisiblePosition;

                _placesListView.Adapter = new PlacesListAdapter(this, _placeResults);
                _placesListView.ItemClick += PlacesListViewItemClick;
                _placesListView.SetSelectionFromTop(currentPosition + 1, 0);

                _loading.Hide();
            });
        }

        private void PlacesListViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var place = _placeResults[e.Position];

            if (place.HasBeenReported)
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Cruelty already reported here")
                    .SetMessage("The location you have chosen is already on the cruelty map.")
                    .SetNegativeButton("Cancel", (o, args) => {})
                    .SetPositiveButton("Edit Info", (o, args) =>
                                                        {
                                                            var intent = new Intent(this, typeof (CrueltySpotActivity));
                                                            Log.Debug("CrueltySpotId", place.AfaId.ToString());
                                                            intent.PutExtra(AppConstants.CrueltySpotIdKey, place.AfaId);
                                                            StartActivity(intent);
                                                        })
                    .Show();
            }
            else
            {
                //var intent = new Intent(this, typeof(ReportCrueltyFragment));
                var intent = new Intent(this, typeof(IntroActivity));
                intent.PutExtra("tab", "report");
                // Must be a google place because if not, it would have been flagged as reported
                var googlePlace = new AFA.Android.Helpers.GooglePlace();
                googlePlace.Name = place.name;
                googlePlace.Vicinity = place.vicinity;
                googlePlace.Reference = place.reference;
				CrueltyReport _crueltyReport = ((AfaApplication)ApplicationContext).CrueltyReport;
                _crueltyReport.GooglePlace = googlePlace;
  
                StartActivity(intent);
            }
            
        }

        private void FetchMoreResults()
        {
            if (!String.IsNullOrEmpty(_nextPageToken))
            {
                _loading.Show();
                _googlePlaces.Search(_nextPageToken, _gpsTracker.Latitude, _gpsTracker.Longitude, FetchMoreCallback);
            }
        }
    }
}