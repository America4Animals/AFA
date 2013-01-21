using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel.DTOs
{
    [Route("/events/", "POST,PUT,DELETE")]
    [Route("/events/{Id}", "GET")]
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string TimeDescription { get; set; }
        public string DayDescription { get; set; }
        public string OrganizerType { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class EventResponse
    {
        public EventDto Event { get; set; }
    }

    [Route("/events", "GET")]
    public class EventsDto : IReturn<EventsResponse>
    {     
    }

    public class EventsResponse
    {
        public List<EventDto> Events { get; set; }
    }

    [Route("/events/upcoming", "GET")]
    public class UpcomingEventsDto : IReturn<EventsResponse>
    {
    }

}
