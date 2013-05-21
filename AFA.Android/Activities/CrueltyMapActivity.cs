using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AFA.Android.Helpers;
using AFA_Android.Helpers;
using AFA.ServiceModel.DTOs;
using AFA.Android.Service;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace AFA.Android.Activities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	[Activity(Label = "@string/activity_label_mapwithmarkers")]
    public class CrueltyMapActivity : FragmentActivity
	{

		private GoogleMap _map;
		private SupportMapFragment _mapFragment;
		private GPSTracker _gpsTracker;
		List<CrueltySpotDto> _crueltySpots;
		private CrueltySpotsService crueltySpotsService;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker (this);
			SetContentView (Resource.Layout.CrueltyMap);
			InitMapFragment ();
			SetupMapIfNeeded ();

			CrueltyNavMenuHelper.InitCrueltyNavMenu (this, CrueltyNavMenuItem.TrackIt);
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			// Pause the GPS - we won't have to worry about showing the 
			// location.
			_map.MyLocationEnabled = false;

			_map.MarkerClick -= MapOnMarkerClick;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			SetupMapIfNeeded ();

			_map.MyLocationEnabled = true;

			// Setup a handler for when the user clicks on a marker.
			_map.MarkerClick += MapOnMarkerClick;
		}


		private void InitMapFragment ()
		{
			_mapFragment = SupportFragmentManager.FindFragmentByTag ("map") as SupportMapFragment;
			if (_mapFragment == null) {
				GoogleMapOptions mapOptions = new GoogleMapOptions ()
					.InvokeMapType (GoogleMap.MapTypeNormal)
						.InvokeZoomControlsEnabled (true)
						.InvokeCompassEnabled (true);

				FragmentTransaction fragTx = SupportFragmentManager.BeginTransaction ();
				_mapFragment = SupportMapFragment.NewInstance (mapOptions);
				fragTx.Add (Resource.Id.mapWithOverlay, _mapFragment, "map");
				fragTx.Commit ();
			}
		}

		private void MapOnMarkerClick (object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			Marker marker = markerClickEventArgs.P0; // TODO [TO201212142221] Need to fix the name of this with MetaData.xml
			Toast.MakeText (this, marker.Title, ToastLength.Short).Show ();

		}



		private void SetupMapIfNeeded ()
		{
			if (_map == null) {
				_map = _mapFragment.Map;
				if (_map != null) {
				

					//	AddInitialPolarBarToMap();
					LatLng myLocation = new LatLng (_gpsTracker.Latitude, _gpsTracker.Longitude);
					crueltySpotsService = new CrueltySpotsService();
					_crueltySpots = crueltySpotsService.GetMany(new CrueltySpotsDto
					                                                               {
						SortBy = "createdAt",
						SortOrder = "desc"
					});
					MarkerOptions mapOption;
					LatLng crueltyLocation;
					LatLngBounds.Builder builder = new LatLngBounds.Builder ();
					builder.Include (myLocation);
					foreach (CrueltySpotDto spot in _crueltySpots) // Loop through List with foreach
					{
						crueltyLocation = new LatLng (spot.Latitude, spot.Longitude);
						builder.Include (crueltyLocation);
						Console.WriteLine("latitude: " + spot.Latitude + " longtitude: "+spot.Longitude		 );
						mapOption = new MarkerOptions ()
							.SetPosition (crueltyLocation)
								.SetSnippet ("This is cruelty spot: "+spot.Name)
								.SetTitle ("This is cruelty spot: "+spot.Name);
						_map.AddMarker (mapOption);



					}

				//	LatLngBounds bounds = new LatLngBounds.Builder ().Include (myLocation).Build ();
					mapOption = new MarkerOptions ()
						.SetPosition (myLocation)
							.SetSnippet ("Your location")
							.SetTitle ("Your location")
							.InvokeIcon (BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueOrange));
					_map.AddMarker (mapOption);
					
					// Move the map so that it is showing the markers we added above.
					_map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom(myLocation,3));
				}
			}
		}
	}
}




