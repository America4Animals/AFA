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

namespace AFA.Android.Activities
{
    [Activity(Label = "@string/ApplicationName")]
    public class IntroActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Intro);

            FindViewById<RelativeLayout>(Resource.Id.relativeLayoutReport).Click += (sender, args) =>
                {
                    var intent = new Intent(this, typeof (ReportCrueltyActivity));
                    StartActivity(intent);
                };

            FindViewById<RelativeLayout>(Resource.Id.relativeLayoutTrack).Click += (sender, args) =>
            {
                var intent = new Intent(this, typeof(CrueltyMapActivity));
                StartActivity(intent);
            };

            FindViewById<RelativeLayout>(Resource.Id.relativeLayoutFight).Click += (sender, args) =>
            {
                var intent = new Intent(this, typeof(FightItActivity));
                StartActivity(intent);
            };
        }
    }
}