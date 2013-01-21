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

namespace AFA.Android.Helpers
{
    public static class CompassionHelper
    {
        public static void InitCompassionMenu(Activity activity, bool isInEventsArea)
        {
            var calendarMenuButton = activity.FindViewById<Button>(Resource.Id.CalendarMenuButton);
            var orgsMenuButton = activity.FindViewById<Button>(Resource.Id.OrgsMenuButton);

            if (calendarMenuButton != null)
            {
                if (isInEventsArea)
                {
                    calendarMenuButton.SetBackgroundResource(Resource.Color.green);
                    calendarMenuButton.Enabled = false;
                }
                else
                {
                    calendarMenuButton.SetBackgroundResource(Resource.Color.pink);
                    calendarMenuButton.Click += (sender, args) =>
                                                    {
                                                        var intent = new Intent(activity, typeof(EventsActivity));
                                                        activity.StartActivity(intent);
                                                    };
                }
            }

            if (orgsMenuButton != null)
            {
                if (isInEventsArea)
                {
                    orgsMenuButton.SetBackgroundResource(Resource.Color.pink);
                    orgsMenuButton.Click += (sender, args) =>
                                                {
                                                    var intent = new Intent(activity, typeof (OrganizationsActivity));
                                                    activity.StartActivity(intent);
                                                };
                }
                else
                {
                    orgsMenuButton.SetBackgroundResource(Resource.Color.green);
                    orgsMenuButton.Enabled = false;
                }
            }
        }
    }
}