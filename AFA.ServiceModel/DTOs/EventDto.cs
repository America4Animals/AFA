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
    public class EventDto : IReturn<EventResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        public DateTime StartDateTime { get; set; }

        public string TimeDescription {
            get { return StartDateTime.ToString("t"); }
        }

        public string DayDescription {
            get
            {

                var today = DateTime.Now;

                if (StartDateTime < today)
                {
                    return "Past";
                }

                var numDaysDiffernce = (StartDateTime.Date - today.Date).Days;

                if (numDaysDiffernce == 0)
                {
                    return "Today";
                }
                
                if (numDaysDiffernce == 1)
                {
                    return "Tomorrow";
                }
                
                if (numDaysDiffernce < 7)
                {
                    return StartDateTime.DayOfWeek.ToString();
                }

                return "Future";
            }

        }

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
