using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ActionBar_Sherlock.App;
using SherlockActionBar = ActionBar_Sherlock.App.ActionBar;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace AFA.Android.Activities
{
    [Activity]
	public class ReportCrueltyBaseActivity : SherlockActivity, ActionBar.ITabListener
    {
        protected CrueltyReport _crueltyReport;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			SupportActionBar.NavigationMode = SherlockActionBar.NavigationModeTabs;

			ActionBar.Tab reportItTab = SupportActionBar.NewTab ();
			reportItTab.SetText ("Report It");
			reportItTab.SetTabListener (this);
			SupportActionBar.AddTab (reportItTab);

			ActionBar.Tab trackItTab = SupportActionBar.NewTab ();
			trackItTab.SetText ("Track It");
			trackItTab.SetTabListener (this);
			SupportActionBar.AddTab (trackItTab);

			ActionBar.Tab fightItTab = SupportActionBar.NewTab ();
			fightItTab.SetText ("Fight It");
			fightItTab.SetTabListener (this);
			SupportActionBar.AddTab (fightItTab);

            _crueltyReport = ((AfaApplication)ApplicationContext).CrueltyReport;
            if (_crueltyReport == null)
            {
                _crueltyReport = new CrueltyReport();
                ((AfaApplication)ApplicationContext).CrueltyReport = _crueltyReport;
            }
        }

        protected void CommitCrueltyReport()
        {
            ((AfaApplication)ApplicationContext).CrueltyReport = _crueltyReport;
        }

		public void OnTabReselected(SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{}

		public void OnTabSelected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
			//mSelected.Text = "Selected: " + tab.Text;
		}

		public void OnTabUnselected (SherlockActionBar.Tab tab, FragmentTransaction transaction)
		{
		}
    }
}