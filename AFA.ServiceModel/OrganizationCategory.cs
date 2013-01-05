using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel
{
    [Route("/organization-categories/", "POST,PUT,DELETE")]
    [Route("/organization-categories/{Id}", "GET")]
    public class OrganizationCategory : IReturn<OrganizationCategoryResponse>
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OrganizationCategoryResponse : IHasResponseStatus
    {
        public OrganizationCategory OrganizationCategory { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/organization-categories", "GET")]
    public class OrganizationCategories : IReturn<OrganizationCategoriesResponse>
    {
    }

    public class OrganizationCategoriesResponse : IHasResponseStatus
    {
        public List<OrganizationCategory> OrganizationCategories { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}