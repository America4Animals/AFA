using AFA.ServiceModel.DTOs;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IOrganizationService
    {
        /// <summary>
        /// GET /organizations/{Id}
        /// </summary>
        OrganizationResponse Get(OrganizationDto request);

        /// <summary>
        /// Get Organization Users/Followers
        /// GET /organizations/{OrganizationId}/users
        /// </summary>
        OrganizationUsersResponse Get(OrganizationUsers request);

        /// <summary>
        /// Get Organization Comments
        /// GET /organizations/{OrganizationId}/comments
        /// </summary>
        OrganizationCommentsResponse Get(OrganizationCommentsDto request);

        object Post(OrganizationDto request);

        /// <summary>
        /// Post Organization Comment
        /// POST organizations/{OrganizationId}/comments
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        object Post(OrganizationCommentDto request);

        object Put(OrganizationDto organizationDto);
        object Delete(OrganizationDto organizationDto);
        IRequestContext RequestContext { get; set; }
    }

    public interface IOrganizationsService
    {
        OrganizationsResponse Get(OrganizationsDto request);
        IRequestContext RequestContext { get; set; }
    }
}