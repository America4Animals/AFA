using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using ServiceStack;
using ServiceStack.ServiceClient.Web;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class CrueltySpotCategoriesController : ControllerBase
    {
        public ICrueltySpotCategoriesService CrueltySpotCategoriesService { get; set; }
        public ICrueltySpotCategoryService CrueltySpotCategoryService { get; set; }

        public CrueltySpotCategoriesController(
            ICrueltySpotCategoriesService crueltySpotCategoriesService,
            ICrueltySpotCategoryService crueltySpotCategoryService)
        {
            var requestContext = System.Web.HttpContext.Current.ToRequestContext();

            crueltySpotCategoriesService.RequestContext = requestContext;
            CrueltySpotCategoriesService = crueltySpotCategoriesService;

            crueltySpotCategoryService.RequestContext = requestContext;
            CrueltySpotCategoryService = crueltySpotCategoryService;
        }

        public ActionResult Index()
        {
            var response = CrueltySpotCategoriesService.Get(new CrueltySpotCategories());
            return View(response);
        }

        public ActionResult Add()
        {
            var crueltySpotCategory = new CrueltySpotCategory();
            return View(crueltySpotCategory);
        }

        [HttpPost]
        public ActionResult Add(CrueltySpotCategory crueltySpotCategory)
        {
            try
            {
                CrueltySpotCategoryService.Post(crueltySpotCategory);
                return RedirectToAction("Index", "CrueltySpotCategories");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var crueltySpotCategory = CrueltySpotCategoryService.Get(new CrueltySpotCategory { Id = id }).CrueltySpotCategory;
            return View(crueltySpotCategory);
        }

        [HttpPost]
        public ActionResult Edit(CrueltySpotCategory crueltySpotCategory)
        {
            try
            {
                CrueltySpotCategoryService.Put(crueltySpotCategory);
                return RedirectToAction("Index", "CrueltySpotCategories");
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
                CrueltySpotCategoryService.Delete(new CrueltySpotCategory { Id = id });
                return RedirectToAction("Index", "CrueltySpotCategories");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

    }
}
