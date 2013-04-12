using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
using AFA.Android.Helpers;
using AFA.ServiceModel.DTOs;
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
        private bool _locationSpecified;
        private bool _crueltyTypeSpecified;

        // From google, use to retrieve details about a place
        private string _reference;
        private int _crueltySpotCategoryId;

        private Button _submitButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ReportCruelty);

            var locationInput = FindViewById<EditText>(Resource.Id.LocationInput);
            var crueltyTypeInput = FindViewById<EditText>(Resource.Id.TypeOfCrueltyInput);

            var placeName = Intent.GetStringExtra(AppConstants.PlaceNameKey);
            _locationSpecified = !String.IsNullOrEmpty(placeName);
            if (_locationSpecified)
            {
                var placeVicinity = Intent.GetStringExtra(AppConstants.PlaceVicinityKey);
                _reference = Intent.GetStringExtra(AppConstants.PlaceReferenceKey);

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
            _crueltyTypeSpecified = !String.IsNullOrEmpty(crueltyType);
            if (_crueltyTypeSpecified)
            {
                crueltyTypeInput.Text = crueltyType;
                _crueltySpotCategoryId = Intent.GetIntExtra(AppConstants.CrueltySpotCategoryIdKey, 0);
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

            _submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            _submitButton.Enabled = _locationSpecified && _crueltyTypeSpecified;

            _submitButton.Click += (sender, args) =>
                                      {
                                          var loadingDialog = ProgressDialog.Show(this, "Submitting", "Please wait...", true);
                                          var descriptionInput = FindViewById<EditText>(Resource.Id.DetailsInput);
                                          var googlePlaces = new GooglePlacesApi.GooglePlaces();
                                          googlePlaces.GetDetails(_reference, response =>
                                                                                  {
                                                                                      // ToDo: Check Status and handle non-OK
                                                                                      var placeDetails = response.result;
                                                                                      var crueltySpotDto = new CrueltySpotDto
                                                                                                               {
                                                                                                                   Name = placeDetails.name,
                                                                                                                   Description = descriptionInput.Text,
                                                                                                                   Address = placeDetails.Address,
                                                                                                                   City = placeDetails.City,
                                                                                                                   StateProvinceAbbreviation = placeDetails.StateOrProvince,
                                                                                                                   Zipcode = placeDetails.PostalCode,
                                                                                                                   PhoneNumber = placeDetails.formatted_phone_number,
                                                                                                                   WebpageUrl = placeDetails.website,
                                                                                                                   Latitude = placeDetails.geometry.location.lat,
                                                                                                                   Longitude = placeDetails.geometry.location.lng,
                                                                                                                   CrueltySpotCategoryId = _crueltySpotCategoryId
                                                                                                               };

                                                                                      AfaApplication.ServiceClient.PostAsync(crueltySpotDto,
                                                                                           r => RunOnUiThread(() =>
                                                                                           {
                                                                                               locationInput.Text = "";
                                                                                               crueltyTypeInput.Text = "";
                                                                                               descriptionInput.Text = "";
                                                                                               loadingDialog.Hide();
                                                                                               Toast.MakeText(this, "Cruelty Spot Added", ToastLength.Short).Show();
                                                                                           }),
                                                                                           (r, ex) => RunOnUiThread(() =>
                                                                                           {
                                                                                               throw ex;
                                                                                           }));
                                                                                  });                                         
                                      };
        }

    }

    
}