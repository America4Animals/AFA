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
using AFA_Android.Helpers;
using AFA.Android.Utility;
using ActionBar_Sherlock.App;
using ActionBar_Sherlock.View;
using SherlockActionBar = ActionBar_Sherlock.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Android.Support.V4.App;


namespace AFA.Android.Activities
{
	[Activity(Label = "Fight It")]
	public class FightItActivity : Fragment
	{
		private ProgressDialog _loadingDialog;
		private ListView _crueltySpotsList;
		//private CrueltySpotDto _featuredCrueltySpot;
		//private CrueltySpot _featuredCrueltySpot;
		//private List<CrueltySpotDto> _otherCrueltySpots;
		//private List<CrueltySpot> _otherCrueltySpots;
		private List<CrueltySpot> _crueltySpots;
		private GPSTracker _gpsTracker;
		private RelativeLayout ll;
		private FragmentActivity fa;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			fa = base.Activity;
			ll = (RelativeLayout) inflater.Inflate(Resource.Layout.FightIt, container, false);
		

			_gpsTracker = ((AfaApplication)fa.ApplicationContext).GetGpsTracker(fa);
		
			_loadingDialog = LoadingDialogManager.ShowLoadingDialog (fa);

			_crueltySpotsList = ll.FindViewById<ListView>(Resource.Id.CrueltySpots);

			_crueltySpotsList.ItemClick += (sender, e) =>
			{
				var crueltySpot = _crueltySpots[e.Position];
				NavigateToCrueltySpotDetails(crueltySpot.ObjectId);
			};
			setupSpots ();
			_loadingDialog.Dismiss();
			return ll;

		}

		private void NavigateToCrueltySpotDetails (string crueltySpotId)
		{
			var intent = new Intent (fa, typeof(CrueltySpotActivity));
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
				fa.RunOnUiThread (() => {

					_crueltySpotsList.Adapter = new CrueltySpotsAdapter (fa, _crueltySpots);
					//Toast.MakeText (fa, "got into dismiss code", ToastLength.Short).Show ();
					_loadingDialog.Dismiss();

				});               
			} else {
				// ToDo: Handle case where there are no cruelty spots
			}         


		}

			

		public override void OnResume ()
		{
			base.OnResume ();
			_loadingDialog = LoadingDialogManager.ShowLoadingDialog (fa);
			setupSpots();

		}
		

	}
}