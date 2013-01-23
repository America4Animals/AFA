using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    /// <summary>
    /// GET /eventcategories
    /// Returns a list of event categories
    /// </summary>
    public class EventCategoriesService : ServiceStack.ServiceInterface.Service
    {
        public object Get(EventCategories request)
        {
            return new EventCategoriesResponse
            {
                EventCategories = Db.Select<EventCategory>()
            };
        }
    }
}
