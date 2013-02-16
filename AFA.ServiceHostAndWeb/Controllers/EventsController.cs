using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Helpers;
using AFA.ServiceHostAndWeb.Mappers;
using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using AFA.ServiceHostAndWeb.Controllers;
using AFA.ServiceModel;
using ControllerBase = AFA.ServiceHostAndWeb.Controllers.ControllerBase;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class EventsController : ControllerBase
    {
        public IEventCategoriesService EventCategoriesService { get; set; }
        public IEventsService EventsService { get; set; }
        public IEventService EventService { get; set; }

        public EventsController(
            IEventsService eventsService,
            IEventService eventService,
            IEventCategoriesService eventCategoriesService)
        {
            var currentRequestContext = System.Web.HttpContext.Current.ToRequestContext();

            eventsService.RequestContext = currentRequestContext;
            EventsService = eventsService;

            eventService.RequestContext = currentRequestContext;
            EventService = eventService;

            eventCategoriesService.RequestContext = currentRequestContext;
            EventCategoriesService = eventCategoriesService;

        }

        public ActionResult Index()
        {
            var events = EventsService.Get(new EventsDto()).Events;
            var model = events.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var eventDto = EventService.Get(new EventDto { Id = id }).Event;
            var model = eventDto.ToDetailModel();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new EventDetailModel();
            model.StartDateTime = DateTime.Now;
            var allCategories = EventCategoriesService.Get(new EventCategories()).EventCategories;
            model.AllEventCategories = new SelectList(allCategories, "Id", "Name");
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

                EventService.Post(eventEntity);
                return RedirectToAction("Index", "Events");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var eventDto = EventService.Get(new EventDto { Id = id }).Event;
            var model = eventDto.ToDetailModel();
            var allCategories = EventCategoriesService.Get(new EventCategories()).EventCategories;
            model.AllEventCategories = new SelectList(allCategories, "Id", "Name");
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

                EventService.Put(eventEntity);
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
                EventService.Delete(new EventDto { Id = id });
                return RedirectToAction("Index", "Events");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }
    }
}