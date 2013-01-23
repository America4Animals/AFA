using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel
{
    [Route("/organizationcategories/", "POST,PUT,DELETE")]
    [Route("/organizationcategories/{Id}", "GET")]
    public class OrganizationCategory : IReturn<OrganizationCategoryResponse>
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Index(Unique = true)]
        public string Name { get; set; }
    }

    public class OrganizationCategoryResponse : IHasResponseStatus
    {
        public OrganizationCategory OrganizationCategory { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/organizationcategories", "GET")]
    public class OrganizationCategories : IReturn<OrganizationCategoriesResponse>
    {
    }

    public class OrganizationCategoriesResponse : IHasResponseStatus
    {
        public List<OrganizationCategory> OrganizationCategories { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}