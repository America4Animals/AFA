using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AFA.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.Common.Web;

namespace AFA.ServiceInterface
{
    public class OrganizationCategoryService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /organizationcategories/{Id} 
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
            //return new HttpResult(Db.GetLastInsertId(), HttpStatusCode.Created);
            return new OrganizationCategoryResponse { OrganizationCategory = new OrganizationCategory() };
        }

        public object Put(OrganizationCategory organizationCategory)
        {
            Db.Update(organizationCategory);
            //return new HttpResult {StatusCode = HttpStatusCode.NoContent};
            return new OrganizationCategoryResponse { OrganizationCategory = new OrganizationCategory() };
        }

        public object Delete(OrganizationCategory organizationCategory)
        {
            Db.DeleteById<OrganizationCategory>(organizationCategory.Id);
            //return new HttpResult { StatusCode = HttpStatusCode.NoContent };
            return new OrganizationCategoryResponse { OrganizationCategory = new OrganizationCategory() };
        }
    }

    /// <summary>
    /// GET /organizationcategories
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