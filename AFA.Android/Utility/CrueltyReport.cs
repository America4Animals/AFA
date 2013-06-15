using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Helpers
{
    /// <summary>
    /// Class to Report Cruelty
    /// </summary>
    public class CrueltyReport
    {
        // The Place will be either a google place or a user generated place
        private GooglePlace _googlePlace;
        private UserGeneratedPlace _userGeneratedPlace;

        public GooglePlace GooglePlace {
            get { return _googlePlace; }
            set
            {
                if (value != null)
                {
                    ClearUserGeneratedPlace();
                }

                _googlePlace = value;
            }
        }

        public UserGeneratedPlace UserGeneratedPlace
        {
            get { return _userGeneratedPlace; }
            set
            {
                if (value != null)
                {
                    ClearGooglePlace();
                }

                _userGeneratedPlace = value;
            }
        }

        public CrueltyType CrueltyType { get; set; }

        public string PlaceName
        {
            get
            {
                if (IsGooglePlace())
                {
                    return GooglePlace.Name;
                }
                else if (IsUserGeneratedPlace())
                {
                    return UserGeneratedPlace.Name;
                }

                return null;
            }
        }

        public string PlaceFormattedShortAddress
        {
            get
            {
                if (IsGooglePlace())
                {
                    return GooglePlace.Vicinity;
                }
                else if (IsUserGeneratedPlace())
                {
                    var formattedShortAddress = new StringBuilder();
                    var address = UserGeneratedPlace.Address;
                    var city = UserGeneratedPlace.City;

                    if (!String.IsNullOrEmpty(address))
                    {
                        formattedShortAddress.Append(address);
                    }

                    if (!String.IsNullOrEmpty(city))
                    {
                        if (formattedShortAddress.Length > 0)
                        {
                            formattedShortAddress.Append(", ");
                        }

                        formattedShortAddress.Append(city);
                    }

                    return formattedShortAddress.ToString();
                }

                return null;
            }
        }

        public bool IsGooglePlace()
        {
            return GooglePlace != null && !String.IsNullOrEmpty(GooglePlace.Name);
        }

        public bool IsUserGeneratedPlace()
        {
            return UserGeneratedPlace != null && !String.IsNullOrEmpty(UserGeneratedPlace.Name);
        }

        public void ClearUserGeneratedPlace()
        {
            UserGeneratedPlace = null;
        }

        public void ClearGooglePlace()
        {
            GooglePlace = null;
        }     

        public bool PlaceSpecified
        {
            get { return IsGooglePlace() || IsUserGeneratedPlace(); }
        }

        public bool CrueltyTypeSpecified
        {
            get { return CrueltyType != null && !String.IsNullOrEmpty(CrueltyType.Name); }
        }

        public void ClearAll()
        {
            GooglePlace = null;
            UserGeneratedPlace = null;
            CrueltyType = null;   
        }
    }
}