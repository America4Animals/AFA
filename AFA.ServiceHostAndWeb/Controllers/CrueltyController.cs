using AFA.ServiceHostAndWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using ServiceStack;
using AFA.ServiceHostAndWeb.App_Start;

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
            model.AllCrueltySpotCategories = new SelectList(allCategories, "Id", "Name");
            ViewBag.GoogleApiKey = AppHost.AppConfig.GoogleApiKey;
            return View(model);
        }

        [HttpPost]
        public ActionResult Report(ReportCrueltyModel model)
        {    
            throw new NotImplementedException();
        }

    }
}
