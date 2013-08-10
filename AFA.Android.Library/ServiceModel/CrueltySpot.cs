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
using Parse;

namespace AFA.Android.Library.ServiceModel
{
    [ParseClassName("CrueltySpot")]
    public class CrueltySpot : ParseObject
    {
        //public string Id { get; set; }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("description")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("address")]
        public string Address
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("city")]
        public string City
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("stateProvinceAbbreviation")]
        public string StateProvinceAbbreviation
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("zipcode")]
        public string Zipcode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("phoneNumber")]
        public string PhoneNumber
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("email")]
        public string Email
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("webpageUrl")]
        public string WebpageUrl
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
		
        public double Latitude
        {
			get { return Location.Latitude; }
        }
		
        public double Longitude
        {
			get { return Location.Longitude; }
        }

        [ParseFieldName("googlePlaceId")]
        public string GooglePlaceId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("nonGooglePlaceAddressHash")]
        public string NonGooglePlaceAddressHash
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("crueltySpotCategory")]
        public CrueltySpotCategory CrueltySpotCategory
        {
            get { return GetProperty<CrueltySpotCategory>(); }
            set { SetProperty(value); }
        }

        [ParseFieldName("location")]
        public ParseGeoPoint Location
        {
            get { return GetProperty<ParseGeoPoint>(); }
            set { SetProperty(value); }
        }

		public string CrueltySpotCategoryId 
		{ 
			get 
			{
				return CrueltySpotCategory == null ? "0" : CrueltySpotCategory.ObjectId;
			} 
		}

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