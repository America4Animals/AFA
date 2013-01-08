using System.Collections.Generic;
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
            //Categories = new List<OrganizationCategory>();
        }

        [AutoIncrement]
        public int Id { get; set; }
        [Index(Unique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<OrganizationCategory> Categories { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        //public StateProvince State { get; set; }
        [References(typeof(StateProvince))]
        public int StateProvinceId { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }

        // ToDo: Add Photo
        // ToDo: Add Latitide/Longitude

        //public string CityAndState()
        //{
        //    if (State != null)
        //    {
        //        // State specified
        //        if (string.IsNullOrWhiteSpace(City))
        //        {
        //            return State.Name;
        //        }
        //        else
        //        {
        //            // City and State specified
        //            return City + ", " + State.Abbreviation;
        //        }
        //    }
        //    else if (!string.IsNullOrWhiteSpace(City))
        //    {
        //        // City specified
        //        return City;
        //    }

        //    return "";
        //}

        //public virtual ICollection<User> Allies { get; private set; } 
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