﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace AFA.ServiceModel.DTOs
{
    [Route("/crueltyspots/", "POST,PUT,DELETE")]
    [Route("/crueltyspots/{Id}", "GET")]
    public class CrueltySpotDto : IReturn<CrueltySpotResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceName { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string GooglePlaceId { get; set; }
        public string NonGooglePlaceAddressHash { get; set; }

        public int CrueltySpotCategoryId { get; set; }
        public string CrueltySpotCategoryName { get; set; }
        public string CrueltySpotCategoryIconName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public int? CallerUserId { get; set; }

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

    public class CrueltySpotResponse
    {
        public CrueltySpotDto CrueltySpot { get; set; }
    }

    // ToDo: Add route to get cruelty spots near specified location
    [Route("/crueltyspots", "GET")]
    [Route("/crueltyspots/category/{CategoryId}", "GET")]
    public class CrueltySpotsDto : IReturn<CrueltySpotsResponse>
    {
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }

    public class CrueltySpotsResponse
    {
        public List<CrueltySpotDto> CrueltySpots { get; set; }
    }

    [Route("/crueltyspots/googleplaces", "GET")]
    public class CrueltySpotsGooglePlaces : IReturn<CrueltySpotsGooglePlacesResponse>
    {
    }

    public class CrueltySpotsGooglePlacesResponse
    {
        public List<CrueltySpotDto> CrueltySpots { get; set; }
    }
}
