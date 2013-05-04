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
        private CrueltySpotDto _crueltySpot;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltySpot);

            var crueltySpotId = Intent.GetIntExtra(AppConstants.CrueltySpotIdKey, 0);
            Log.Debug("CrueltySpotDetails", crueltySpotId.ToString());
            var tv = FindViewById<TextView>(Resource.Id.textView1);
            //_crueltySpot = AfaApplication.ServiceClient.Get(new CrueltySpotDto { Id = crueltySpotId }).CrueltySpot;
            _crueltySpot = CrueltySpotService.GetById(crueltySpotId);
            tv.Text = String.Format("DETAILS about {0}", _crueltySpot.Name);

            var showSuccessAddedAlert = Intent.GetBooleanExtra(AppConstants.ShowCrueltySpotAddedSuccessfullyKey, false);
            if (showSuccessAddedAlert)
            {
                DialogManager.ShowAlertDialog(this, "Thanks!", "...for reporting animal cruelty here. This location is now on the map so others can take action!", true);
            }
        }
    }
}