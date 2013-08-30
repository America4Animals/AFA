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
using Android.Support.V4.App;
using Xamarin.FacebookBinding;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/FacebookAppId")]

namespace AFA.Android.Activities
{
    [Activity(Label = "Login", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class LoginActivity : FragmentActivity
    {
        private Session.IStatusCallback _callback;
        private UiLifecycleHelper _uiHelper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
        }
    }
}