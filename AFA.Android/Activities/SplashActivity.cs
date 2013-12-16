using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AFA.Android.Activities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AFA.Android.Helpers;
using Parse;
using Android.Content.PM;

namespace AFA.Android
{
	[Activity (MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true,ScreenOrientation = ScreenOrientation.Portrait)]		
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			var firstTimeLaunch = UserPreferencesHelper.IsFirstTimeLaunch ();
			Thread.Sleep(2000);

			// Start our real activity
//			if (firstTimeLaunch) {
//				StartActivity(typeof(LoginActivity));
//			} else {
//				StartActivity (typeof (IntroActivity));
//			}

			if (ParseUser.CurrentUser != null) {
				StartActivity (typeof(IntroActivity));
			} else {
				StartActivity(typeof(LoginActivity));
			}
		}
	}
}

