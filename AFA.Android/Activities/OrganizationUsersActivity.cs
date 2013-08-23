//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AFA.ServiceModel.DTOs;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace AFA.Android
//{
//    [Activity(Label = "Organization Users")]
//    public class OrganizationUsersActivity : Activity
//    {
//        private ListView _usersList;
//        private IList<UserDto> _users;

//        private const string FollowingLabelText = "{0} people are following them";

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);

//            SetContentView(Resource.Layout.OrganizationUsers);

//            var loadingDialog = new ProgressDialog(this);
//            loadingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
//            loadingDialog.Show();

//            var numFollowers = Intent.GetIntExtra("numFollowers", 0);
//            FindViewById<TextView>(Resource.Id.FollowingLabel).Text = String.Format(FollowingLabelText, numFollowers);

//            _usersList = FindViewById<ListView>(Resource.Id.OrganizationUsers);
//            _usersList.ItemClick += (sender, e) =>
//            {
//                var user = _users[e.Position];
//                var intent = new Intent(this, typeof(UserActivity));
//                intent.PutExtra("userId", user.Id);
//                StartActivity(intent);
//            };

//            //AfaApplication.ServiceClient.GetAsync(new OrganizationUsers { OrganizationId = Intent.GetIntExtra("organizationId", 0) },
//            //    r => RunOnUiThread(() =>
//            //    {
//            //        _users = r.Users.ToList();
//            //        _usersList.Adapter = new UserListAdapter(this, _users);
//            //        loadingDialog.Hide();
//            //    }),
//            //    (r, ex) => RunOnUiThread(() =>
//            //    {
//            //        throw ex;
//            //    }));
//        }
//    }
//}