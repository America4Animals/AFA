using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceInterface.Mappers;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class EventService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /events/{Id}
        /// </summary>
        public object Get(EventDto request)
        {
            var query = string.Format("select e.*, ec.Id as EventCategoryId, ec.Name as EventCategoryName " +
                "from Event e " +
                "left join EventCategory ec " +
                "on e.EventCategoryId = ec.Id " +
                "where e.Id = {0}", request.Id);

            var eventDto = Db.Select<EventDto>(query).FirstOrDefault();

            return new EventResponse
            {
                Event = eventDto
            };
        }

        public object Post(EventDto request)
        {
            var eventEntity = request.ToEntity();
            eventEntity.CreatedAt = DateTime.Now;
            Db.Insert(eventEntity);

            return new EventResponse { Event = new EventDto() };
        }

        public object Put(EventDto request)
        {
            var eventEntity = request.ToEntity();
            Db.Update(eventEntity);

            return new EventResponse { Event = new EventDto() };
        }

        public object Delete(EventDto request)
        {
            Db.DeleteById<Event>(request.Id);
            return new EventResponse { Event = new EventDto() };
        }
    }

    /// <summary>
    /// Returns a list of events
    /// </summary>
    public class EventsService : ServiceStack.ServiceInterface.Service
    {
        // GET /events
        public object Get(EventsDto request)
        {
            var query = "select e.*, ec.Id as EventCategoryId, ec.Name as EventCategoryName " +
                        "from Event e " +
                        "left join EventCategory ec " +
                        "on e.EventCategoryId = ec.Id";

            List<EventDto> events = Db.Select<EventDto>(query);

            return new EventsResponse
            {
                Events = events
            };
        }

        // GET /events/upcoming
        public object Get(UpcomingEventsDto request)
        {
            var query = String.Format("select e.*, ec.Id as EventCategoryId, ec.Name as EventCategoryName " +
                        "from Event e " +
                        "left join EventCategory ec " +
                        "on e.EventCategoryId = ec.Id " +
                        "where StartDateTime >= '{0}'", DateTime.Now);

            List<EventDto> events = Db.Select<EventDto>(query);

            return new EventsResponse
            {
                Events = events
            };
        }
    }

    
}
