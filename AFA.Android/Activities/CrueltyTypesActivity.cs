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
using Parse;

namespace AFA.Android.Activities
{
    [Activity(Label = "Types of Cruelty")]
    public class CrueltyTypesActivity : ReportCrueltyBaseActivity
    {
        private ListView _crueltySpotCategoriesList;
        private IList<CrueltySpotCategory> _crueltySpotCategories;
        private ProgressDialog _loadingDialog;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltyTypes);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            _crueltySpotCategoriesList = FindViewById<ListView>(Resource.Id.CrueltyTypes);
            _crueltySpotCategoriesList.ItemClick += (sender, e) =>
            {
                var crueltySpotCategory = _crueltySpotCategories[e.Position];
                var intent = new Intent(this, typeof(ReportCrueltyActivity));
                var crueltyType = new CrueltyType();
                //crueltyType.Id = crueltySpotCategory.Id;
                crueltyType.Id = crueltySpotCategory.ObjectId;
                crueltyType.Name = crueltySpotCategory.Name;
                _crueltyReport.CrueltyType = crueltyType;
                CommitCrueltyReport();
                StartActivity(intent);
            };

            var crueltySpotCategoriesService = new CrueltySpotCategoriesService();

            var crueltySpotCategories = await crueltySpotCategoriesService.GetAllAsync();
            RunOnUiThread(() =>
                {
                    _crueltySpotCategories = crueltySpotCategories;
                    _crueltySpotCategoriesList.Adapter = new CrueltyTypesAdapter(this, _crueltySpotCategories);
                    _loadingDialog.Hide();
                });

			/*SetContentView (Resource.Layout.Test);
			var textView = FindViewById<TextView> (Resource.Id.textView1);
			var query = new ParseQuery<CrueltySpotCategory> ();
			var result = await query.FindAsync ();
			var output = "";
			foreach (var crueltySpotCategory in result) {
				output += "Name: " + crueltySpotCategory.Name;
				output += "\n";
				output += "Description: " + crueltySpotCategory.Description;
				output += "\n";
				output += "Icon: " + crueltySpotCategory.IconName;
				output += "\n";
			}

			RunOnUiThread (() => {
				textView.Text = output;
			});*/
            
        }
    }
}