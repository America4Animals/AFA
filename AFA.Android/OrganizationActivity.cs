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

        private int _organizationId;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Organization);

            _organizationId = Intent.GetIntExtra("organizationId", -1);
            var organization = AfaApplication.ServiceClient.Get(new OrganizationDto { Id = _organizationId }).Organization;

            FindViewById<TextView>(Resource.Id.OrgName).Text = organization.Name;

            if (organization.NeedsVolunteers)
            {
                FindViewById<TextView>(Resource.Id.NeedVolunteersLabel).Visibility = ViewStates.Visible;
            }

            FindViewById<TextView>(Resource.Id.DescriptionText).Text = organization.Description;

            FindViewById<Button>(Resource.Id.FollowingButton).Text = string.Format(FollowingButtonLabelText, organization.OrganizationAlliesCount);
            FindViewById<Button>(Resource.Id.NewsButton).Text = string.Format(NewsButtonLabelText, organization.OrganizationNewsCount);
            FindViewById<Button>(Resource.Id.EventsButton).Text = string.Format(EventsButtonLabelText, organization.OrganizationEventsCount);
            FindViewById<Button>(Resource.Id.CommentsButton).Text = string.Format(CommentsButtonLabelText, organization.OrganizationCommentsCount);

            FindViewById<Button>(Resource.Id.EmailButton).Text = organization.Email;
            FindViewById<Button>(Resource.Id.PhoneButton).Text = organization.PhoneNumber;
            FindViewById<Button>(Resource.Id.AddressButton).Text =
                GlobalHelper.GetFormattedAddress(organization.AddressLine1, organization.AddressLine2,
                                                 organization.CityAndState, organization.Zipcode);

            FindViewById<Button>(Resource.Id.WebsiteButton).Text = organization.WebpageUrl;


        }
    }
}