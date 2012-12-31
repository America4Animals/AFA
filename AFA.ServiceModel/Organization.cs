using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel
{
    [Route("/organizations/", "POST,PUT,DELETE")]
    [Route("/organizations/{Id}", "GET")]
    public class Organization : IReturn<OrganizationResponse>
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
        // ToDo: Add Latitide/Longitude
    }

    public class OrganizationResponse : IHasResponseStatus
    {
        public Organization Organization { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    // ToDo: Add route to get orgs near specified location
    [Route("/organizations", "GET")]
    [Route("/organizations/category/{CategoryId}", "GET")]
    public class Organizations : IReturn<OrganizationsResponse>
    {
        public int? CategoryId { get; set; }
    }

    public class OrganizationsResponse
    {
        public List<Organization> Organizations { get; set; }
    }
}