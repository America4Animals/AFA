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
    public static class DialogManager
    {
        public static void ShowAlertDialog(Context context, string title, string message,
                                    bool isSuccessStatus)
        {
            var alertDialog = new AlertDialog.Builder(context).Create();

            // Setting Dialog Title
            alertDialog.SetTitle(title);

            // Setting Dialog Message
            alertDialog.SetMessage(message);

            // Setting alert dialog icon
            alertDialog.SetIcon(isSuccessStatus ? Resource.Drawable.success : Resource.Drawable.fail);

            // Setting OK Button
            alertDialog.SetButton("OK", (sender, args) => { });

            // Showing Alert Message
            alertDialog.Show();
        }

        public static ProgressDialog ShowLoadingDialog(Context context)
        {
            return ProgressDialog.Show(context, "Submitting", "Please wait...", true);
        }
    }
}