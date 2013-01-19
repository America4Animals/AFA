using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceModel;
using AFA.Web.Helpers;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AFA.Web.Controllers
{
    public class OrganizationCategoriesController : RestController
    {
        public OrganizationCategoriesController(IRestClient client) : base(client) { }

        public ActionResult Index()
        {
            var response = _client.Get(new OrganizationCategories());
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
                _client.Post(orgCategory);
                return RedirectToAction("Index", "OrganizationCategories");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var orgCategory = _client.Get(new OrganizationCategory { Id = id });
            return View(orgCategory.OrganizationCategory);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationCategory orgCategory)
        {
            try
            {
                _client.Put(orgCategory);
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
                _client.Delete(new OrganizationCategory { Id = id });
                return RedirectToAction("Index", "OrganizationCategories");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

    }
}
