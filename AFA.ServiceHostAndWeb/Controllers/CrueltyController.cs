using AFA.ServiceHostAndWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using AFA.ServiceHostAndWeb.App_Start;
using AFA.ServiceHostAndWeb.Helpers.Enums;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class CrueltyController : ControllerBase
    {
        public ICrueltySpotService CrueltySpotService { get; set; }
        public ICrueltySpotsService CrueltySpotsService { get; set; }
        public IStateProvincesService StateProvincesService { get; set; }
        public ICrueltySpotCategoriesService CrueltySpotCategoriesService { get; set; }

        public CrueltyController(
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

        public ActionResult Report()
        {
            var model = new ReportCrueltyModel();
            var allCategories = CrueltySpotCategoriesService.Get(new CrueltySpotCategories()).CrueltySpotCategories;
            model.AllCrueltySpotCategories =
                allCategories.Select(csc => new KeyValuePair<int, string>(csc.Id, csc.Name)).ToList();
            model.AllCrueltySpotCategories.Insert(0, new KeyValuePair<int, string>(0, "-- Please Select --"));
            ViewBag.GoogleApiKey = AppHost.AppConfig.GoogleApiKey;
            return View(model);
        }

        [HttpPost]
        public JsonResult Report(ReportCrueltyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Keys.SelectMany(key => this.ModelState[key].Errors) });
            }

            var crueltySpot = new CrueltySpotDto();
            crueltySpot.Name = model.CrueltySpotPlace.Name;
            crueltySpot.Address = model.ExtractAddress();
            crueltySpot.City = model.ExtractCity();
            crueltySpot.StateProvinceAbbreviation = model.ExtractStateAbbreviation();
            crueltySpot.Zipcode = model.ExtractZipcode();
            crueltySpot.PhoneNumber = model.CrueltySpotPlace.formatted_phone_number;
            crueltySpot.WebpageUrl = model.CrueltySpotPlace.Website;
            crueltySpot.Latitude = model.CrueltySpotPlace.Lat;
            crueltySpot.Longitude = model.CrueltySpotPlace.Lng;
            crueltySpot.CrueltySpotCategoryId = model.CrueltySpotCategoryId;
            CrueltySpotService.Post(crueltySpot);

            var redirectUrl = Url.Action("Index", "CrueltySpots");
            return Json(new { success = true, redirectUrl = redirectUrl, statusCode = StatusCode.AddCrueltySpotSuccess });
        }

    }
}
