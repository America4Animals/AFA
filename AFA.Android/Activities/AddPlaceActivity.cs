using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Java.Util;

namespace AFA.Android.Activities
{
    [Activity(Label = "Add a New Place")]
    public class AddPlaceActivity : ReportCrueltyBaseActivity
    {
        private EditText _placeNameInput;
        private EditText _addressInput;
        private EditText _cityInput;
        private EditText _stateInput;
        private EditText _zipInput;
        private Button _submitButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AddPlace);

            _placeNameInput = FindViewById<EditText>(Resource.Id.PlaceName);
            _addressInput = FindViewById<EditText>(Resource.Id.StreetAddress);
            _cityInput = FindViewById<EditText>(Resource.Id.City);
            _stateInput = FindViewById<EditText>(Resource.Id.State);
            _zipInput = FindViewById<EditText>(Resource.Id.Zip);
            _submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            _submitButton.Enabled = false;

            var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);

            var geocoder = new Geocoder(this, Locale.Default);
            var addresses = geocoder.GetFromLocation(gpsTracker.Latitude, gpsTracker.Longitude, 1);
            var myAddress = addresses.FirstOrDefault();
            if (myAddress != null)
            {
                var streetAddress = myAddress.GetAddressLine(0);
                //var city = address.SubAdminArea; // RETURNS NOTHING
                //var city = address.Locality; // RETURNS NOTHING
                //var city = myAddress.SubLocality; // e.g. Brooklyn

                // Try to get a city
                string city = myAddress.SubLocality;
                if (String.IsNullOrEmpty(city))
                {
                    city = myAddress.Locality;
                }

                var state = myAddress.AdminArea; // e.g. New York
                var zip = myAddress.PostalCode;

                if (!String.IsNullOrEmpty(streetAddress))
                {
                    _addressInput.Text = streetAddress;
                }

                if (!String.IsNullOrEmpty(city))
                {
                    _cityInput.Text = city;
                }

                if (!String.IsNullOrEmpty(state))
                {
                    _stateInput.Text = state;
                }

                if (!String.IsNullOrEmpty(zip))
                {
                    _zipInput.Text = zip;
                }
            }

            _placeNameInput.AfterTextChanged += (sender, args) => ValidateInput();
            _cityInput.AfterTextChanged += (sender, args) => ValidateInput();
            _stateInput.AfterTextChanged += (sender, args) => ValidateInput();

            _submitButton.Click += async delegate(object sender, EventArgs args)
                {
                    var validInput = ValidateInput();
                    if (!validInput)
                    {
                        DialogManager.ShowAlertDialog(this, "Incomplete Submission",
                                                           "Please supply a Name, City and State",
                                                           false);
                    }
                    else
                    {
                        var loadingDialog = DialogManager.ShowLoadingDialog(this);
                        var name = _placeNameInput.Text;
                        var address = _addressInput.Text;
                        var city = _cityInput.Text;
                        var stateInput = _stateInput.Text;
                        var zipInput = _zipInput.Text;
                        var stateAbbreviation = stateInput.Length == 2 ? stateInput : StateNamesAndAbbreviations.StateAbbreviationLookup[stateInput];

                        var crueltySpotsService = new CrueltySpotsService();
                        var crueltySpots = await crueltySpotsService.GetMany(new CrueltySpot
                                                                           {
                                                                               Name = name,
                                                                               City = city,
                                                                               StateProvinceAbbreviation
                                                                                   =
                                                                                   stateAbbreviation
                                                                           }, false);

                        RunOnUiThread(()
                            =>
                            {
                                if (crueltySpots.Any())
                                {
                                    //var existingPlaceId = crueltySpots.First().Id;
                                    var existingPlaceId = crueltySpots.First().ObjectId;

                                    new AlertDialog.Builder(this)
                                        .SetTitle("Cruelty already reported here")
                                        .SetMessage("The location you have chosen is already on the cruelty map.")
                                        .SetNegativeButton("Cancel", (o, args1) => { })
                                        .SetPositiveButton("Edit Info", (o, args1) =>
                                            {
                                                var intent = new Intent(this,typeof(CrueltySpotActivity));
                                                intent.PutExtra(AppConstants.CrueltySpotIdKey, existingPlaceId);
                                                StartActivity(intent);
                                            })
                                        .Show();
                                }
                                else
                                {
                                    _crueltyReport.UserGeneratedPlace = new UserGeneratedPlace
                                                                            {
                                                                                Name = name,
                                                                                Address = address,
                                                                                City = city,
                                                                                StateProvinceAbbreviation = stateAbbreviation,
                                                                                Zipcode = zipInput,
                                                                                Email = FindViewById<EditText>(Resource.Id.Email).Text
                                                                            };

                                    CommitCrueltyReport();
                                    var intent = new Intent(this, typeof(ReportCrueltyActivity));
                                    StartActivity(intent);
                                }
                            });

                    };
                };
        }

        private bool ValidateInput()
        {
            if (_placeNameInput.Text.Length > 0 && _cityInput.Text.Length > 0 && _stateInput.Text.Length > 0)
            {
                _submitButton.Enabled = true;
                return true;
            }
            else
            {
                _submitButton.Enabled = false;
                return false;
            }
        }
    }
}