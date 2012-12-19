using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AFA.Service.Models;
using ServiceStack.OrmLite;
using ServiceStack.Common.Web;

namespace AFA.Service.Services
{
    public class OrganizationCategoryService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /organization-categories/{Id} 
        /// </summary>
        public object Get(OrganizationCategory organizationCategory)
        {
            return new OrganizationCategoryResponse
                       {
                           OrganizationCategory = Db.Id<OrganizationCategory>(organizationCategory.Id)
                       };
        }

        public object Post(OrganizationCategory organizationCategory)
        {
            Db.Insert(organizationCategory);
            return new HttpResult(Db.GetLastInsertId(), HttpStatusCode.Created);
        }

        public object Put(OrganizationCategory organizationCategory)
        {
            Db.Update(organizationCategory);
            return new HttpResult {StatusCode = HttpStatusCode.NoContent};
        }

        public object Delete(OrganizationCategory organizationCategory)
        {
            Db.DeleteById<OrganizationCategory>(organizationCategory.Id);
            return new HttpResult { StatusCode = HttpStatusCode.NoContent };
        }
    }

    /// <summary>
    /// GET /organization-categories
    /// Returns a list of organization categories
    /// </summary>
    public class OrganizationCategoriesService : ServiceStack.ServiceInterface.Service
    {
        public object Get(OrganizationCategories request)
        {
            return new OrganizationCategoriesResponse
                       {
                           OrganizationCategories = Db.Select<OrganizationCategory>()
                       };
        }
    }
}