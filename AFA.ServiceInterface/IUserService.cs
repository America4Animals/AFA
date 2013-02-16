using AFA.ServiceModel.DTOs;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IUserService
    {
        /// <summary>
        /// GET /users/{Id}
        /// </summary>
        UserResponse Get(UserDto request);

        /// <summary>
        /// GET "/users/{UserId}/organizations"
        /// </summary>
        UserOrganizationsResponse Get(UserOrganizations request);

        object Post(UserDto request);
        object Put(UserDto request);
        object Delete(UserDto request);
        object Post(UserOrganizationAction request);
        IRequestContext RequestContext { get; set; }
    }

    public interface IUsersService
    {
        UsersResponse Get(UsersDto request);
        IRequestContext RequestContext { get; set; }
    }
}