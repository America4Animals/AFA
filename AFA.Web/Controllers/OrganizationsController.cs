using System.Configuration;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AFA.Web.Mappers;
using AFA.Web.Models;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.Web.Helpers;
using ServiceStack.Service;

namespace AFA.Web.Controllers
{
    public class OrganizationsController : RestController
    {
        public OrganizationsController(IRestClient client) : base(client) {}

        public ActionResult Index(int? id)
        {
            var orgs = _client.Get(new OrganizationsDto()).Organizations;
            var model = orgs.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var org = _client.Get(new OrganizationDto { Id = id }).Organization;
            var model = org.ToDetailModel();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new OrganizationDetailModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            var allCategories = _client.Get(new OrganizationCategories()).OrganizationCategories;
            //model.AllCategories = allCategories.Select(c => new CheckboxItem
            //                                                    {
            //                                                        Id = c.Id.ToString(),
            //                                                        Name = c.Name
            //                                                    });
            model.AllOrganizationCategories = new SelectList(allCategories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(OrganizationDetailModel model)
        {
            try
            {              
                var org = model.ToEntity();
                //org.Categories = ExtractSelectedOrgCategories(model);
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
            var org = _client.Get(new OrganizationDto { Id = id }).Organization;
            var model = org.ToDetailModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            var allCategories = _client.Get(new OrganizationCategories()).OrganizationCategories;
            //model.AllCategories = allCategories.Select(c => new CheckboxItem
            //{
            //    Id = c.Id.ToString(),
            //    Name = c.Name,
            //    Checked = org.Categories.Any(oc => oc.Id == c.Id)
            //});
            model.AllOrganizationCategories = new SelectList(allCategories, "Id", "Name", model.OrganizationCategoryId);


            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationDetailModel model)
        {
            try
            {
                var org = model.ToEntity();
                //org.Categories = ExtractSelectedOrgCategories(model);
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
                _client.Delete(new OrganizationDto { Id = id });
                return RedirectToAction("Index", "Organizations");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

        public ActionResult Users(int organizationId)
        {
            var organizationUsers = _client.Get(new OrganizationUsers {OrganizationId = organizationId}).Users;
            var organization = _client.Get(new OrganizationDto { Id = organizationId }).Organization;

            var model = new OrganizationAlliesModel();
            model.OrganizationId = organizationId;
            model.OrganizationName = organization.Name;
            model.Users = organizationUsers.Select(o => o.ToDetailModel()).ToList();

            return View(model);
        }

        //private List<OrganizationCategory> ExtractSelectedOrgCategories(OrganizationDetailModel model)
        //{
        //    return model.AllCategories
        //            .Where(c => c.Checked)
        //            .Select(c => new OrganizationCategory
        //            {
        //                Id =
        //                    Convert.ToInt32(c.Id),
        //                Name = c.Name
        //            }).ToList();
        //}

    }
}
