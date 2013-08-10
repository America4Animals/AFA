using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Adapters;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Activities
{
    [Activity(Label = "Fight It")]
    public class FightItActivity : Activity
    {
        private ProgressDialog _loadingDialog;
        private ListView _crueltySpotsList;
        //private CrueltySpotDto _featuredCrueltySpot;
        //private CrueltySpot _featuredCrueltySpot;
        //private List<CrueltySpotDto> _otherCrueltySpots;
        //private List<CrueltySpot> _otherCrueltySpots;
        private List<CrueltySpot> _crueltySpots; 

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FightIt);
            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.FightIt);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            _crueltySpotsList = FindViewById<ListView>(Resource.Id.CrueltySpots);

            //FindViewById<LinearLayout>(Resource.Id.linearLayoutFeatured).Click += (sender, e) => NavigateToCrueltySpotDetails(_featuredCrueltySpot.Id);

            _crueltySpotsList.ItemClick += (sender, e) =>
            {
                var crueltySpot = _crueltySpots[e.Position];
                //NavigateToCrueltySpotDetails(crueltySpot.Id);
                NavigateToCrueltySpotDetails(crueltySpot.ObjectId);
            };
            
            //var crueltySpotsService = new CrueltySpotsService();
            //crueltySpotsService.GetManyAsync<CrueltySpotsResponse>(new CrueltySpotsDto
            //                                     {
            //                                         SortBy = "createdAt",
            //                                         SortOrder = "desc"
            //                                     }, r => RunOnUiThread(() 
            //                                         =>
            //                                         {
            //                                             var allCrueltySpots = r.CrueltySpots;      
                                                         
            //                                             if (allCrueltySpots.Any())
            //                                             {
            //                                                 _featuredCrueltySpot = allCrueltySpots.First();
            //                                                 var resourceId = Resources.GetIdentifier(_featuredCrueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", PackageName);
            //                                                 FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
            //                                                 FindViewById<TextView>(Resource.Id.Name).Text = _featuredCrueltySpot.Name;
            //                                                 FindViewById<TextView>(Resource.Id.Address).Text = _featuredCrueltySpot.Address;
            //                                                 //FindViewById<TextView>(Resource.Id.boycotts).Visibility = ViewStates.Visible;
            //                                                 FindViewById<LinearLayout>(Resource.Id.linearLayoutFeatured).Visibility = ViewStates.Visible;

            //                                                  var distanceLabel = FindViewById<TextView>(Resource.Id.Distance);
            //                                                  var gpsTracker = ((AfaApplication) ApplicationContext).GetGpsTracker(this);
            //                                                  var myLat = gpsTracker.Latitude;
            //                                                  var myLng = gpsTracker.Longitude;
            //                                                  var geoHelper = new GeoHelper();
            //                                                  double distance = geoHelper.Distance(myLat, myLng, _featuredCrueltySpot.Latitude, _featuredCrueltySpot.Longitude, 'M');
            //                                                  distanceLabel.Text = distance.ToString("N2") + " miles";

            //                                                 if (allCrueltySpots.Count > 1)
            //                                                 {
            //                                                     _otherCrueltySpots = allCrueltySpots.Skip(1).ToList();
            //                                                     _crueltySpotsList.Adapter =
            //                                                         new CrueltySpotsAdapter(this, _otherCrueltySpots.ToList());
            //                                                 }
            //                                             }
            //                                             else
            //                                             {
            //                                                 // ToDo: Handle case where there are no cruelty spots
            //                                             }

            //                                             _loadingDialog.Hide();
            //                                         }));

            var crueltySpotsService = new CrueltySpotsService();
            var allCrueltySpots = await crueltySpotsService.GetAllAsync(true);
            if (allCrueltySpots.Any())
            {
                //_featuredCrueltySpot = allCrueltySpots.First();
                //var resourceId = Resources.GetIdentifier(_featuredCrueltySpot.CrueltySpotCategory.IconName.Replace(".png", ""), "drawable", PackageName);
                //FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                //FindViewById<TextView>(Resource.Id.Name).Text = _featuredCrueltySpot.Name;
                //FindViewById<TextView>(Resource.Id.Address).Text = _featuredCrueltySpot.Address;
                ////FindViewById<TextView>(Resource.Id.boycotts).Visibility = ViewStates.Visible;
                //FindViewById<LinearLayout>(Resource.Id.linearLayoutFeatured).Visibility = ViewStates.Visible;

                //var distanceLabel = FindViewById<TextView>(Resource.Id.Distance);
                //var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
                //var myLat = gpsTracker.Latitude;
                //var myLng = gpsTracker.Longitude;
                //var geoHelper = new GeoHelper();
                //double distance = geoHelper.Distance(myLat, myLng, _featuredCrueltySpot.Latitude, _featuredCrueltySpot.Longitude, 'M');
                //distanceLabel.Text = distance.ToString("N2") + " miles";

                //if (allCrueltySpots.Count > 1)
                //{
                //    _otherCrueltySpots = allCrueltySpots.Skip(1).ToList();
                //    _crueltySpotsList.Adapter =
                //        new CrueltySpotsAdapter(this, _otherCrueltySpots.ToList());
                //}

				RunOnUiThread (() => {
					_crueltySpots = allCrueltySpots;
					_crueltySpotsList.Adapter = new CrueltySpotsAdapter(this, _crueltySpots);
					_loadingDialog.Hide();
				});
               
            }
            else
            {
                // ToDo: Handle case where there are no cruelty spots
            }

           
        }

        private void NavigateToCrueltySpotDetails(string crueltySpotId)
        {
            var intent = new Intent(this, typeof(CrueltySpotActivity));
            intent.PutExtra(AppConstants.CrueltySpotIdKey, crueltySpotId);
            StartActivity(intent);
        }
    }
}