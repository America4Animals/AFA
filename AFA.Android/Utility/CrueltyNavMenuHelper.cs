using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Activities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Helpers
{
    public enum CrueltyNavMenuItem
    {
        ReportIt,
        TrackIt,
        FightIt,
        Legend,
        Child
    }

    public class CrueltyNavMenuHelper
    {
        public static void InitCrueltyNavMenu(Activity activity, CrueltyNavMenuItem activeItem)
        {
            var reportItItem = activity.FindViewById<TextView>(Resource.Id.ReportItMenuButton);
            var trackItItem = activity.FindViewById<TextView>(Resource.Id.TrackItMenuButton);
            var fightItItem = activity.FindViewById<TextView>(Resource.Id.FightItMenuButton);
            var moreItItem = activity.FindViewById<TextView>(Resource.Id.MoreMenuButton);

            reportItItem.Click += (sender, args) =>
                                      {
                                          var intent = new Intent(activity, typeof (ReportCrueltyActivity));
                                          activity.StartActivity(intent);
                                      };

            trackItItem.Click += (sender, args) =>
            {
                var intent = new Intent(activity, typeof(CrueltyMapActivity));
                activity.StartActivity(intent);
            };

            fightItItem.Click += (sender, args) =>
                {
                    var intent = new Intent(activity, typeof (FightItActivity));
                    activity.StartActivity(intent);
                };

			moreItItem.Click += (sender, args) =>
			{
				var intent = new Intent(activity, typeof (LegendActivity));
				activity.StartActivity(intent);
			};

            switch (activeItem)
            {
                case CrueltyNavMenuItem.ReportIt:
                    reportItItem.SetBackgroundResource(Resource.Color.gray);
                    reportItItem.Enabled = false;
                    break;
                case CrueltyNavMenuItem.TrackIt:
                    trackItItem.SetBackgroundResource(Resource.Color.gray);
                    trackItItem.Enabled = false;
                    break;
                case CrueltyNavMenuItem.FightIt:
                    fightItItem.SetBackgroundResource(Resource.Color.gray);
                    fightItItem.Enabled = false;
                    break;
                case CrueltyNavMenuItem.Legend:
                    moreItItem.SetBackgroundResource(Resource.Color.gray);
                    moreItItem.Enabled = false;
                    break;
            }
        }
    }
}