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


        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FightIt);
            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.FightIt);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            _crueltySpotsList = FindViewById<ListView>(Resource.Id.CrueltySpots);

            _crueltySpotsList.ItemClick += (sender, e) =>
            {
                var crueltySpot = _crueltySpots[e.Position];
                NavigateToCrueltySpotDetails(crueltySpot.ObjectId);
            };

            var crueltySpotsService = new CrueltySpotsService();

			ISharedPreferences sp = Application.Context.GetSharedPreferences(PackageName, FileCreationMode.Private);
			var allCrueltySpots = new List<CrueltySpot> ();
			var categoryList = sp.GetString ("categories", null);
			if (categoryList == null) {

				allCrueltySpots = await crueltySpotsService.GetAllAsync (true);
		    
			} else {
				List<String> categoryIds = categoryList.Split (':').ToList<String> ();
				if (categoryIds.Count () > 0) {
					allCrueltySpots = await crueltySpotsService.GetManyAsync (categoryIds, true);

				} else {
					allCrueltySpots = await crueltySpotsService.GetAllAsync (true);
				}
			}

            if (allCrueltySpots.Any())
            {
				RunOnUiThread (() => {
					_crueltySpots = allCrueltySpots;
					_crueltySpotsList.Adapter = new CrueltySpotsAdapter(this, _crueltySpots);
					_loadingDialog.Hide();
				});               
            }
            else
            {
                // ToDo: Handle case where there are no cruelty spots
            }         
        }

        private void NavigateToCrueltySpotDetails(string crueltySpotId)
        {
            var intent = new Intent(this, typeof(CrueltySpotActivity));
            intent.PutExtra(AppConstants.CrueltySpotIdKey, crueltySpotId);
            StartActivity(intent);
        }
    }
}