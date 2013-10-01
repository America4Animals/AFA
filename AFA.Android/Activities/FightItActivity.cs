using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Adapters;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using AFA.Android.Helpers;
using AFA_Android.Helpers;
using AFA.Android.Utility;



namespace AFA.Android.Activities
{
	[Activity(Label = "Fight It")]
	public class FightItActivity : Activity
	{
		private ProgressDialog _loadingDialog;
		private ListView _crueltySpotsList;
		//private CrueltySpotDto _featuredCrueltySpot;
		//private CrueltySpot _featuredCrueltySpot;
		//private List<CrueltySpotDto> _otherCrueltySpots;
		//private List<CrueltySpot> _otherCrueltySpots;
		private List<CrueltySpot> _crueltySpots;
		private GPSTracker _gpsTracker;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.FightIt);
			_gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
			CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.FightIt);

			_loadingDialog = LoadingDialogManager.ShowLoadingDialog (this);

			_crueltySpotsList = FindViewById<ListView>(Resource.Id.CrueltySpots);

			_crueltySpotsList.ItemClick += (sender, e) =>
			{
				var crueltySpot = _crueltySpots[e.Position];
				NavigateToCrueltySpotDetails(crueltySpot.ObjectId);
			};
			setupSpots ();
			_loadingDialog.Dismiss();

		}

		private void NavigateToCrueltySpotDetails (string crueltySpotId)
		{
			var intent = new Intent (this, typeof(CrueltySpotActivity));
			intent.PutExtra (AppConstants.CrueltySpotIdKey, crueltySpotId);
			StartActivity (intent);
		}

		private async void setupSpots()
		{

			var crueltySpotsService = new CrueltySpotsService ();

			List<String> categories = UserPreferencesHelper.GetFilterCategories ();
			GeoQueryRequest geoQueryRequest = null;
			if (UserPreferencesHelper.getClosestSpotsFilter () == true) {
				geoQueryRequest = new GeoQueryRequest ();
				geoQueryRequest.Latitude = _gpsTracker.Latitude;
				geoQueryRequest.Longititude = _gpsTracker.Longitude;
				geoQueryRequest.DistanceInMiles = 50;
			}

			if (!categories.Any()) {
				if (geoQueryRequest != null) {
				
					_crueltySpots = await crueltySpotsService.GetManyAsync (geoQueryRequest, 
					                                                        true, 
					                                                        CrueltySpotSortField.CreatedAt,
					                                                        SortDirection.Asc);
				} 
				else {
					_crueltySpots = await crueltySpotsService.GetAllAsync (true);
				}


			} else {
				if (geoQueryRequest != null) {
					_crueltySpots = await crueltySpotsService.GetManyAsync (geoQueryRequest, categories, true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);
				} else {
					_crueltySpots = await crueltySpotsService.GetManyAsync (categories, true, CrueltySpotSortField.CreatedAt, SortDirection.Asc);
				}
			}

			if (_crueltySpots.Any ()) {
				RunOnUiThread (() => {

					_crueltySpotsList.Adapter = new CrueltySpotsAdapter (this, _crueltySpots);
					Toast.MakeText (this, "got into dismiss code", ToastLength.Short).Show ();
					_loadingDialog.Dismiss();

				});               
			} else {
				// ToDo: Handle case where there are no cruelty spots
			}         


		}

			

		protected override void OnResume ()
		{
			base.OnResume ();
			_loadingDialog = LoadingDialogManager.ShowLoadingDialog (this);
			setupSpots();

		}
		

	}
}