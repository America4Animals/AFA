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
using Android.Util;
using Android.Views;
using Android.Widget;
using AFA.Android.Library.Helpers;

namespace AFA.Android
{
    [Activity(Label = "Organization")]
    public class OrganizationActivity : Activity
    {
        private const string FollowingButtonLabelText = "Who's Following ({0})";
        private const string NewsButtonLabelText = "News ({0})";
        private const string EventsButtonLabelText = "Events ({0})";
        private const string CommentsButtonLabelText = "Comments ({0})";

        private const string FollowingButtonTextFollowing = "End Union";
        private const string FollowingButtonTextNotFollowing = "Unite!";

        private int _organizationId;
        private OrganizationDto _organization;

        private Button _uniteButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Organization);

            _organizationId = Intent.GetIntExtra("organizationId", -1);
            _organization = AfaApplication.ServiceClient.Get(new OrganizationDto { Id = _organizationId }).Organization;

            FindViewById<TextView>(Resource.Id.OrgName).Text = _organization.Name;

            if (_organization.NeedsVolunteers)
            {
                FindViewById<TextView>(Resource.Id.NeedVolunteersLabel).Visibility = ViewStates.Visible;
            }

            FindViewById<TextView>(Resource.Id.DescriptionText).Text = GlobalHelper.GetFieldText(_organization.Description);

            var followersButton = FindViewById<Button>(Resource.Id.FollowingButton);
            followersButton.Text = string.Format(FollowingButtonLabelText, _organization.OrganizationAlliesCount);
            followersButton.Enabled = _organization.OrganizationAlliesCount > 0;
            followersButton.Click += (sender, args) =>
                                         {
                                             var intent = new Intent(this, typeof (OrganizationUsersActivity));
                                             intent.PutExtra("organizationId", _organization.Id);
                                             intent.PutExtra("numFollowers", _organization.OrganizationAlliesCount);
                                             StartActivity(intent);
                                         };

            FindViewById<Button>(Resource.Id.NewsButton).Text = string.Format(NewsButtonLabelText, _organization.OrganizationNewsCount);
            FindViewById<Button>(Resource.Id.EventsButton).Text = string.Format(EventsButtonLabelText, _organization.OrganizationEventsCount);
            FindViewById<Button>(Resource.Id.CommentsButton).Text = string.Format(CommentsButtonLabelText, _organization.OrganizationCommentsCount);

            FindViewById<Button>(Resource.Id.EmailButton).Text = GlobalHelper.GetFieldText(_organization.Email);
            FindViewById<Button>(Resource.Id.PhoneButton).Text = GlobalHelper.GetFieldText(_organization.PhoneNumber);
            FindViewById<Button>(Resource.Id.AddressButton).Text =
                GlobalHelper.GetFormattedAddress(_organization.AddressLine1, _organization.AddressLine2,
                                                 _organization.CityAndState, _organization.Zipcode);

            FindViewById<Button>(Resource.Id.WebsiteButton).Text = GlobalHelper.GetFieldText(_organization.WebpageUrl);

            _uniteButton = FindViewById<Button>(Resource.Id.UniteButton);
            _uniteButton.Text = _organization.CallerIsFollowingOrg
                                    ? FollowingButtonTextFollowing
                                    : FollowingButtonTextNotFollowing;

            _uniteButton.Click += (sender, args) =>
                                     {
                                         if (_organization.CallerIsFollowingOrg)
                                         {
                                             string confirmText =
                                                 String.Format("Are you sure you want to end your union with {0}?", _organization.Name);

                                             new AlertDialog.Builder(this)
                                                 .SetTitle("Unfollow?")
                                                 .SetMessage(confirmText)
                                                 .SetPositiveButton("Yes", (o, eventArgs) => UnfollowOrganization())
                                                 .SetNegativeButton("No", (o, eventArgs) => { })
                                                 .Show();

                                         }
                                         else
                                         {
                                             FollowOrganization();
                                         }
                                     };


        }

        /// <summary>
        /// Call to follow organization and set UI
        /// </summary>
        private void FollowOrganization()
        {
            // Call to follow org

            string alertText = String.Format("You are now following {0}!\nYou'll receive updates in your feed", _organization.Name);

            new AlertDialog.Builder(this)
               .SetTitle("Following Organization")
               .SetMessage(alertText)
               .SetPositiveButton("OK", (o, eventArgs) =>
                                            {
                                                _uniteButton.Text = FollowingButtonTextFollowing;
                                                _organization.CallerIsFollowingOrg = true;
                                            })
               .Show();
        }

        /// <summary>
        /// Call to unfollow organization and set UI
        /// </summary>
        private void UnfollowOrganization()
        {
            // Call to unfollow org

            Toast.MakeText(this, "Unfollowed Organization", ToastLength.Short).Show();

            _uniteButton.Text = FollowingButtonTextNotFollowing;
            _organization.CallerIsFollowingOrg = false;
        }
    }
}