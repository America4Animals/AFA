using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Utility
{
    public enum ResourceSize
    {
        Base,
        Medium,
        Large
    }

    public static class ResourceHelper
    {
        public static int GetDrawableResourceId(Context context, string resourceName, ResourceSize resourceSize)
        {
            string formattedResourceName = resourceName.Replace(".png", "");
            if (resourceSize == ResourceSize.Medium)
            {
                formattedResourceName += "_med";
            }
            else if (resourceSize == ResourceSize.Large)
            {
                formattedResourceName += "_lg";
            }

            return context.Resources.GetIdentifier(formattedResourceName, "drawable", context.PackageName);
        }
    }
}