using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.Service.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OrganizationCategory> Categories { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public StateProvince State { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }
        public string Description { get; set; }
    }
}