using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AFA.Android.Library.ServiceModel
{
    public class CrueltySpot
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        //public string StateProvinceId { get; set; }
        //public string StateProvinceName { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string GooglePlaceId { get; set; }
        public string NonGooglePlaceAddressHash { get; set; }

        public CrueltySpotCategory CrueltySpotCategory { get; set; }

        public string CrueltySpotCategoryId { get; set; }
        //public string CrueltySpotCategoryName { get; set; }
        //public string CrueltySpotCategoryIconName { get; set; }

        public string CityAndState
        {
            get
            {
                if (StateProvinceAbbreviation != null)
                {
                    // State specified
                    if (string.IsNullOrWhiteSpace(City))
                    {
                        return StateProvinceAbbreviation;
                    }
                    else
                    {
                        // City and State specified
                        return City + ", " + StateProvinceAbbreviation;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(City))
                {
                    // City specified
                    return City;
                }

                return "";
            }
        }

        public string CityStateAndZip
        {
            get
            {
                var cityAndState = CityAndState;
                if (!String.IsNullOrEmpty(Zipcode))
                {
                    return String.Format("{0} {1}", cityAndState, Zipcode);
                }

                return cityAndState;
            }
        }
    }
}