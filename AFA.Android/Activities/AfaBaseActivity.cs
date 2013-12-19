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
using ActionBar_Sherlock.App;
using AFA.Android.Activities;
using Parse;
using AFA.Android.Helpers;
using Xamarin.FacebookBinding;
using Android.Content.PM; 

namespace AFA.Android
{
	[Activity (Label = "AfaBaseActivity", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class AfaBaseActivity : SherlockFragmentActivity
	{
		private ActionBar_Sherlock.View.IMenuItem _loginLogoutMenuItem;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}

		public override bool OnCreateOptionsMenu (ActionBar_Sherlock.View.IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);
			var isLoggedIn = ParseUser.CurrentUser != null;
			var loginLogoutResourceText = isLoggedIn ? Resource.String.logoutMenuItem : Resource.String.loginMenuItem;
			menu.Add (0, AppConstants.LoginLogoutMenuItemId, 100, loginLogoutResourceText);

			return true;
		}

		public override bool OnOptionsItemSelected (ActionBar_Sherlock.View.IMenuItem item)
		{
			switch (item.ItemId) {
			case AppConstants.LoginLogoutMenuItemId:
				_loginLogoutMenuItem = item;
				var menuItemText = item.TitleFormatted.ToString ();
				if (menuItemText.ToLower () == "login") {
					var intent = new Intent (this, typeof(AFA.Android.Activities.LoginActivity));
					StartActivity (intent);
				} else {
					Logout ();
				}
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		private void Logout()
		{
			ParseUser.LogOut ();
			FacebookGateway.LogoutFacebookIfLoggedIn (true);
			Toast.MakeText (this, "You are now logged out", ToastLength.Short).Show ();
			_loginLogoutMenuItem.SetTitle (Resource.String.loginMenuItem);
		}
	}
}

