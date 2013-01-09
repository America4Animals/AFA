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
    }

    public class UserResponse : IHasResponseStatus
    {
        public UserDto User { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/users", "GET")]
    [Route("/users/organization/{OrganizationId}", "GET")]
    public class UsersDto : IReturn<OrganizationsResponse>
    {
        public int? OrganizationId { get; set; }
    }

    public class UsersResponse
    {
        public List<UserDto> Users { get; set; }
    }

    [Route("/users/{UserId}/organizationaction/{OrganizationId}", "POST")]
    public class UserOrganizationAction
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string Action { get; set; }
    }

    public class UserOrganizationActionResponse { }
}
