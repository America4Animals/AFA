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

namespace AFA.Android.Library.Helpers
{
    public static class GlobalHelper
    {
        public static string DefaultTextForMissingFields = "not provided";

        public static string GetFormattedAddress(string addressLine1, string addressLine2, string cityAndState,
                                                 string zipcode)
        {
            var ret = new StringBuilder();
            
            if (! string.IsNullOrWhiteSpace(addressLine1))
            {
                ret.Append(addressLine1);
            }

            if (! string.IsNullOrWhiteSpace(addressLine2))
            {
                if (ret.Length > 0)
                {
                    ret.Append("\n");
                }
                ret.Append(addressLine2);
            }

            if (! string.IsNullOrWhiteSpace(cityAndState))
            {
                if (ret.Length > 0)
                {
                    ret.Append("\n");
                }

                ret.Append(cityAndState);

                if (!string.IsNullOrWhiteSpace(zipcode))
                {
                    ret.Append(" ");
                    ret.Append(zipcode);
                }
            }

            if (ret.Length == 0)
            {
                ret.Append(DefaultTextForMissingFields);
            }

            return ret.ToString();
        }

        public static string GetFieldText(string fieldVal)
        {
            return String.IsNullOrWhiteSpace(fieldVal) ? DefaultTextForMissingFields : fieldVal;
        }
    }
}