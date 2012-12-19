using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace AFA.Service.Models
{
    [Route("/organizations")]
    [Route("/organizations/{Id}")]
    public class Organization
    {
        public Organization()
        {
            Categories = new List<OrganizationCategory>();
        }

        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OrganizationCategory> Categories { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public StateProvince State { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        // ToDo: Add Photo
    }

    public class OrganizationResponse
    {
        public Organization Organization { get; set; }
    }
}