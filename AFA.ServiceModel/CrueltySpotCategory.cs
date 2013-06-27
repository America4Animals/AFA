using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace AFA.ServiceModel
{
    [Route("/crueltyspotcategories/", "POST,PUT,DELETE")]
    [Route("/crueltyspotcategories/{Id}", "GET")]
    public class CrueltySpotCategory : IReturn<CrueltySpotCategoryResponse>
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Index(Unique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public string ColorCode { get; set; }
    }

    public class CrueltySpotCategoryResponse
    {
        public CrueltySpotCategory CrueltySpotCategory { get; set; }
    }

    [Route("/crueltyspotcategories", "GET")]
    public class CrueltySpotCategories : IReturn<CrueltySpotCategoriesResponse>
    {
    }

    public class CrueltySpotCategoriesResponse
    {
        public List<CrueltySpotCategory> CrueltySpotCategories { get; set; }
    }
}
