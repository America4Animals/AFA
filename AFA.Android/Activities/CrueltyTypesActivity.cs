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
using Android.Util;
using Android.Views;
using Android.Widget;
using AFA.Android.Library;
using AFA.Android.Service;

namespace AFA.Android.Activities
{
    [Activity(Label = "Types of Cruelty")]
    public class CrueltyTypesActivity : ReportCrueltyBaseActivity
    {
        private ListView _crueltySpotCategoriesList;
        private IList<CrueltySpotCategory> _crueltySpotCategories;
        private ProgressDialog _loadingDialog;

        protected override void OnCreate(Bundle bundle)
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
                crueltyType.Id = crueltySpotCategory.Id;
                crueltyType.Name = crueltySpotCategory.Name;
                _crueltyReport.CrueltyType = crueltyType;
                CommitCrueltyReport();
                StartActivity(intent);
            };

            //AfaApplication.ServiceClient.GetAsync(new CrueltySpotCategories(),
            //    r => RunOnUiThread(() =>
            //    {
            //        _crueltySpotCategories = r.CrueltySpotCategories.OrderBy(csc => csc.Name).ToList();
            //        _crueltySpotCategoriesList.Adapter = new CrueltyTypesAdapter(this, _crueltySpotCategories);
            //        loadingDialog.Hide();
            //    }),
            //    (r, ex) => RunOnUiThread(() =>
            //    {
            //        throw ex;
            //    }));

            CrueltySpotCategoriesService.GetAllAsync(r => RunOnUiThread(() =>
                              {
                                  _crueltySpotCategories = r.CrueltySpotCategories.OrderBy(csc => csc.Name).ToList();
                                  _crueltySpotCategoriesList.Adapter = new CrueltyTypesAdapter(this,
                                                                                               _crueltySpotCategories);
                                  _loadingDialog.Hide();
                              }));
        }
    }
}