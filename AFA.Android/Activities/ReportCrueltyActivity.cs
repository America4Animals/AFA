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
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text.Style;
using System.Drawing;

namespace AFA.Android
{
    [Activity(Label = "Report Cruelty", MainLauncher = true, Icon = "@drawable/icon")]
    public class ReportCrueltyActivity : Activity
    {
        private EditText _locationInput;
        private EditText _crueltyTypeInput;
        private Button _submitButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ReportCruelty);

            _locationInput = FindViewById<EditText>(Resource.Id.LocationInput);
            _crueltyTypeInput = FindViewById<EditText>(Resource.Id.TypeOfCrueltyInput);

            if (!ReportCruelty.LocationSpecified)
            {
                ReportCruelty.PlaceName = Intent.GetStringExtra(AppConstants.PlaceNameKey);
                ReportCruelty.PlaceVicinity = Intent.GetStringExtra(AppConstants.PlaceVicinityKey);
                ReportCruelty.Reference = Intent.GetStringExtra(AppConstants.PlaceReferenceKey);

                if (ReportCruelty.LocationSpecified)
                {
                    var locationText = new StringBuilder();
                    locationText.Append("<font color='#FAA3DA'><b>");
                    locationText.Append(ReportCruelty.PlaceName);
                    locationText.Append("</b></font>");
                    locationText.Append("<br />");
                    locationText.Append("<font color='#B5B5B5'><small>");
                    locationText.Append(ReportCruelty.PlaceVicinity);
                    locationText.Append("</small></font>");
                    ReportCruelty.PlaceText = locationText.ToString();
                }
            }

            if (!String.IsNullOrEmpty(ReportCruelty.PlaceText))
            {
                _locationInput.SetText(Html.FromHtml(ReportCruelty.PlaceText), TextView.BufferType.Spannable);     
            }

            if (!ReportCruelty.CrueltyTypeSpecified)
            {
                ReportCruelty.CrueltyTypeName = Intent.GetStringExtra(AppConstants.CrueltySpotCategoryNameKey);
                ReportCruelty.CrueltyTypeId = Intent.GetIntExtra(AppConstants.CrueltySpotCategoryIdKey, 0);
            }
                      
            _crueltyTypeInput.Text = ReportCruelty.CrueltyTypeName;

            _locationInput.FocusChange += (sender, args) =>
                                            {
                                                if (args.HasFocus)
                                                {
                                                    var intent = new Intent(this, typeof (NearbyPlacesActivity));
                                                    StartActivity(intent);
                                                }
                                            };

            _crueltyTypeInput.FocusChange += (sender, args) =>
            {
                if (args.HasFocus)
                {
                    var intent = new Intent(this, typeof(CrueltyTypesActivity));
                    StartActivity(intent);
                }
            };

            //Log.Debug("LocationSpecified", _locationSpecified.ToString());
            //Log.Debug("CrueltyTypeSpecified", _crueltyTypeSpecified.ToString());

            _submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            _submitButton.Enabled = ReportCruelty.LocationSpecified && ReportCruelty.CrueltyTypeSpecified;

            _submitButton.Click += (sender, args) =>
                                      {
                                          var loadingDialog = ProgressDialog.Show(this, "Submitting", "Please wait...", true);
                                          var descriptionInput = FindViewById<EditText>(Resource.Id.DetailsInput);
                                          var googlePlaces = new GooglePlacesApi.GooglePlaces();
                                          googlePlaces.GetDetails(ReportCruelty.Reference, response =>
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
                                                                                                                   CrueltySpotCategoryId = ReportCruelty.CrueltyTypeId
                                                                                                               };

                                                                                      AfaApplication.ServiceClient.PostAsync(crueltySpotDto,
                                                                                           r => RunOnUiThread(() =>
                                                                                           {
                                                                                               _locationInput.Text = "";
                                                                                               _crueltyTypeInput.Text = "";
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