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
using Android.Util;

namespace AFA.Android.Utility
{
    public static class DebugHelper
    {
		/*public static string AfaDebugKeyTemplate = "AfaDebug - {0}";

		private static string FormattedLogEntryKey(string keyText)
        {
            return String.Format(AfaDebugKeyTemplate, keyText);
        }*/

		public static string AndroidDebugTag = "AfaDebug";

		public static void WriteDebugEntry(string message)
		{
			Log.Debug(AndroidDebugTag, message);
		}

    }
}