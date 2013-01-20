using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel.DTOs
{
    [Route("/organizations/", "POST,PUT,DELETE")]
    [Route("/organizations/{Id}", "GET")]
    public class OrganizationDto : IReturn<OrganizationResponse>
    {
        //public OrganizationDto()
        //{
        //    Categories = new List<OrganizationCategory>();    
        //}

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceName { get; set; }
        public string StateProvinceAbbreviation { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebpageUrl { get; set; }
        public bool NeedsVolunteers { get; set; }

        //public List<OrganizationCategory> Categories { get; set; }
        public int OrganizationCategoryId { get; set; }
        public string OrganizationCategoryName { get; set; }

        public int OrganizationAlliesCount { get; set; }
        public int OrganizationNewsCount { get; set; }
        public int OrganizationCommentsCount { get; set; }
        public int OrganizationEventsCount { get; set; }

        public int? CallerUserId { get; set; }
        public bool CallerIsFollowingOrg { get; set; }

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
    }

    public class OrganizationResponse : IHasResponseStatus
    {
        public OrganizationDto Organization { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    // ToDo: Add route to get orgs near specified location
    [Route("/organizations", "GET")]
    [Route("/organizations/category/{CategoryId}", "GET")]
    public class OrganizationsDto : IReturn<OrganizationsResponse>
    {
        public int? CategoryId { get; set; }
    }

    public class OrganizationsResponse
    {
        public List<OrganizationDto> Organizations { get; set; }
    }

    [Route("/organizations/{OrganizationId}/users", "GET")]
    public class OrganizationUsers : IReturn<OrganizationUsersResponse>
    {
        public int OrganizationId { get; set; }
    }

    public class OrganizationUsersResponse
    {
        public List<UserDto> Users { get; set; }
    }

}
