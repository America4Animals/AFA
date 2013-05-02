using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
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
    [Activity(Label = "My Activity")]
    public class AddPlaceActivity : Activity
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
            var address = addresses.FirstOrDefault();
            if (address != null)
            {
                var streetAddress = address.GetAddressLine(0);
                //var city = address.SubAdminArea; // RETURNS NOTHING
                //var city = address.Locality; // RETURNS NOTHING
                var city = address.SubLocality; // e.g. Brooklyn
                var state = address.AdminArea; // e.g. New York
                var zip = address.PostalCode;

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

            _submitButton.Click += (sender, args) =>
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
                                               var city = _cityInput.Text;
                                               var stateInput = _stateInput.Text;
                                               var stateAbbreviation = stateInput.Length == 2 ? stateInput : StateNamesAndAbbreviations.StateAbbreviationLookup[stateInput];
                                               var response =
                                                   AfaApplication.ServiceClient.Get(new CrueltySpotsDto()
                                                                                        {
                                                                                            Name = name,
                                                                                            City = city,
                                                                                            StateProvinceAbbreviation
                                                                                                =
                                                                                                stateAbbreviation
                                                                                        });

                                               if (response.CrueltySpots.Any())
                                               {
                                                   var existingPlaceId = response.CrueltySpots.First().Id;

                                                   new AlertDialog.Builder(this)
                                                    .SetTitle("Cruelty already reported here")
                                                    .SetMessage("The location you have chosen is already on the cruelty map.")
                                                    .SetNegativeButton("Cancel", (o, args1) => { })
                                                    .SetPositiveButton("Edit Info", (o, args1) =>
                                                    {
                                                        var intent = new Intent(this, typeof(CrueltySpotActivity));
                                                        intent.PutExtra(AppConstants.CrueltySpotIdKey, existingPlaceId);
                                                        StartActivity(intent);
                                                    })
                                                    .Show();
                                               }
                                               else
                                               {
                                                   
                                               }


                                               
                                           }
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