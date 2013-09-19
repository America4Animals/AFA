using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Utility
{
    public class GeoQueryRequest
    {
        public double Latitude { get; set; }
        public double Longititude { get; set; }
        public int DistanceInMiles { get; set; }
    }
}