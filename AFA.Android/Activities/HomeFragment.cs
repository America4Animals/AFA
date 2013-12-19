using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Adapters;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using AFA_Android.Helpers;
using AFA.Android.Utility;
using ActionBar_Sherlock.App;
using ActionBar_Sherlock.View;
using SherlockActionBar = ActionBar_Sherlock.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;

namespace AFA.Android.Activities
{
	public class HomeFragment : Fragment
	{
		private TextView ll;
		private FragmentActivity fa;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			fa = base.Activity;
			ll = (TextView)inflater.Inflate (Resource.Layout.HomeFragment, container, false);
			return ll;
		}
	}
}

