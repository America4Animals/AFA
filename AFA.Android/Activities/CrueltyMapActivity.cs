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
using AFA.Android.Helpers;

namespace AFA.Android.Activities
{
    [Activity(Label = "My Activity")]
    public class CrueltyMapActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltyMap);

            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.TrackIt);
        }
    }
}