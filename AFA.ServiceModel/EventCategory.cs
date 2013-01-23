using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace AFA.ServiceModel
{
    [Route("/eventcategories/", "POST,PUT,DELETE")]
    [Route("/eventcategories/{Id}", "GET")]
    public class EventCategory : IReturn<EventCategoryResponse>
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Index(Unique = true)]
        public string Name { get; set; }
    }

    public class EventCategoryResponse
    {
        public EventCategory EventCategory { get; set; }
    }

    [Route("/eventcategories", "GET")]
    public class EventCategories : IReturn<EventCategoriesResponse>
    {
    }

    public class EventCategoriesResponse
    {
        public List<EventCategory> EventCategories { get; set; }
    }
}
