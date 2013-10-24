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
using SherlockActionBar = ActionBar_Sherlock.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using ActionBar_Sherlock.View;
using Android.Support.V4.App;


namespace AFA.Android
{
	/// <summary>
	/// Listener that handles the selection of a tab in the user interface
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TabListener<T> : Java.Lang.Object, ActionBar.ITabListener
		where T: Fragment, new()
	{
		private T _fragment;

		/// <summary>
		/// initializes a new instance of the tab listener
		/// </summary>
		public TabListener()
		{
			_fragment = new T();
		}

		/// <summary>
		/// Initializes a new instance of the tab listener
		/// </summary>
		/// <param name="fragment"></param>
		protected TabListener(T fragment)
		{
			_fragment = fragment;
		}

		/// <summary>
		/// Handles the reselection of the tab
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="ft"></param>
		public void OnTabReselected(ActionBar.Tab tab, FragmentTransaction ft)
		{

		}

		/// <summary>
		/// Adds the fragment when the tab was selected
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="ft"></param>
		public void OnTabSelected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			FragmentTransaction fragTx = _fragment.Activity.SupportFragmentManager.BeginTransaction ();
			fragTx.Add(Resource.Id.fragmentContainer,_fragment, typeof(T).FullName);
			fragTx.Commit ();
		}

		/// <summary>
		/// Removes the fragment when the tab was deselected
		/// </summary>
		/// <param name="tab"></param>
		/// <param name="ft"></param>
		public void OnTabUnselected(ActionBar.Tab tab, FragmentTransaction ft)
		{
			ft.Remove( _fragment);
		}
	}

}

