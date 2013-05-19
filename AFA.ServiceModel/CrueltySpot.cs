using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace AFA.ServiceModel
{
    public class CrueltySpot
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [References(typeof(StateProvince))]
        public int StateProvinceId { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        //public decimal Latitude { get; set; }
        //public decimal Longitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [References(typeof(CrueltySpotCategory))]
        public int CrueltySpotCategoryId { get; set; }

        public string GooglePlaceId { get; set; }
        public string NonGooglePlaceAddressHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

    }
}
