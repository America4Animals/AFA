using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
using AFA.Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Text.Style;
using System.Drawing;

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
            var crueltyTypeInput = FindViewById<EditText>(Resource.Id.TypeOfCrueltyInput);

            var placeName = Intent.GetStringExtra(AppConstants.PlaceNameKey);
            if (!string.IsNullOrEmpty(placeName))
            {
                var placeVicinity = Intent.GetStringExtra(AppConstants.PlaceVicinityKey);
                var locationText = new StringBuilder();

                locationText.Append("<font color='#FAA3DA'><b>");
                locationText.Append(placeName);
                locationText.Append("</b></font>");
                locationText.Append("<br />");
                locationText.Append("<font color='#B5B5B5'><small>");
                locationText.Append(placeVicinity);
                locationText.Append("</small></font>");
                locationInput.SetText(Html.FromHtml(locationText.ToString()), TextView.BufferType.Spannable);
            }

            var crueltyType = Intent.GetStringExtra(AppConstants.CrueltySpotCategoryNameKey);
            if (!string.IsNullOrEmpty(crueltyType))
            {
                crueltyTypeInput.Text = crueltyType;
            }

            locationInput.FocusChange += (sender, args) =>
                                             {
                                                 if (args.HasFocus)
                                                 {
                                                     var intent = new Intent(this, typeof (NearbyPlacesActivity));
                                                     StartActivity(intent);
                                                 }
                                             };

            crueltyTypeInput.FocusChange += (sender, args) =>
            {
                if (args.HasFocus)
                {
                    var intent = new Intent(this, typeof(CrueltyTypesActivity));
                    StartActivity(intent);
                }
            };
        }
    }
}