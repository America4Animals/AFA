using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AFA.Android.Library;
using AFA.Android.Service;
using Android.Preferences;
using Android.Graphics;

namespace AFA.Android.Activities
{
	[Activity(Label = "Filter/Legend")]
	public class LegendActivity : Activity
	{
		private ListView _crueltySpotCategoriesList;
		private IList<CrueltySpotCategory> _crueltySpotCategories;
		private ProgressDialog _loadingDialog;

		protected override async  void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Legend);

			CheckBox closestFilterCheckbox = FindViewById<CheckBox>(Resource.Id.chooseClosest);
			Boolean filterClosest = UserPreferencesHelper.getClosestSpotsFilter ();
			closestFilterCheckbox.Checked = filterClosest;

			closestFilterCheckbox.Click += (sender, args) => {

				if (closestFilterCheckbox.Checked) {
					UserPreferencesHelper.setClosestSpotsFilter (true);
				} else {
					UserPreferencesHelper.setClosestSpotsFilter (false);
				}
			};


			_loadingDialog = LoadingDialogManager.ShowLoadingDialog (this);

			_crueltySpotCategoriesList = FindViewById<ListView> (Resource.Id.Legend);

			var crueltySpotCategoriesService = new CrueltySpotCategoriesService ();
			_crueltySpotCategories = await crueltySpotCategoriesService.GetAllAsync ();

			RunOnUiThread (() => {
				_loadingDialog.Dismiss ();
				_crueltySpotCategoriesList.Adapter = new LegendAdapter (this, _crueltySpotCategories);
				

			});               


			List<String> categories = UserPreferencesHelper.GetFilterCategories ();
			if (categories.Count () == 0) {
				foreach (CrueltySpotCategory category in _crueltySpotCategories) {
					categories.Add (category.ObjectId);
				}
				UserPreferencesHelper.SaveFilterCategories (categories);
			}



		}
	}
}