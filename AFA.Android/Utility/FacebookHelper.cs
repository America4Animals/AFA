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
using Xamarin.FacebookBinding;

namespace AFA.Android
{
	public static class FacebookGateway
	{
		public static void LogoutFacebookIfLoggedIn(bool clearTokenCache)
		{
			Session session = Session.ActiveSession;
			bool openSession = (session != null && session.IsOpened);
			if (openSession) {
				if (clearTokenCache) {
					session.CloseAndClearTokenInformation ();
				} else {
					session.Close ();
				}

			}
		}
	}
}

