using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.Service.Models
{
    [Route("/organization-categories/", "POST,PUT,PATCH,DELETE")]
    [Route("/organization-categories/{Id}")]
    public class OrganizationCategory
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

    [Route("/organization-categories", "GET,OPTIONS")]
    public class OrganizationCategories
    {
    }

    public class OrganizationCategoriesResponse : IHasResponseStatus
    {
        public List<OrganizationCategory> OrganizationCategories { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}