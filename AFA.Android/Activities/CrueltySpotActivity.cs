using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFA.Android.Helpers;
using AFA.Android.Service;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
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
    public class CrueltySpotActivity : Activity
    {
        private ProgressDialog _loadingDialog;
        private CrueltySpotDto _crueltySpot;
        private Button _issueButton;
        private Button _takeActionButton;
        //private CrueltySpotViewState _state = CrueltySpotViewState.TakeAction; 
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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CrueltySpot);

            CrueltyNavMenuHelper.InitCrueltyNavMenu(this, CrueltyNavMenuItem.Child);

            _loadingDialog = LoadingDialogManager.ShowLoadingDialog(this);

            var crueltySpotId = Intent.GetIntExtra(AppConstants.CrueltySpotIdKey, 0);
            var crueltySpotsService = new CrueltySpotsService();
            crueltySpotsService.GetByIdAsync<CrueltySpotResponse>(crueltySpotId, r =>
                RunOnUiThread(() =>
                    {
                        _crueltySpot = r.CrueltySpot;
                        var formattedAddress = String.Format("{0}\n{1}", _crueltySpot.Address, _crueltySpot.CityStateAndZip);
                        FindViewById<TextView>(Resource.Id.Name).Text = _crueltySpot.Name;
                        FindViewById<TextView>(Resource.Id.Address).Text = formattedAddress;
                        var resourceId = ResourceHelper.GetDrawableResourceId(this,
                                                                              _crueltySpot.CrueltySpotCategoryIconName,
                                                                              ResourceSize.Medium);
                        FindViewById<ImageView>(Resource.Id.CrueltyTypeImage).SetImageResource(resourceId);
                        SetViewState(CrueltySpotViewState.TakeAction);
                        _loadingDialog.Hide();

                        var showSuccessAddedAlert = Intent.GetBooleanExtra(AppConstants.ShowCrueltySpotAddedSuccessfullyKey, false);
                        if (showSuccessAddedAlert)
                        {
                            DialogManager.ShowAlertDialog(this, "Thanks!", "...for reporting animal cruelty here. This location is now on the map so others can take action!", true);
                        }

                        var crueltySpotCategoriesService = new CrueltySpotCategoriesService();
                        crueltySpotCategoriesService.GetByIdAsync<CrueltySpotCategoryResponse>(
                            _crueltySpot.CrueltySpotCategoryId, response =>
                                                                RunOnUiThread(() 
                                                                    =>
                                                                    {
                                                                        var crueltySpotCategory = response.CrueltySpotCategory;
                                                                        FindViewById<TextView>(Resource.Id.issueName).Text = crueltySpotCategory.Name;
                                                                        FindViewById<TextView>(Resource.Id.issueDescription).Text = crueltySpotCategory.Description;
                                                                    }));
                    }));

            

            IssueButton.Click += (sender, args) => SetViewState(CrueltySpotViewState.TheIssue);
            TakeActionButton.Click += (sender, args) => SetViewState(CrueltySpotViewState.TakeAction);
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