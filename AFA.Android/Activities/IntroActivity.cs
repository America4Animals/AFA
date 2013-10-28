using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ActionBar_Sherlock.App;
using SherlockActionBar = ActionBar_Sherlock.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using ActionBar_Sherlock.View;
using Android.Support.V4.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;
using AFA.Android.Helpers;
using AFA_Android.Helpers;
using AFA.Android.Service;
using AFA.Android.Library.ServiceModel;



namespace AFA.Android.Activities
{
	[Activity (Label = "@string/ApplicationName")]
	public class IntroActivity : SherlockFragmentActivity, ActionBar.ITabListener, GoogleMap.IOnInfoWindowClickListener
	{
		Fragment reportFragment;
		Fragment fightitFragment;
		private GoogleMap _map;
		private SupportMapFragment _mapFragment;
		private GPSTracker _gpsTracker;
		//List<CrueltySpotDto> _crueltySpots;
		List<CrueltySpot> _crueltySpots;
		private CrueltySpotsService _crueltySpotsService;
		//Dictionary<String,CrueltySpotDto> _crueltyLookup = new Dictionary<String,CrueltySpotDto> ();
		Dictionary<String, CrueltySpot> _crueltyLookup = new Dictionary<String, CrueltySpot>();
		Dictionary<String,float> _pinColorLookup = new Dictionary<String,float> 
		{
			{"cariaggespin",128}, {"foiegraspin",56}, {"labspin",198}, {"morepin",327}, 
			{"petstorespin",209}, {"racespin",259}, {"rodeopin",33},{"sharkpin",302}, 
			{"furpin",68} ,{"performancepin",354}
		};

		class CustomInfoWindowAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
		{

			// These a both viewgroups containing an ImageView with id "badge" and two TextViews with id
			// "title" and "snippet".

			private readonly View mContents;
			IntroActivity parent;

			internal CustomInfoWindowAdapter (IntroActivity parent)
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
				if (parent._crueltyLookup.ContainsKey (marker.Id)) {
					CrueltySpot spot = parent._crueltyLookup [marker.Id];
					if (spot.CrueltySpotCategory.IconName != null) {
						resourceId = parent.Resources.GetIdentifier (spot.CrueltySpotCategory.IconName.Replace(".png", ""), "drawable", parent.PackageName);
					}
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
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Intro);
			_gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker (this);

			SupportActionBar.NavigationMode = SherlockActionBar.NavigationModeTabs;

			ActionBar.Tab reportItTab = SupportActionBar.NewTab ();
			reportItTab.SetText ("Report it");
			reportItTab.SetTabListener (this);
			reportItTab.SetTag ("report");
		
			SupportActionBar.AddTab (reportItTab);


			ActionBar.Tab trackItTab = SupportActionBar.NewTab ();

			trackItTab.SetText ("Track it");
			trackItTab.SetTabListener (this);
			trackItTab.SetTag ("track");

			SupportActionBar.AddTab (trackItTab);


			ActionBar.Tab fightItTab = SupportActionBar.NewTab ();
			fightItTab.SetText ("Fight it");
			fightItTab.SetTabListener (this);
			fightItTab.SetTag ("fight");
			SupportActionBar.AddTab (fightItTab);

			SupportActionBar.SelectTab (trackItTab);
		}

		public void OnTabReselected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{


		}

		public void OnTabSelected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
			if ((String)tab.Tag == "report") {
				if (reportFragment == null) {
					reportFragment = new ReportCrueltyActivity ();

					transaction.Add (Resource.Id.fragmentContainer, reportFragment, "report");

				} else {

					transaction.Show (reportFragment);
		
				}
			} else if ((String)tab.Tag == "track") {
				InitMapFragment ();

				transaction.Show (_mapFragment);
				SetupMapIfNeeded ();
			} else if ((String)tab.Tag == "fight") {
				if (fightitFragment == null) {
					fightitFragment = new FightItActivity ();

					transaction.Add (Resource.Id.fragmentContainer, fightitFragment, "fight");

				} else {

					transaction.Show (fightitFragment);

				}


			}
		}

