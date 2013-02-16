using AFA.ServiceModel.DTOs;
using ServiceStack.ServiceHost;

namespace AFA.ServiceInterface
{
    public interface IEventService
    {
        /// <summary>
        /// GET /events/{Id}
        /// </summary>
        EventResponse Get(EventDto request);

        object Post(EventDto request);
        object Put(EventDto request);
        object Delete(EventDto request);
        IRequestContext RequestContext { get; set; }
    }

    public interface IEventsService
    {
        EventsResponse Get(EventsDto request);
        object Get(UpcomingEventsDto request);
        IRequestContext RequestContext { get; set; }
    }
}