using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel.DTOs
{
    [Route("/users/", "POST,PUT,DELETE")]
    [Route("/users/{Id}", "GET")]
    public class UserDto : IReturn<UserResponse>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceName { get; set; }
        public string StateProvinceAbbreviation { get; set; }

        // ToDo: Add Photo

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

        public string FullName
        {
            get
            {
                if (! string.IsNullOrWhiteSpace(FirstName))
                {
                    if (!string.IsNullOrWhiteSpace(LastName))
                    {
                        return FirstName + " " + LastName;
                    }

                    return FirstName;
                }

                return "";
            }
        }

        public string DisplayName {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    if (!string.IsNullOrWhiteSpace(LastName))
                    {
                        return FirstName + " " + LastName.Substring(0, 1).ToUpper() + ".";
                    }

                    return FirstName;
                }

                return "";
            }
        }
    }

    public class UserResponse : IHasResponseStatus
    {
        public UserDto User { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/users", "GET")]
    [Route("/users/organization/{OrganizationId}", "GET")]
    public class UsersDto : IReturn<UsersResponse>
    {
        public int? OrganizationId { get; set; }
    }

    public class UsersResponse
    {
        public List<UserDto> Users { get; set; }
    }

    [Route("/users/{UserId}/organizationaction/{OrganizationId}", "POST")]
    public class UserOrganizationAction : IReturn<UserOrganizationActionResponse>
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string Action { get; set; }
    }

    public class UserOrganizationActionResponse { }

    [Route("/users/{userId}/organizations", "GET")]
    public class UserOrganizations : IReturn<UserOrganizationsResponse>
    {
        public int UserId { get; set; }
    }

    public class UserOrganizationsResponse
    {
        public List<OrganizationDto> Organizations { get; set; }
    }
}