		public void OnTabUnselected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
			if ((String)tab.Tag == "report") {
				if (reportFragment != null) {
				
					transaction.Hide (reportFragment);

					return;
				}

			} else if ((String)tab.Tag == "track") {
				if (_map != null) {
					_map.MyLocationEnabled = false;
					_map.Clear ();

					_map.MarkerClick -= MapOnMarkerClick;
				}

				transaction.Hide (_mapFragment);
			}
			if ((String)tab.Tag == "fight") {
				if (fightitFragment != null) {

					transaction.Hide (fightitFragment);

					return;
				}
			}

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
				fragTx.Add (Resource.Id.fragmentContainer, _mapFragment, "map");
				fragTx.Commit ();
				SupportFragmentManager.ExecutePendingTransactions(); 
			}
		}

		private void MapOnMarkerClick (object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			Marker marker = markerClickEventArgs.P0; // TODO [TO201212142221] Need to fix the name of this with MetaData.xml
			float zoomValue = 11;
			if (_map.CameraPosition.Zoom > zoomValue) {
				zoomValue = _map.CameraPosition.Zoom;
			}
			_map.AnimateCamera (CameraUpdateFactory.NewLatLngZoom(marker.Position,zoomValue));
			marker.ShowInfoWindow ();

		}

		protected override void OnPause ()
		{
			base.OnPause ();

			// Pause the GPS - we won't have to worry about showing the 
			// location.
			if (_map != null) {
				_map.MyLocationEnabled = false;
				_map.Clear ();

				_map.MarkerClick -= MapOnMarkerClick;
			}
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			SetupMapIfNeeded ();
			if (_map != null) {
				_map.MyLocationEnabled = true;

				// Setup a handler for when the user clicks on a marker.
				_map.MarkerClick += MapOnMarkerClick;
			}
		}


		public async void SetupMapIfNeeded ()
		{
			if (_map == null && _mapFragment != null) {
				_map = _mapFragment.Map;
			}
			if (_map != null) {

				LatLng myLocation = new LatLng (_gpsTracker.Latitude, _gpsTracker.Longitude);
				_crueltySpotsService = new CrueltySpotsService();


				_map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(myLocation, 11));

				List<String> categories = UserPreferencesHelper.GetFilterCategories ();
				if (!categories.Any()) {
					_crueltySpots = await _crueltySpotsService.GetManyAsync(new CrueltySpot(), true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);

				} else {
					_crueltySpots = await _crueltySpotsService.GetManyAsync(categories, true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);
				}

				RunOnUiThread(() =>
				              {
					MarkerOptions mapOption;
					LatLng crueltyLocation;
					LatLngBounds.Builder builder = new LatLngBounds.Builder();
					builder.Include(myLocation);
					foreach (CrueltySpot spot in _crueltySpots)
					{ // Loop through List with foreach
						if (spot.Name.CompareTo("Universoul Circus") == 0)
						{
							continue;
						}
						crueltyLocation = new LatLng(spot.Latitude, spot.Longitude);
						builder.Include(crueltyLocation);

						float defaultValue = 33;

						if (_pinColorLookup.ContainsKey(spot.CrueltySpotCategory.IconName.Replace(".png", "pin")))
						{
							defaultValue = _pinColorLookup[spot.CrueltySpotCategory.IconName.Replace(".png", "pin")];
						}
						mapOption = new MarkerOptions()
							.SetPosition(crueltyLocation)
								.SetSnippet(spot.Address)
								.SetTitle(spot.Name)
								.InvokeIcon(BitmapDescriptorFactory.DefaultMarker(defaultValue));

						Marker marker = _map.AddMarker(mapOption);

						_crueltyLookup.Add(marker.Id, spot);

					}


					// Move the map so that it is showing the markers we added above.
					_map.SetInfoWindowAdapter(new CustomInfoWindowAdapter(this));

					// Set listeners for marker events.  See the bottom of this class for their behavior.

					_map.SetOnInfoWindowClickListener(this);
				});
			}

		}

		public void OnInfoWindowClick (Marker marker)
		{
			if (_crueltyLookup.ContainsKey (marker.Id)) {
				CrueltySpot spot = _crueltyLookup [marker.Id];
				NavigateToCrueltySpotDetails (spot.ObjectId);
			}
		}

		private void NavigateToCrueltySpotDetails (string crueltySpotId)
		{
			var intent = new Intent (this, typeof(CrueltySpotActivity));
			intent.PutExtra (AppConstants.CrueltySpotIdKey, crueltySpotId);
			StartActivity (intent);
		}

		public override bool OnCreateOptionsMenu (ActionBar_Sherlock.View.IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);
			SupportMenuInflater.Inflate (Resource.Menu.report_menu, menu);
			return true;
		}

		public override bool OnOptionsItemSelected (ActionBar_Sherlock.View.IMenuItem item)
		{
			//This uses the imported MenuItem from ActionBarSherlock

			if (item.TitleFormatted.ToString () == "Settings") {
				var intent = new Intent(this,typeof(LegendActivity));
			
				StartActivity(intent);


			}
			return true;
		}


	}
}