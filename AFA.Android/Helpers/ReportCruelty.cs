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
    public static class ReportCruelty
    {
        public static string PlaceName { get; set; }
        public static string PlaceVicinity { get; set; }
        public static string Reference { get; set; }
        public static string PlaceText { get; set; }

        public static string CrueltyTypeName { get; set; }
        public static int CrueltyTypeId { get; set; }

        public static bool LocationSpecified
        {
            get { return !String.IsNullOrEmpty(PlaceName); }
        }

        public static bool CrueltyTypeSpecified
        {
            get { return !String.IsNullOrEmpty(CrueltyTypeName); }
        }

        public static void ClearAll()
        {
            PlaceName = null;
            PlaceVicinity = null;
            Reference = null;
            PlaceText = null;
            CrueltyTypeName = null;
            CrueltyTypeId = 0;
        }
    }
}