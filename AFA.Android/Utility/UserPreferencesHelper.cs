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
	public static class UserPreferencesHelper
	{

		public static Boolean getClosestSpotsFilter()
		{
			ISharedPreferences sp = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			return sp.GetBoolean ("closestSpotsFilter", false);

		}

		public static void setClosestSpotsFilter(bool value)
		{
			ISharedPreferences sp = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			sp.Edit ().PutBoolean ("closestSpotsFilter", value).Commit ();
		}

		public static List<String> GetFilterCategories()
		{
			ISharedPreferences sp = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			var categoryList = sp.GetString ("categories", null);
			List<String> categoryIds = new List<String> ();
			if (categoryList != null) {
				categoryIds = categoryList.Split (':').ToList<String> ();
			}
			return categoryIds;
		}

		public static Boolean SaveFilterCategories(List<String> categories)
		{
			ISharedPreferences sp = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			Boolean result = true;
			if (categories.Count () > 0) {
				Boolean firstTime = true;
				String categoryList = "";
				foreach (String category in categories) {
					if (!firstTime) {
						categoryList += ":";

					} else {
						firstTime = false;
					}
					categoryList += category;
				}
				result = sp.Edit ().PutString ("categories", categoryList).Commit ();
				return result;
			}

			return result;
		}

		public static Boolean addCategoryFilter(String categoryName)
		{
			List<String> categories = GetFilterCategories ();
			if (!categories.Contains (categoryName)) {
				categories.Add (categoryName);
			}

			return SaveFilterCategories (categories);
		}

		public static Boolean removeCategoryFilter(String categoryName)
		{

			List<String> categories = GetFilterCategories ();
			if (categories.Contains (categoryName)) {
				categories.Remove (categoryName);
			}

			return SaveFilterCategories (categories);
		}
	}
	
}