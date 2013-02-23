using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class CrueltySpotCategoryService : ServiceStack.ServiceInterface.Service, ICrueltySpotCategoryService
    {
        /// <summary>
        /// GET /crueltyspotcategories/{Id} 
        /// </summary>
        public ServiceModel.CrueltySpotCategoryResponse Get(ServiceModel.CrueltySpotCategory crueltySpotCategory)
        {
            return new CrueltySpotCategoryResponse
            {
                CrueltySpotCategory = Db.Id<CrueltySpotCategory>(crueltySpotCategory.Id)
            };
        }

        public object Post(ServiceModel.CrueltySpotCategory crueltySpotCategory)
        {
            Db.Insert(crueltySpotCategory);
            return new CrueltySpotCategoryResponse { CrueltySpotCategory = new CrueltySpotCategory() };
        }

        public object Put(ServiceModel.CrueltySpotCategory crueltySpotCategory)
        {
            Db.Update(crueltySpotCategory);
            return new CrueltySpotCategoryResponse { CrueltySpotCategory = new CrueltySpotCategory() };
        }

        public object Delete(ServiceModel.CrueltySpotCategory crueltySpotCategory)
        {
            Db.DeleteById<CrueltySpotCategory>(crueltySpotCategory.Id);
            return new CrueltySpotCategoryResponse { CrueltySpotCategory = new CrueltySpotCategory() };
        }
    }

    /// <summary>
    /// GET /crueltyspotcategories
    /// Returns a list of cruelty spot categories
    /// </summary>
    public class CrueltySpotCategoriesService : ServiceStack.ServiceInterface.Service, ICrueltySpotCategoriesService
    {
        public CrueltySpotCategoriesResponse Get(CrueltySpotCategories request)
        {
            return new CrueltySpotCategoriesResponse
            {
                CrueltySpotCategories = Db.Select<CrueltySpotCategory>()
            };
        }
    }
}
