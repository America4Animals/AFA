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
    public static class LoadingDialogManager
    {
        public static ProgressDialog ShowLoadingDialog(Context context)
        {
            var loadingDialog = new ProgressDialog(context);
            loadingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            loadingDialog.Show();
            return loadingDialog;

            // Alt
            //ProgressDialog.Show(this, "Retrieving Nearby Places", "Please wait...", true);
        }
    }
}