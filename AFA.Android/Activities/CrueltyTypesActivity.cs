using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AFA.Android.Library;

namespace AFA.Android.Activities
{
    [Activity(Label = "Types of Cruelty")]
    public class CrueltyTypesActivity : Activity
    {
        private ListView _crueltySpotCategoriesList;
        private IList<CrueltySpotCategory> _crueltySpotCategories;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltyTypes);

            var loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            _crueltySpotCategoriesList = FindViewById<ListView>(Resource.Id.CrueltyTypes);
            _crueltySpotCategoriesList.ItemClick += (sender, e) =>
            {
                var crueltySpotCategory = _crueltySpotCategories[e.Position];
                var intent = new Intent(this, typeof(ReportCrueltyActivity));
                intent.PutExtra(AppConstants.CrueltySpotCategoryIdKey, crueltySpotCategory.Id);
                intent.PutExtra(AppConstants.CrueltySpotCategoryNameKey, crueltySpotCategory.Name);
                StartActivity(intent);
            };

            AfaApplication.ServiceClient.GetAsync(new CrueltySpotCategories(),
                r => RunOnUiThread(() =>
                {
                    _crueltySpotCategories = r.CrueltySpotCategories.OrderBy(csc => csc.Name).ToList();
                    _crueltySpotCategoriesList.Adapter = new CrueltyTypesAdapter(this, _crueltySpotCategories);
                    loadingDialog.Hide();
                }),
                (r, ex) => RunOnUiThread(() =>
                {
                    throw ex;
                }));
        }
    }
}