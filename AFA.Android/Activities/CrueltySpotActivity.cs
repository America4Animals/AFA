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
            var tv = FindViewById<TextView>(Resource.Id.textView1);
            //_crueltySpot = AfaApplication.ServiceClient.Get(new CrueltySpotDto { Id = crueltySpotId }).CrueltySpot;
            var crueltySpotsService = new CrueltySpotsService();

            crueltySpotsService.GetByIdAsync<CrueltySpotResponse>(crueltySpotId, r =>
                RunOnUiThread(() =>
                    {
                        _crueltySpot = r.CrueltySpot;
                        tv.Text = String.Format("DETAILS about {0}", _crueltySpot.Name);

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