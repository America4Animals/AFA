using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface ICrueltySpotCategoryService
    {
        CrueltySpotCategoryResponse Get(CrueltySpotCategory crueltySpotCategory);
        object Post(CrueltySpotCategory crueltySpotCategory);
        object Put(CrueltySpotCategory crueltySpotCategory);
        object Delete(CrueltySpotCategory crueltySpotCategory);
        IRequestContext RequestContext { get; set; }
    }

    public interface ICrueltySpotCategoriesService
    {
        CrueltySpotCategoriesResponse Get(CrueltySpotCategories request);
        IRequestContext RequestContext { get; set; }
    }
}
