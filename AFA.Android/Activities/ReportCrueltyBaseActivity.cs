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

namespace AFA.Android.Activities
{
    [Activity]
    public class ReportCrueltyBaseActivity : Activity
    {
        protected CrueltyReport _crueltyReport;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
    }
}