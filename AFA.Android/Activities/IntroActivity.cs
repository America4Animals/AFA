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
using AFA.Android.Utility;
using ContentPM = Android.Content.PM;
using SupportV4 = Android.Support.V4.App;


namespace AFA.Android.Activities
{
    [Activity(Label = "@string/ApplicationName", ConfigurationChanges = ContentPM.ConfigChanges.Orientation) ]
	public class IntroActivity : AfaBaseActivity, SherlockActionBar.ITabListener, GoogleMap.IOnInfoWindowClickListener
	{
		SupportV4.Fragment reportFragment;
		SupportV4.Fragment fightitFragment;
		SupportV4.Fragment homeFragment;
		private GoogleMap _map;
		private SupportMapFragment _mapFragment;
		private GPSTracker _gpsTracker;
		List<CrueltySpot> _crueltySpots;
		private CrueltySpotsService _crueltySpotsService;
		Dictionary<String, CrueltySpot> _crueltyLookup = new Dictionary<String, CrueltySpot>();
		private String toggleString = "map";
	    private string _currentTabTag;

		Dictionary<String,float> _pinColorLookup = new Dictionary<String,float> 
		{
			{"cariaggespin",128}, {"foiegraspin",56}, {"labspin",198}, {"morepin",327}, 
			{"petstorespin",209}, {"racespin",259}, {"rodeopin",33},{"sharkpin",302}, 
			{"furpin",68} ,{"performancepin",354}
		};

		Button listButton;

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
		    DebugHelper.WriteDebugEntry("In IntroActivity OnCreate()");
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Intro);
			_gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker (this);
			listButton = FindViewById<Button>(Resource.Id.ListButton);
			listButton.Click += (o, e) => {
				if ((String)(SupportActionBar.SelectedTab.Tag) == "track")
				{
					FragmentTransaction ft = SupportFragmentManager.BeginTransaction ();
					if (toggleString == "map")
					{
						toggleString = "list";
						// first hide the map
						if (_map != null) {
							_map.MyLocationEnabled = false;
							_map.Clear ();

							_map.MarkerClick -= MapOnMarkerClick;
						}

						ft.Hide (_mapFragment);

						// now create the fight it tab
						if (fightitFragment == null) {
							fightitFragment = new FightItActivity ();

							ft.Add (Resource.Id.fragmentContainer, fightitFragment, "fight");

						} else {

							ft.Show (fightitFragment);

						}


					}
					else{
						toggleString = "map";
						if (fightitFragment != null) {

							ft.Hide (fightitFragment);
						}
						InitMapFragment ();

						ft.Show (_mapFragment);
						SetupMapIfNeeded ();

					}
					ft.Commit();
					//	Toast.MakeText (this, toggleString, ToastLength.Short).Show ();
			
				}
			};

			SupportActionBar.NavigationMode = SherlockActionBar.NavigationModeTabs;

			ActionBar.Tab homeTab = SupportActionBar.NewTab ();
			homeTab.SetText ("Home");
			homeTab.SetTabListener (this);
			homeTab.SetTag ("home");
		
			SupportActionBar.AddTab (homeTab);


			ActionBar.Tab trackItTab = SupportActionBar.NewTab ();

			trackItTab.SetText ("Track it");
			trackItTab.SetTabListener (this);
			trackItTab.SetTag ("track");
			SupportActionBar.AddTab (trackItTab);


			ActionBar.Tab reportItTab = SupportActionBar.NewTab ();
			reportItTab.SetText ("Report It");
			reportItTab.SetTabListener (this);
			reportItTab.SetTag ("report");
			SupportActionBar.AddTab (reportItTab);


            string tabFromIntent = Intent.GetStringExtra("tab") ?? null;
            if (tabFromIntent != null)
            {
                _currentTabTag = tabFromIntent;
            }
		    else if(bundle != null)
		    {
                _currentTabTag = bundle.GetString("tab") ?? "";
		    }
		    else
		    {
		        _currentTabTag = "";
		    }


            DebugHelper.WriteDebugEntry("Selecting Tab");
            switch (_currentTabTag.ToLower())
		    {
                case "home":
                    SupportActionBar.SelectTab(homeTab);
		            break;
                case "report":
                    SupportActionBar.SelectTab(reportItTab);
		            break;
                default:
                    SupportActionBar.SelectTab(trackItTab);
		            break;

		    }
		
		}

		public void OnTabReselected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{


		}

		public void OnTabSelected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
            DebugHelper.WriteDebugEntry("In IntroActivity OnTabSelected(): " + tab.Tag);
		    _currentTabTag = (String)tab.Tag;

			if ((String)tab.Tag == "home") {
				if (homeFragment == null) {
					homeFragment = new HomeFragment ();

					transaction.Add (Resource.Id.fragmentContainer, homeFragment, "home");

				} else {

					transaction.Show (homeFragment);
		
				}
				if (listButton != null) {
					listButton.Visibility = ViewStates.Invisible;
				}
			} else if ((String)tab.Tag == "track") {
				InitMapFragment ();

				transaction.Show (_mapFragment);
				SetupMapIfNeeded ();
				if (listButton != null) {
					listButton.Visibility = ViewStates.Visible;
				}
			} else if ((String)tab.Tag == "report") {


				if (reportFragment == null) {
					reportFragment = new ReportCrueltyFragment ();

					transaction.Add (Resource.Id.fragmentContainer, reportFragment, "report");

				} else {

					transaction.Show (reportFragment);

				}
				if (listButton != null) {
					listButton.Visibility = ViewStates.Invisible;
				}


			}
		}

		public void OnTabUnselected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
            DebugHelper.WriteDebugEntry("In IntroActivity OnTabUnselected()");

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

				if (toggleString == "map") {
					transaction.Hide (_mapFragment);
				} else {
					transaction.Hide (fightitFragment);
				}
			}
			if ((String)tab.Tag == "home") {
				if (homeFragment != null) {

					transaction.Hide (homeFragment);

					return;
				}
			}

		}

	    protected override void OnSaveInstanceState(Bundle outState)
	    {
	        base.OnSaveInstanceState(outState);

            outState.PutString("tab", _currentTabTag);
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
            DebugHelper.WriteDebugEntry("In IntroActivity OnPause()");
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
            DebugHelper.WriteDebugEntry("In IntroActivity OnResume()");

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

				DebugHelper.WriteDebugEntry ("Fetched categories for mapping (" + categories.Count + ")");

				if (!categories.Any()) {
					_crueltySpots = await _crueltySpotsService.GetManyAsync(new CrueltySpot(), true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);

				} else {
					_crueltySpots = await _crueltySpotsService.GetManyAsync(categories, true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);
				}

				DebugHelper.WriteDebugEntry ("Fetched CrueltySPots for mapping");

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
				return true;
			}

			return base.OnOptionsItemSelected (item);

		}


	}
}