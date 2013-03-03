using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFA.ServiceHostAndWeb.Models
{
    public class ReportCrueltyModel
    {
        public int CrueltySpotCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        //public SelectList AllCrueltySpotCategories { get; set; }
        public IList<KeyValuePair<int, string>> AllCrueltySpotCategories { get; set; }
    }
}