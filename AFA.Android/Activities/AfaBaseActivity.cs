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

namespace AFA.Android
{
	[Activity (Label = "AfaBaseActivity")]			
	public class AfaBaseActivity : SherlockFragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}

		public override bool OnCreateOptionsMenu (ActionBar_Sherlock.View.IMenu menu)
		{
			base.OnCreateOptionsMenu (menu);
			menu.Add (0, 0, 100, Resource.String.loginMenuItem);
			return true;
		}

		public override bool OnOptionsItemSelected (ActionBar_Sherlock.View.IMenuItem item)
		{
			switch (item.ItemId) {
			case 0:
				var intent = new Intent (this, typeof(LoginActivity));
				StartActivity (intent);
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}

		}
	}
}

