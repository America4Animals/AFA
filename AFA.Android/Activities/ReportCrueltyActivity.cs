using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using AFA.Android.Utility;
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
using Android.Views.InputMethods;
using ServiceStack.Text;
using Parse;

namespace AFA.Android
{
	[Activity(Label = "Report Cruelty", MainLauncher = false, Icon = "@drawable/icon")]
	public class ReportCrueltyActivity : ReportCrueltyBaseActivity
	{
		private EditText _locationInput;
		private EditText _crueltyTypeInput;
		private EditText _descriptionInput;
		private Button _submitButton;
		private ProgressDialog _loadingDialog;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ReportCruelty);

			CrueltyNavMenuHelper.InitCrueltyNavMenu (this, CrueltyNavMenuItem.ReportIt);

			_locationInput = FindViewById<EditText> (Resource.Id.LocationInput);
			_crueltyTypeInput = FindViewById<EditText> (Resource.Id.TypeOfCrueltyInput);

			if (_crueltyReport.PlaceSpecified) {
				var locationText = new StringBuilder ();
				locationText.Append ("<font color='#D34C96'><b>");
				locationText.Append (_crueltyReport.PlaceName);
				locationText.Append ("</b></font>");
				locationText.Append ("<br />");
				locationText.Append ("<font color='#B5B5B5'><small>");
				locationText.Append (_crueltyReport.PlaceFormattedShortAddress);
				locationText.Append ("</small></font>");
				var htmlText = locationText.ToString ();
				_locationInput.SetText (Html.FromHtml (htmlText), TextView.BufferType.Spannable);
			}

			_crueltyTypeInput.Text = _crueltyReport.CrueltyType != null ? _crueltyReport.CrueltyType.Name : null;

			_locationInput.FocusChange += (sender, args) =>
			{
				if (args.HasFocus) {
					var intent = new Intent (this, typeof(NearbyPlacesActivity));
					StartActivity (intent);
				}
			};

			_crueltyTypeInput.FocusChange += (sender, args) =>
			{
				if (args.HasFocus) {
					var intent = new Intent (this, typeof(CrueltyTypesActivity));
					StartActivity (intent);
				}
			};

			// ToDo: This line hides the soft keyboard, but doing so makes the EditText only have 1 line.
			// Look into alternative
			//_locationInput.InputType = InputTypes.Null;
			_crueltyTypeInput.InputType = InputTypes.Null;

			_submitButton = FindViewById<Button> (Resource.Id.SubmitButton);
			_submitButton.Enabled = _crueltyReport.PlaceSpecified && _crueltyReport.CrueltyTypeSpecified;

			_submitButton.Click +=
                (sender, args) =>
			{
				// Create a new Cruelty Spot
				_loadingDialog = DialogManager.ShowLoadingDialog (this);
				//CrueltySpotDto newCrueltySpot;
				CrueltySpot newCrueltySpot;

				if (_crueltyReport.IsGooglePlace ()) {
					// Fetch details from google
					var googlePlaces = new GooglePlacesApi.GooglePlaces ();
					googlePlaces.GetDetails (_crueltyReport.GooglePlace.Reference, response =>
					{
						// ToDo: Check Status and handle non-OK
						var placeDetails = response.result;

						var latitude = placeDetails.geometry.location.lat;
						var longitude = placeDetails.geometry.location.lng;
						newCrueltySpot = new CrueltySpot {
							Name = placeDetails.name,
							//Description = _descriptionInput.Text,
							Address = placeDetails.Address,
							City = placeDetails.City,
							StateProvinceAbbreviation = placeDetails.StateOrProvince,
							Zipcode = placeDetails.PostalCode,
							PhoneNumber = placeDetails.formatted_phone_number,
							WebpageUrl = placeDetails.website,
							//Latitude = placeDetails.geometry.location.lat,
							//Longitude = placeDetails.geometry.location.lng,
							Location = new ParseGeoPoint(latitude, longitude),
							//CrueltySpotCategoryId = _crueltyReport.CrueltyType.Id,
							CrueltySpotCategory = ParseObject.CreateWithoutData<CrueltySpotCategory>(_crueltyReport.CrueltyType.Id),
							GooglePlaceId = placeDetails.id
						};

						SubmitNewCrueltySpot (newCrueltySpot);
                                
					});
				} else {
					// Submit with user lat/lng
					var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker (this);

					var latitude = gpsTracker.Latitude;
					var longitude = gpsTracker.Longitude;

					newCrueltySpot = new CrueltySpot {
						Name = _crueltyReport.PlaceName,
						//Description = _descriptionInput.Text,
						Address = _crueltyReport.UserGeneratedPlace.Address,
						City = _crueltyReport.UserGeneratedPlace.City,
						StateProvinceAbbreviation = _crueltyReport.UserGeneratedPlace.StateProvinceAbbreviation,
						Zipcode = _crueltyReport.UserGeneratedPlace.Zipcode,
						//PhoneNumber = placeDetails.formatted_phone_number,
						//WebpageUrl = placeDetails.website,
						Email = _crueltyReport.UserGeneratedPlace.Email,
						//Latitude = gpsTracker.Latitude,
						//Longitude = gpsTracker.Longitude,
						Location = new ParseGeoPoint(latitude, longitude),
						//CrueltySpotCategoryId = _crueltyReport.CrueltyType.Id
						CrueltySpotCategory = ParseObject.CreateWithoutData<CrueltySpotCategory>(_crueltyReport.CrueltyType.Id)
					};

					SubmitNewCrueltySpot (newCrueltySpot);
				}

                    
			};

		}

		private async void SubmitNewCrueltySpot (CrueltySpot newCrueltySpot)
		{
			var crueltySpotsService = new CrueltySpotsService ();        
			var newCrueltySpotId = await crueltySpotsService.SaveAsync (newCrueltySpot);

			RunOnUiThread (() =>
			{
				var intent = new Intent (this, typeof(CrueltySpotActivity));
				intent.PutExtra (AppConstants.ShowCrueltySpotAddedSuccessfullyKey, true);
				intent.PutExtra (AppConstants.CrueltySpotIdKey, newCrueltySpotId);
				ClearAllUi ();
				_crueltyReport.ClearAll ();
				CommitCrueltyReport ();
				StartActivity (intent);
				_loadingDialog.Dismiss ();
			});
		}

		private void ClearAllUi ()
		{
			_locationInput.Text = "";
			_crueltyTypeInput.Text = "";
			//_descriptionInput.Text = "";
		}
	}
}