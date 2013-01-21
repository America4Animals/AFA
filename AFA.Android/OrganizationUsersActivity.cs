using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.ServiceModel.DTOs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android
{
    [Activity(Label = "Organization Users")]
    public class OrganizationUsersActivity : Activity
    {
        private ListView _usersList;
        private IList<UserDto> _users;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Organizations);

            var loadingDialog = new ProgressDialog(this);
            loadingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            loadingDialog.Show();

            _usersList = FindViewById<ListView>(Resource.Id.OrganizationUsers);
            _usersList.ItemClick += (sender, e) =>
            {
                //var organization = _users[e.Position];
                //var intent = new Intent(this, typeof(OrganizationActivity));
                //intent.PutExtra("organizationId", organization.Id);
                //StartActivity(intent);
            };

            //AfaApplication.ServiceClient.GetAsync(new OrganizationUsers {OrganizationId = Convert.ToInt32(Intent.GetStringExtra("organizationId"))},
            //    r => RunOnUiThread(() =>
            //    {
            //        _users = r.Users.ToList();
            //        _usersList.Adapter = new OrganizationListAdapter(this, _users);
            //        loadingDialog.Hide();
            //    }),
            //    (r, ex) => RunOnUiThread(() =>
            //    {
            //        throw ex;
            //    }));
        }
    }
}