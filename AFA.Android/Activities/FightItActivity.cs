using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Adapters;
using AFA.Android.Helpers;
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
        private IList<CrueltySpotDto> _crueltySpots;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FightIt);
            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.FightIt);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);
            var crueltySpotsService = new CrueltySpotsService();

            crueltySpotsService.GetManyAsync<CrueltySpotsResponse>(new CrueltySpotsDto
                                                 {
                                                     SortBy = "createdAt",
                                                     SortOrder = "desc"
                                                 }, r => RunOnUiThread(() =>
                                                     {
                                                         _crueltySpots = r.CrueltySpots;
                                                         if (_crueltySpots.Any())
                                                         {
                                                             var featuredCrueltySpot = _crueltySpots.First();
                                                             var resourceId = Resources.GetIdentifier(featuredCrueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", PackageName);
                                                             FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                                                             FindViewById<TextView>(Resource.Id.Name).Text = featuredCrueltySpot.Name;
                                                             FindViewById<TextView>(Resource.Id.Address).Text = featuredCrueltySpot.Address;
                                                             FindViewById<TextView>(Resource.Id.boycotts).Visibility = ViewStates.Visible;
                                                             FindViewById<LinearLayout>(Resource.Id.linearLayoutFeatured).Visibility = ViewStates.Visible;

                                                             var distanceLabel = FindViewById<TextView>(Resource.Id.Distance);
                                                             var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
                                                             var myLat = gpsTracker.Latitude;
                                                             var myLng = gpsTracker.Longitude;
                                                             var geoHelper = new GeoHelper();
                                                             double distance = geoHelper.Distance(myLat, myLng, featuredCrueltySpot.Latitude, featuredCrueltySpot.Longitude, 'M');
                                                             distanceLabel.Text = distance.ToString("N2") + " miles";

                                                             _crueltySpotsList = FindViewById<ListView>(Resource.Id.CrueltySpots);
                                                             _crueltySpotsList.Adapter = new CrueltySpotsAdapter(this, _crueltySpots.Skip(1).ToList());
                                                             _loadingDialog.Hide();
                                                         }
                                                         else
                                                         {
                                                             // ToDo: Handle case where there are no cruelty spots
                                                         }
                                                     }));

            //_crueltySpots = crueltySpotsService.GetMany(new CrueltySpotsDto
            //                                {
            //                                    SortBy = "createdAt",
            //                                    SortOrder = "desc"
            //                                });
            //if (_crueltySpots.Any())
            //{
            //    var featuredCrueltySpot = _crueltySpots.First();
            //    var resourceId = Resources.GetIdentifier(featuredCrueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", PackageName);
            //    FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                
            //    FindViewById<TextView>(Resource.Id.Name).Text = featuredCrueltySpot.Name;
            //    FindViewById<TextView>(Resource.Id.Address).Text = featuredCrueltySpot.Address;

            //    var distanceLabel = FindViewById<TextView>(Resource.Id.Distance);
            //    var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
            //    var myLat = gpsTracker.Latitude;
            //    var myLng = gpsTracker.Longitude;
            //    var geoHelper = new GeoHelper();
            //    double distance = geoHelper.Distance(myLat, myLng, featuredCrueltySpot.Latitude, featuredCrueltySpot.Longitude, 'M');
            //    distanceLabel.Text = distance.ToString("N2") + " miles";

            //    _crueltySpotsList = FindViewById<ListView>(Resource.Id.CrueltySpots);
            //    _crueltySpotsList.Adapter = new CrueltySpotsAdapter(this, _crueltySpots.Skip(1).ToList());
            //    _loadingDialog.Hide();
            //}
            //else
            //{
            //    // ToDo: Handle case where there are no cruelty spots
            //}

            //_loadingDialog.Hide();

        }
    }
}