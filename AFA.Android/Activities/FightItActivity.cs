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
    [Activity(Label = "Fight It")]
    public class FightItActivity : Activity
    {
        private ProgressDialog _loadingDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.FightIt);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);
        }
    }
}