using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA_Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Parse;

namespace AFA.Android
{
    [Application]
    public class AfaApplication : Application
    {
        private GPSTracker _gpsTracker;

        public AfaApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            // Initialize the Parse client with your Application ID and Windows Key found on
            // your Parse dashboard
            ParseObject.RegisterSubclass<CrueltySpotCategory>();
            ParseObject.RegisterSubclass<CrueltySpot>();
            ParseClient.Initialize(AfaConfig.ParseApplicationId, AfaConfig.ParseWindowsKey);
            ParseFacebookUtils.Initialize(AfaConfig.FacebookApplicationId);

        }

        public CrueltyReport CrueltyReport { get; set; }

        public GPSTracker GetGpsTracker(Context context)
        {
            if (_gpsTracker != null)
            {
                return _gpsTracker;
            }
            else
            {
                _gpsTracker = new GPSTracker(this);
                return _gpsTracker;
            }
        }
    }
}