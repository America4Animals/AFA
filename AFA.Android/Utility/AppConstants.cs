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
    public static class AppConstants
    {
        public static string PlaceNameKey = "PlaceName";
        public static string PlaceVicinityKey = "PlaceVicinity";
        public static string PlaceReferenceKey = "PlaceReference";

        public static string CrueltySpotCategoryIdKey = "CrueltySpotCategoryId";
        public static string CrueltySpotCategoryNameKey = "CrueltySpotCategoryName";

        public static string ShowCrueltySpotAddedSuccessfullyKey = "ShowCrueltySpotAddedSuccessfully";

        public static string CrueltySpotIdKey = "CrueltySpotId";

		public static string HasLaunchedAppKey = "AfaHasLaunchedBefore";
		
		public const int LoginLogoutMenuItemId = 10;
    }
}