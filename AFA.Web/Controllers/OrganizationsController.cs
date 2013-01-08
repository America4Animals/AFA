using System.Configuration;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.Web.Mappers;
using AFA.Web.Models;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.Web.Helpers;

namespace AFA.Web.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly JsonServiceClient _client = new JsonServiceClient(GlobalHelper.GetServiceUrl());

        public ActionResult Index()
        {
            var response = _client.Get(new Organizations());
            return View(response);
        }

        public ActionResult Add()
        {
            //var org = new Organization();
            //return View(org);

            var model = new OrganizationModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(OrganizationModel model)
        {
            try
            {
                //var result = _client.Post<HttpResult>("/organizations", org);
                //_client.Post(org);
                //return RedirectToAction("Index", "Organizations");
                
                var org = model.ToEntity();
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
            //var org = _client.Get(new Organization { Id = id });
            //return View(org.Organization);

            var org = _client.Get(new Organization { Id = id }).Organization;
            var model = org.ToModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationModel model)
        {
            try
            {
                //_client.Put(org);
                //return RedirectToAction("Index", "Organizations");

                var org = model.ToEntity();
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
