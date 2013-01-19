using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.Web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AFA.Web.Models
{
    public class OrganizationDetailModel
    {
        public int Id { get; set; }
        public int OrganizationCategoryId { get; set; }
        public string OrganizationCategoryName { get; set; }
        [Required]
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
        public bool NeedsVolunteers { get; set; }

        public SelectList AllStateProvinces { get; set; }
        //public IEnumerable<CheckboxItem> AllCategories { get; set; }
        public SelectList AllOrganizationCategories { get; set; }
    }
}