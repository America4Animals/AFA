using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Library.ServiceModel;
using AFA.Android.Service;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using AFA.Android.Utility;


namespace AFA.Android.Activities
{
    [Activity(Label = "Cruelty Spot")]
    public class CrueltySpotActivity : AfaBaseActivity
    {
        private ProgressDialog _loadingDialog;
        private CrueltySpot _crueltySpot;
        private Button _issueButton;
        private Button _takeActionButton;
        private LinearLayout _boycottArea;
        private LinearLayout _theIssueArea;


        private Button IssueButton
        {
            get
            {
                _issueButton = _issueButton ?? FindViewById<Button>(Resource.Id.isueButton);
                return _issueButton;
            }
        }

        private Button TakeActionButton
        {
            get
            {
                _takeActionButton = _takeActionButton ?? FindViewById<Button>(Resource.Id.takeActionButton);
                return _takeActionButton;
            }
        }

        private enum CrueltySpotViewState
        {
            TheIssue,
            TakeAction
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltySpot);

         

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            // Event Handlers
            IssueButton.Click += (sender, args) => SetViewState(CrueltySpotViewState.TheIssue);
            TakeActionButton.Click += (sender, args) => SetViewState(CrueltySpotViewState.TakeAction);

            var crueltySpotId = Intent.GetStringExtra(AppConstants.CrueltySpotIdKey);
            var crueltySpotsService = new CrueltySpotsService();
            _crueltySpot = await crueltySpotsService.GetByIdAsync(crueltySpotId, true);
            RunOnUiThread(() =>
                {
                    var formattedAddress = String.Format("{0}\n{1}", _crueltySpot.Address, _crueltySpot.CityStateAndZip);
                    FindViewById<TextView>(Resource.Id.Name).Text = _crueltySpot.Name;
                    FindViewById<TextView>(Resource.Id.Address).Text = formattedAddress;
                    var resourceId = ResourceHelper.GetDrawableResourceId(this, _crueltySpot.CrueltySpotCategory.IconName, ResourceSize.Medium);
                    FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                    SetViewState(CrueltySpotViewState.TakeAction);
                    _loadingDialog.Hide();

                    var showSuccessAddedAlert = Intent.GetBooleanExtra(AppConstants.ShowCrueltySpotAddedSuccessfullyKey, false);
                    if (showSuccessAddedAlert)
                    {
                        DialogManager.ShowAlertDialog(this, "Thanks!", "...for reporting animal cruelty here. This location is now on the map so others can take action!", true);
                    }

                    FindViewById<TextView>(Resource.Id.issueName).Text = _crueltySpot.CrueltySpotCategory.Name;
                    FindViewById<TextView>(Resource.Id.issueDescription).Text = _crueltySpot.CrueltySpotCategory.Description;
                });
        }

        private void SetViewState(CrueltySpotViewState state)
        {
            _boycottArea = FindViewById<LinearLayout>(Resource.Id.linearLayoutBoycott);
            _theIssueArea = FindViewById<LinearLayout>(Resource.Id.linearLayoutTheIssue);

            switch (state)
            {
                case CrueltySpotViewState.TakeAction:
                    SetButtonState(TakeActionButton, false);
                    SetButtonState(IssueButton, true);
                    _theIssueArea.Visibility = ViewStates.Gone;
                    _boycottArea.Visibility = ViewStates.Visible;
                    break;
                case CrueltySpotViewState.TheIssue:
                    SetButtonState(TakeActionButton, true);
                    SetButtonState(IssueButton, false);
                    _theIssueArea.Visibility = ViewStates.Visible;
                    _boycottArea.Visibility = ViewStates.Gone;
                    break;
            }
        }

        private void SetButtonState(Button button, bool isEnabled)
        {
            button.SetBackgroundColor(isEnabled
                ? this.Resources.GetColor(Resource.Color.mediumgray)
                : this.Resources.GetColor(Resource.Color.darkgray));

            button.Enabled = isEnabled;
        }
    }
}