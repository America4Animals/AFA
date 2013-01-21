using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceModel.DTOs;
using AFA.Web.Mappers;
using AFA.Web.Models;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AFA.Web.Controllers
{
    public class EventsController : RestController
    {
        public EventsController(IRestClient client) : base(client) { }

        public ActionResult Index()
        {
            var events = _client.Get(new EventsDto()).Events;
            var model = events.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var eventDto = _client.Get(new EventDto { Id = id }).Event;
            var model = eventDto.ToDetailModel();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new EventDetailModel();
            model.StartDateTime = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(EventDetailModel model)
        {
            try
            {
                var eventEntity = model.ToEntity();
                eventEntity.StartDateTime = new DateTime(
                    model.StartDateTime.Year, 
                    model.StartDateTime.Month,
                    model.StartDateTime.Day,
                    model.StartHour,
                    model.StartMinute,
                    0);

                _client.Post(eventEntity);
                return RedirectToAction("Index", "Events");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var eventDto = _client.Get(new EventDto { Id = id }).Event;
            var model = eventDto.ToDetailModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EventDetailModel model)
        {
            try
            {
                var eventEntity = model.ToEntity();

                eventEntity.StartDateTime = new DateTime(
                   model.StartDateTime.Year,
                   model.StartDateTime.Month,
                   model.StartDateTime.Day,
                   model.StartHour,
                   model.StartMinute,
                   0);

                _client.Put(eventEntity);
                return RedirectToAction("Index", "Events");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _client.Delete(new EventDto { Id = id });
                return RedirectToAction("Index", "Events");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }
    }
}