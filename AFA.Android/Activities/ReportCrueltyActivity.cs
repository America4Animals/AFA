using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android
{
    [Activity(Label = "Report Cruelty", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReportCrueltyActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.ReportCruelty);

            var locationInput = FindViewById<EditText>(Resource.Id.LocationInput);
            locationInput.FocusChange += (sender, args) =>
                                             {
                                                 if (args.HasFocus)
                                                 {
                                                     var intent = new Intent(this, typeof (NearbyPlacesActivity));
                                                     StartActivity(intent);
                                                 }
                                             };
        }
    }
}