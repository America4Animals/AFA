using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceStack.ServiceClient.Web;

namespace AFA.Android
{
    [Activity(Label = "Organizations", MainLauncher = true, Icon = "@drawable/icon")]
    public class OrganizationsActivity : Activity
    {
        private ListView _organizationsList;
        private IList<OrganizationDto> _organizations;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Organizations);

            var loadingDialog = new ProgressDialog(this);
            loadingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            loadingDialog.Show();

            _organizationsList = FindViewById<ListView>(Resource.Id.Organizations);
            _organizationsList.ItemClick += (sender, e) =>
                                                {
                                                    var organization = _organizations[e.Position];
                                                    var intent = new Intent(this, typeof(OrganizationActivity));
                                                    intent.PutExtra("organizationId", organization.Id);
                                                    StartActivity(intent);
                                                };

            AfaApplication.ServiceClient.GetAsync(new OrganizationsDto(), 
                r => RunOnUiThread(() => 
                { 
                    _organizations = r.Organizations.OrderByDescending(o => o.OrganizationAlliesCount).ToList();
                    _organizationsList.Adapter = new OrganizationListAdapter(this, _organizations);
                    loadingDialog.Hide();
                }),
                (r, ex) => RunOnUiThread(() =>
                {
                    throw ex;
                }));
        }

        protected override void OnResume()
        {
            base.OnResume();


        }
    }
}