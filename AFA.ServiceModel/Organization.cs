using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel
{
    public class Organization
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        [Index(Unique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        [References(typeof(StateProvince))]
        public int StateProvinceId { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [Index(Unique = true)]
        public string Email { get; set; }
        public string WebpageUrl { get; set; }
        [References(typeof(OrganizationCategory))]
        public int OrganizationCategoryId { get; set; }
        public bool NeedsVolunteers { get; set; }


        // ToDo: Add Photo
        // ToDo: Add Latitide/Longitude
    }
    
}