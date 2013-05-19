using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FightIt);
            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.FightIt);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);
            var crueltySpotsService = new CrueltySpotsService();
            var crueltySpots = crueltySpotsService.GetMany(new CrueltySpotsDto
                                            {
                                                SortBy = "createdAt",
                                                SortOrder = "desc"
                                            });
            if (crueltySpots.Any())
            {
                var featuredCrueltySpot = crueltySpots.First();
                var resourceId = Resources.GetIdentifier(featuredCrueltySpot.CrueltySpotCategoryIconName.Replace(".png", ""), "drawable", PackageName);
                FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                
                FindViewById<TextView>(Resource.Id.Name).Text = featuredCrueltySpot.Name;
                FindViewById<TextView>(Resource.Id.Address).Text = featuredCrueltySpot.Address;

                var distanceLabel = FindViewById<TextView>(Resource.Id.Distance);
                var gpsTracker = ((AfaApplication)ApplicationContext).GetGpsTracker(this);
                var myLat = gpsTracker.Latitude;
                var myLng = gpsTracker.Longitude;
                var geoHelper = new GeoHelper();
                double distance = geoHelper.Distance(myLat, myLng, featuredCrueltySpot.Latitude, featuredCrueltySpot.Longitude, 'M');
                distanceLabel.Text = distance.ToString("N2") + " miles";
            }
            else
            {
                // ToDo: Handle case where there are no cruelty spots
            }

            _loadingDialog.Hide();

        }
    }
}