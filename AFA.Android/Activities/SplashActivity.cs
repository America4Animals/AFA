
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



namespace AFA.Android
{
	[Activity (MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true)]		
	public class SplashActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Thread.Sleep(2000);
			// Start our real activity
			//StartActivity (typeof (IntroActivity));
            //StartActivity(typeof(LoginActivity));
            StartActivity(typeof(LoginParseActivity));
		}
	}
}

