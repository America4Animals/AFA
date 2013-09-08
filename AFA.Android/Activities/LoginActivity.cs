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
using Xamarin.FacebookBinding.Model;
using Xamarin.FacebookBinding.Widget;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/FacebookAppId")]

namespace AFA.Android.Activities
{
    [Activity(Label = "Login", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class LoginActivity : FragmentActivity
    {
		//readonly String PENDING_ACTION_BUNDLE_KEY = "com.facebook.samples.hellofacebook:PendingAction";
		private Session.IStatusCallback _callback;
		private UiLifecycleHelper _uiHelper;
		private IGraphUser _user;
		private LoginButton _loginButton;
		private TextView _greeting;
		//private ViewGroup _controlsContainer;
		private ProfilePictureView _profilePictureView;
		private PendingAction _pendingAction = PendingAction.NONE;

		public LoginActivity()
		{
			_callback = new MyStatusCallback (this);
		}

		enum PendingAction
		{
			NONE,
			POST_PHOTO,
			POST_STATUS_UPDATE
		}

		class MyStatusCallback : Java.Lang.Object, Session.IStatusCallback
		{
			LoginActivity _owner;

			public MyStatusCallback(LoginActivity owner)
			{
				_owner = owner;
			}

		    public void Call(Session session, SessionState state, Java.Lang.Exception exception)
		    {
                _owner.OnSessionStateChange(session, state, exception);
		    }
		}

		class MyUserInfoChangedCallback : Java.Lang.Object, LoginButton.IUserInfoChangedCallback
		{
			LoginActivity _owner;

			public MyUserInfoChangedCallback(LoginActivity owner)
			{
				_owner = owner;
			}

			public void OnUserInfoFetched(IGraphUser user)
			{
				_owner._user = user;
				_owner.UpdateUI ();

			}
		}        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			_uiHelper = new UiLifecycleHelper (this, _callback);

			SetContentView (Resource.Layout.Login);

			_loginButton = (LoginButton)FindViewById (Resource.Id.login_button);
			_loginButton.UserInfoChangedCallback = new MyUserInfoChangedCallback (this);
			_profilePictureView = FindViewById<ProfilePictureView> (Resource.Id.profilePicture);
			_greeting = FindViewById<TextView> (Resource.Id.greeting);

        }

		protected override void OnResume ()
		{
			base.OnResume ();
			_uiHelper.OnResume ();
            UpdateUI();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			_uiHelper.OnSaveInstanceState (outState);

			//outState.PutString (PENDING_ACTION_BUNDLE_KEY, _pendingAction.ToString ());
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			_uiHelper.OnActivityResult (requestCode, (int)resultCode, data);
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			_uiHelper.OnPause ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			_uiHelper.OnDestroy ();
		}

		private void OnSessionStateChange(Session session, SessionState state, Exception exception)
		{
			if (_pendingAction != PendingAction.NONE &&
			    (exception is FacebookOperationCanceledException ||
			 exception is FacebookAuthorizationException)) {
				new AlertDialog.Builder (this)
					.SetTitle (Resource.String.cancelled)
						.SetMessage (Resource.String.permission_not_granted)
						.SetPositiveButton (Resource.String.ok, (object sender, DialogClickEventArgs e) => {})
						.Show ();
				_pendingAction = PendingAction.NONE;
			} else if (state == SessionState.OpenedTokenUpdated) {
				HandlePendingAction ();
			}
			UpdateUI ();
		}

		private void UpdateUI()
		{
			Session session = Session.ActiveSession;
			bool openSession = (session != null && session.IsOpened);

			if (openSession && _user != null) {
				_profilePictureView.ProfileId = (_user.Id);
				_greeting.Text = GetString (Resource.String.hello_user, new Java.Lang.String (_user.FirstName));
			} else {
				_profilePictureView.ProfileId = (null);
				_greeting.Text = (null);
			}
		}

		private void HandlePendingAction()
		{
			PendingAction previouslyPendingAction = _pendingAction;
			// These actions may re-set pendingAction if they are still pending, but we assume they
			// will succeed.
			_pendingAction = PendingAction.NONE;
		}
    }
}