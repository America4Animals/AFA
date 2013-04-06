using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
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

            var placeName = Intent.GetStringExtra("placeName");
            if (!string.IsNullOrEmpty(placeName))
            {
                var placeVicinity = Intent.GetStringExtra("placeVicinity");
                var locationText = new StringBuilder();
                //locationText.Append(placeName);
                //locationText.Append("\n");
                //locationText.Append(placeVicinity);
                //locationInput.Text = locationText.ToString();

                //var ssb = new SpannableStringBuilder(locationText.ToString());
                //var pinkColorSpan = new ForegroundColorSpan()

                locationText.Append("<font color='#FAA3DA'><b>");
                locationText.Append(placeName);
                locationText.Append("</b></font>");
                locationText.Append("<br />");
                locationText.Append("<font color='#B5B5B5'><small>");
                locationText.Append(placeVicinity);
                locationText.Append("</small></font>");
                locationInput.SetText(Html.FromHtml(locationText.ToString()), TextView.BufferType.Spannable);
            }
            
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