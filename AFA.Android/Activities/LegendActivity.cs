using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AFA.Android.Library;
using AFA.Android.Service;

namespace AFA.Android.Activities
{
	[Activity(Label = "Legend")]
	public class LegendActivity : ReportCrueltyBaseActivity
	{
		private ListView _crueltySpotCategoriesList;
		private IList<CrueltySpotCategory> _crueltySpotCategories;
		private ProgressDialog _loadingDialog;

		protected async override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Legend);

			_loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

			_crueltySpotCategoriesList = FindViewById<ListView>(Resource.Id.Legend);

            var crueltySpotCategoriesService = new CrueltySpotCategoriesService();
            var crueltySpotCategories = await crueltySpotCategoriesService.GetAllAsync();
            _crueltySpotCategories = crueltySpotCategories.ToList();
            _crueltySpotCategoriesList.Adapter = new CrueltyTypesAdapter(this, _crueltySpotCategories);
            _loadingDialog.Hide();
		}
	}
}