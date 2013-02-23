using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Mappers;
using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using ServiceStack.ServiceClient.Web;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class CrueltySpotsController : Controller
    {
        public ICrueltySpotService CrueltySpotService { get; set; }
        public ICrueltySpotsService CrueltySpotsService { get; set; }
        public IStateProvincesService StateProvincesService { get; set; }
        public ICrueltySpotCategoriesService CrueltySpotCategoriesService { get; set; }

        public CrueltySpotsController(
            ICrueltySpotService crueltySpotService,
            ICrueltySpotsService crueltySpotsService,
            IStateProvincesService stateProvincesService,
            ICrueltySpotCategoriesService crueltySpotCategoriesService)
        {
            var currentRequestContext = System.Web.HttpContext.Current.ToRequestContext();

            crueltySpotService.RequestContext = currentRequestContext;
            CrueltySpotService = crueltySpotService;

            crueltySpotsService.RequestContext = currentRequestContext;
            CrueltySpotsService = crueltySpotsService;

            stateProvincesService.RequestContext = currentRequestContext;
            StateProvincesService = stateProvincesService;

            crueltySpotCategoriesService.RequestContext = currentRequestContext;
            CrueltySpotCategoriesService = crueltySpotCategoriesService;
        }

        public ActionResult Index()
        {
            var crueltySpots = CrueltySpotsService.Get(new CrueltySpotsDto()).CrueltySpots;
            var model = crueltySpots.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var crueltySpot = CrueltySpotService.Get(new CrueltySpotDto { Id = id }).CrueltySpot;
            var model = crueltySpot.ToDetailModel();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new CrueltySpotDetailModel();
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            var allCategories = CrueltySpotCategoriesService.Get(new CrueltySpotCategories()).CrueltySpotCategories;
            model.AllCrueltySpotCategories = new SelectList(allCategories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(CrueltySpotDetailModel model)
        {
            try
            {
                var crueltySpot = model.ToEntity();
                CrueltySpotService.Post(crueltySpot);
                return RedirectToAction("Index", "CrueltySpots");

            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var crueltySpot = CrueltySpotService.Get(new CrueltySpotDto { Id = id }).CrueltySpot;
            var model = crueltySpot.ToDetailModel();
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            var allCategories = CrueltySpotCategoriesService.Get(new CrueltySpotCategories()).CrueltySpotCategories;
            model.AllCrueltySpotCategories = new SelectList(allCategories, "Id", "Name", model.CrueltySpotCategoryId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CrueltySpotDetailModel model)
        {
            try
            {
                var crueltySpot = model.ToEntity();
                CrueltySpotService.Put(crueltySpot);
                return RedirectToAction("Index", "CrueltySpots");
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
                CrueltySpotService.Delete(new CrueltySpotDto { Id = id });
                return RedirectToAction("Index", "CrueltySpots");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

    }
}
