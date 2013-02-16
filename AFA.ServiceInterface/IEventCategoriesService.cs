using AFA.ServiceModel;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IEventCategoriesService
    {
        EventCategoriesResponse Get(EventCategories request);
        IRequestContext RequestContext { get; set; }
    }
}