using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using AFA.Android.Helpers;
using Android.Util;
using AFA.Android.Utility;
using Parse;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/FacebookAppId")]

namespace AFA.Android.Activities
{
    [Activity(Label = "Welcome", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class LoginActivity : FragmentActivity
	{
		//readonly String PENDING_ACTION_BUNDLE_KEY = "com.facebook.samples.hellofacebook:PendingAction";
		private Session.IStatusCallback _callback;
		private UiLifecycleHelper _uiHelper;
		private IGraphUser _user;
		private LoginButton _loginButton;
		private Button _continueAnonymouslyButton;
		//private TextView _greeting;
		//private ViewGroup _controlsContainer;
		//private ProfilePictureView _profilePictureView;
		private PendingAction _pendingAction = PendingAction.NONE;
		private ParseUser _parseUser;
		private LinearLayout _mainLayoutView;
		private ProgressDialog _loadingDialog;

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
			_loginButton.SetReadPermissions(new List<string>{"email"});
			//_profilePictureView = FindViewById<ProfilePictureView> (Resource.Id.profilePicture);
			//_greeting = FindViewById<TextView> (Resource.Id.greeting);

			_mainLayoutView = FindViewById<LinearLayout> (Resource.Id.main_ui_container);

			_continueAnonymouslyButton = FindViewById<Button>(Resource.Id.btnNoLogin);
			_continueAnonymouslyButton.Click += (object sender, EventArgs e) => StartActivity (typeof (IntroActivity));

			UpdateUI ();
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

		private async void UpdateUI()
		{
			_mainLayoutView.Visibility = ViewStates.Visible;
			if (_loadingDialog != null && _loadingDialog.IsShowing) {
				_loadingDialog.Dismiss ();
			}

			Session session = Session.ActiveSession;
			bool openSession = (session != null && session.IsOpened);

			if (openSession && _user != null)
			{
				// Authenticated with Facebook

				//_profilePictureView.ProfileId = (_user.Id);
				//_greeting.Text = GetString (Resource.String.hello_user, new Java.Lang.String (_user.FirstName));

				if (ParseUser.CurrentUser == null) {
					_mainLayoutView.Visibility = ViewStates.Invisible;
					_loadingDialog = DialogManager.ShowLoadingDialog (this, "Logging in", "Please wait...");
					_parseUser = await LoginParse (_user.Id, session.AccessToken, DateTime.MaxValue);
					StartActivity (typeof(IntroActivity));
				} else {
					// Authenticated with Parse and Facebook
					_continueAnonymouslyButton.Visibility = ViewStates.Invisible;
				}
			} else {
				//_profilePictureView.ProfileId = (null);
				//_greeting.Text = (null);

				// Not authenticated with Facebook
				if (ParseUser.CurrentUser != null) {
					ParseUser.LogOut ();
				}

				_continueAnonymouslyButton.Visibility = ViewStates.Visible;
			}
		}

		private void HandlePendingAction()
		{
			PendingAction previouslyPendingAction = _pendingAction;
			// These actions may re-set pendingAction if they are still pending, but we assume they
			// will succeed.
			_pendingAction = PendingAction.NONE;
		}

		private async Task<ParseUser> LoginParse(string userFacebookId, string accessToken, DateTime sessionExpiration)
		{
			var parseUser = await ParseFacebookUtils.LogInAsync(userFacebookId, accessToken, sessionExpiration);

			if (String.IsNullOrEmpty(parseUser.Email)) {
				parseUser.Email = _user.GetProperty("email").ToString();
				await parseUser.SaveAsync();
			}

			return parseUser;
		}
	}
}