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

namespace AFA.Android
{
    [Activity(Label = "Organization")]
    public class OrganizationActivity : Activity
    {
        private int _organizationId;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Organization);

            _organizationId = Intent.GetIntExtra("organizationId", -1);
            //var organization = AfaApplication.ServiceClient.Get(new Organization { Id = _organizationId }).Organization;
            var organization = AfaApplication.ServiceClient.Get(new OrganizationDto { Id = _organizationId }).Organization;

            FindViewById<TextView>(Resource.Id.OrgName).Text = organization.Name;
        }
    }
}