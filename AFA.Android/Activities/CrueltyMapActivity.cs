using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using AFA.Android.Helpers;
using AFA_Android.Helpers;
using AFA.ServiceModel.DTOs;
using AFA.Android.Service;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFA.Android.Activities
{
	[Activity(Label = "@string/activity_label_mapwithmarkers")]
	public class CrueltyMapActivity : FragmentActivity , GoogleMap.IOnInfoWindowClickListener
	{

		class CustomInfoWindowAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
		{



			// These a both viewgroups containing an ImageView with id "badge" and two TextViews with id
			// "title" and "snippet".
		
			private readonly View mContents;
			CrueltyMapActivity parent;

			internal CustomInfoWindowAdapter (CrueltyMapActivity parent)
			{

				mContents = parent.LayoutInflater.Inflate (Resource.Layout.custom_info_content, null);
				this.parent = parent;

			}

			public View GetInfoWindow (Marker marker)
			{
				return null;
			}

			public View GetInfoContents (Marker marker)
			{

				Render (marker, mContents);
				return mContents;
			}

			private void Render (Marker marker, View view)
			{

				var resourceId = 0;
				if (parent._crueltyLookup.ContainsKey (marker.Title)) {
					CrueltySpotDto spot = parent._crueltyLookup [marker.Title];
					resourceId = parent.Resources.GetIdentifier (spot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", parent.PackageName);
				}
				((ImageView)view.FindViewById (Resource.Id.badge)).SetImageResource (resourceId);

				String title = marker.Title;
				TextView titleUi = ((TextView)view.FindViewById (Resource.Id.title));
				if (title != null) {
					// Spannable string allows us to edit the formatting of the text.
					SpannableString titleText = new SpannableString (title);
				
					// FIXME: this somehow rejects to compile
					//titleText.SetSpan (new ForegroundColorSpan(Color.Red), 0, titleText.Length, st);
					titleUi.TextFormatted = (titleText);
				} else {
					titleUi.Text = ("");
				}

				String snippet = marker.Snippet;
				TextView snippetUi = ((TextView)view.FindViewById (Resource.Id.snippet));
				if (snippet != null) {
					SpannableString snippetText = new SpannableString (snippet);
					//	snippetText.SetSpan(new ForegroundColorSpan(Color.Magenta), 0, 10, 0);
					//	snippetText.SetSpan(new ForegroundColorSpan(Color.Blue), 12, 21, 0);
					snippetUi.TextFormatted = (snippetText);
				} else {
					snippetUi.Text = ("");
				}
			}
		}

		private GoogleMap _map;
		private SupportMapFragment _mapFragment;
		private GPSTracker _gpsTracker;
		List<CrueltySpotDto> _crueltySpots;
		private CrueltySpotsService crueltySpotsService;
		Dictionary<String,CrueltySpotDto> _crueltyLookup = new Dictionary<String,CrueltySpotDto> ();

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
			_map.Clear ();

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
			float zoomValue = 10;
			if (_map.CameraPosition.Zoom > zoomValue) {
				zoomValue = _map.CameraPosition.Zoom;
			}
			_map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom(marker.Position,zoomValue));
			marker.ShowInfoWindow ();

		}

		private void SetupMapIfNeeded ()
		{
			if (_map == null) {
				_map = _mapFragment.Map;
			}
			if (_map != null) {
				

				//	AddInitialPolarBarToMap();
				LatLng myLocation = new LatLng (_gpsTracker.Latitude, _gpsTracker.Longitude);
				crueltySpotsService = new CrueltySpotsService ();
				_crueltySpots = crueltySpotsService.GetMany (new CrueltySpotsDto
					                                                               {
						SortBy = "createdAt",
						SortOrder = "desc"
					});
				MarkerOptions mapOption;
				LatLng crueltyLocation;
				LatLngBounds.Builder builder = new LatLngBounds.Builder ();
				builder.Include (myLocation);
				foreach (CrueltySpotDto spot in _crueltySpots) { // Loop through List with foreach
					crueltyLocation = new LatLng (spot.Latitude, spot.Longitude);
					builder.Include (crueltyLocation);
					Console.WriteLine ("latitude: " + spot.Latitude + " longtitude: "+spot.Longitude);
					mapOption = new MarkerOptions ()
							.SetPosition (crueltyLocation)
								.SetSnippet (spot.Address)
								.SetTitle (spot.Name);
					_crueltyLookup.Add (spot.Name, spot);
					_map.AddMarker (mapOption);



				}

				//	LatLngBounds bounds = new LatLngBounds.Builder ().Include (myLocation).Build ();
				/*	mapOption = new MarkerOptions ()
						.SetPosition (myLocation)
							.SetSnippet ("Your location")
							.SetTitle ("Your location")
							.InvokeIcon (BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueOrange));
				_map.AddMarker (mapOption);*/
					
				// Move the map so that it is showing the markers we added above.
				_map.SetInfoWindowAdapter (new CustomInfoWindowAdapter (this));

				// Set listeners for marker events.  See the bottom of this class for their behavior.

				_map.SetOnInfoWindowClickListener (this);

				_map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom(myLocation,3));
			}
		
		}

		public void OnInfoWindowClick (Marker marker)
		{
			if (_crueltyLookup.ContainsKey (marker.Title)) {
				CrueltySpotDto spot = _crueltyLookup [marker.Title];
				NavigateToCrueltySpotDetails (spot.Id);
			}
		}

		private void NavigateToCrueltySpotDetails (int crueltySpotId)
		{
			var intent = new Intent (this, typeof(CrueltySpotActivity));
			intent.PutExtra (AppConstants.CrueltySpotIdKey, crueltySpotId);
			StartActivity (intent);
		}
	}
}




