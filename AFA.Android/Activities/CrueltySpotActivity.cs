using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Service;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace AFA.Android.Activities
{
    [Activity(Label = "Cruelty Spot")]
    public class CrueltySpotActivity : Activity
    {
        private ProgressDialog _loadingDialog;
        private CrueltySpotDto _crueltySpot;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltySpot);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            var crueltySpotId = Intent.GetIntExtra(AppConstants.CrueltySpotIdKey, 0);
            //_crueltySpot = AfaApplication.ServiceClient.Get(new CrueltySpotDto { Id = crueltySpotId }).CrueltySpot;
            var crueltySpotsService = new CrueltySpotsService();

            crueltySpotsService.GetByIdAsync<CrueltySpotResponse>(crueltySpotId, r =>
                RunOnUiThread(() =>
                    {
                        _crueltySpot = r.CrueltySpot;
                        var formattedAddress = String.Format("{0}\n{1}", _crueltySpot.Address, _crueltySpot.CityStateAndZip);
                        FindViewById<TextView>(Resource.Id.Name).Text = _crueltySpot.Name;
                        FindViewById<TextView>(Resource.Id.Address).Text = formattedAddress;
                        var resourceId = Resources.GetIdentifier(_crueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", PackageName);
                        FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);

                        _loadingDialog.Hide();

                        var showSuccessAddedAlert = Intent.GetBooleanExtra(AppConstants.ShowCrueltySpotAddedSuccessfullyKey, false);
                        if (showSuccessAddedAlert)
                        {
                            DialogManager.ShowAlertDialog(this, "Thanks!", "...for reporting animal cruelty here. This location is now on the map so others can take action!", true);
                        }
                    }));
        }
    }
}