using System.Configuration;
using AFA.ServiceHostAndWeb.Mappers;
using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Service;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class OrganizationsController : ControllerBase
    {
        public IOrganizationService OrganizationService { get; set; }
        public IOrganizationsService OrganizationsService { get; set; }
        public IStateProvincesService StateProvincesService { get; set; }
        public IOrganizationCategoriesService OrganizationCategoriesService { get; set; }

        public OrganizationsController(
            IOrganizationService organizationService,
            IOrganizationsService organizationsService,
            IStateProvincesService stateProvincesService,
            IOrganizationCategoriesService organizationCategoriesService)
        {
            var currentRequestContext = System.Web.HttpContext.Current.ToRequestContext();

            organizationService.RequestContext = currentRequestContext;
            OrganizationService = organizationService;

            organizationsService.RequestContext = currentRequestContext;
            OrganizationsService = organizationsService;

            stateProvincesService.RequestContext = currentRequestContext;
            StateProvincesService = stateProvincesService;

            organizationCategoriesService.RequestContext = currentRequestContext;
            OrganizationCategoriesService = organizationCategoriesService;
        }

        public ActionResult Index()
        {
            var orgs = OrganizationsService.Get(new OrganizationsDto()).Organizations;
            var model = orgs.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var org = OrganizationService.Get(new OrganizationDto { Id = id }).Organization;
            var model = org.ToDetailModel();
            return View(model);
        }

        public ActionResult Add()
        {
            var model = new OrganizationDetailModel();
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            var allCategories = OrganizationCategoriesService.Get(new OrganizationCategories()).OrganizationCategories;
            model.AllOrganizationCategories = new SelectList(allCategories, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(OrganizationDetailModel model)
        {
            try
            {
                var org = model.ToEntity();
                OrganizationService.Post(org);
                return RedirectToAction("Index", "Organizations");

            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var org = OrganizationService.Get(new OrganizationDto { Id = id }).Organization;
            var model = org.ToDetailModel();
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            var allCategories = OrganizationCategoriesService.Get(new OrganizationCategories()).OrganizationCategories;
            model.AllOrganizationCategories = new SelectList(allCategories, "Id", "Name", model.OrganizationCategoryId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationDetailModel model)
        {
            try
            {
                var org = model.ToEntity();
                OrganizationService.Put(org);
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
                OrganizationService.Delete(new OrganizationDto { Id = id });
                return RedirectToAction("Index", "Organizations");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

        public ActionResult Users(int organizationId)
        {
            var organizationUsers = OrganizationService.Get(new OrganizationUsers { OrganizationId = organizationId }).Users;
            var organization = OrganizationService.Get(new OrganizationDto { Id = organizationId }).Organization;

            var model = new OrganizationAlliesModel();
            model.OrganizationId = organizationId;
            model.OrganizationName = organization.Name;
            model.Users = organizationUsers.Select(o => o.ToDetailModel()).ToList();

            return View(model);
        }
    }
}
