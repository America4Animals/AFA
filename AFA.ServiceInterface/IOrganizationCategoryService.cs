using AFA.ServiceModel;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IOrganizationCategoryService
    {
        OrganizationCategoryResponse Get(OrganizationCategory organizationCategory);
        object Post(OrganizationCategory organizationCategory);
        object Put(OrganizationCategory organizationCategory);
        object Delete(OrganizationCategory organizationCategory);
        IRequestContext RequestContext { get; set; }
    }

    public interface IOrganizationCategoriesService
    {
        OrganizationCategoriesResponse Get(OrganizationCategories request);
        IRequestContext RequestContext { get; set; }
    }
}