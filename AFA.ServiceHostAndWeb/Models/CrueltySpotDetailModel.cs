using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFA.ServiceHostAndWeb.Models
{
    public class CrueltySpotDetailModel
    {
        public int Id { get; set; }
        public int CrueltySpotCategoryId { get; set; }
        public string CrueltySpotCategoryName { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        public string GooglePlaceId { get; set; }
        public string NonGooglePlaceAddressHash { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public SelectList AllStateProvinces { get; set; }
        public SelectList AllCrueltySpotCategories { get; set; }
    }
}