using AFA.ServiceInterface;
using AFA.ServiceModel;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFA.Web.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly JsonServiceClient _client = new JsonServiceClient("http://localhost:60782");

        public ActionResult Index()
        {
            var response = _client.Get(new Organizations());
            return View(response);
        }

        public ActionResult Add()
        {
            var org = new Organization();
            return View(org);
        }

        [HttpPost]
        public ActionResult Add(Organization org)
        {
            try
            {
                //var result = _client.Post<HttpResult>("/organizations", org);
                _client.Post(org);
                return RedirectToAction("Index", "Organizations");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var org = _client.Get(new Organization { Id = id });
            return View(org.Organization);
        }

        [HttpPost]
        public ActionResult Edit(Organization org)
        {
            try
            {
                _client.Put(org);
                return RedirectToAction("Index", "Organizations");
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
                _client.Delete(new Organization { Id = id });
                return RedirectToAction("Index", "Organizations");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }
    }
}
