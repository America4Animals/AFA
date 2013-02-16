using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Helpers;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class OrganizationCategoriesController : ControllerBase
    {
        public IOrganizationCategoriesService OrganizationCategoriesService { get; set; }
        public IOrganizationCategoryService OrganizationCategoryService { get; set; }

        public OrganizationCategoriesController(
            IOrganizationCategoriesService organizationCategoriesService,
            IOrganizationCategoryService organizationCategoryService)
        {
            organizationCategoriesService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
            OrganizationCategoriesService = organizationCategoriesService;

            organizationCategoryService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
            OrganizationCategoryService = organizationCategoryService;
        }

        public ActionResult Index()
        {
            var response = OrganizationCategoriesService.Get(new OrganizationCategories());
            return View(response);
        }

        public ActionResult Add()
        {
            var orgCategory = new OrganizationCategory();
            return View(orgCategory);
        }

        [HttpPost]
        public ActionResult Add(OrganizationCategory orgCategory)
        {
            try
            {
                OrganizationCategoryService.Post(orgCategory);
                return RedirectToAction("Index", "OrganizationCategories");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var orgCategory = OrganizationCategoryService.Get(new OrganizationCategory { Id = id }).OrganizationCategory;
            return View(orgCategory);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationCategory orgCategory)
        {
            try
            {
                OrganizationCategoryService.Put(orgCategory);
                return RedirectToAction("Index", "OrganizationCategories");
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
                OrganizationCategoryService.Delete(new OrganizationCategory { Id = id });
                return RedirectToAction("Index", "OrganizationCategories");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

    }
}
