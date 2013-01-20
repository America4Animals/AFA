using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using AFA.Web.Mappers;
using AFA.Web.Models;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AFA.Web.Controllers
{
    public class UsersController : RestController
    {
        public UsersController(IRestClient client) : base(client) { }

        public ActionResult Index()
        {
            var users = _client.Get(new UsersDto()).Users;
            var model = users.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var user = _client.Get(new UserDto { Id = id }).User;
            var model = user.ToDetailModel();

            // have this here for testing
            var allOrgs = _client.Get(new OrganizationsDto()).Organizations;
            var myOrgs = _client.Get(new UserOrganizations {UserId = id}).Organizations;
            var myOrgIds = myOrgs.Select(mo => mo.Id).ToList();

            var organizations = new List<Tuple<int, string, bool, string>>();

            foreach (var org in allOrgs)
            {
                bool following = myOrgIds.Contains(org.Id);
                string actionText = following ? "unfollow" : "follow";
                organizations.Add(Tuple.Create(org.Id, org.Name, following, actionText));
            }

            model.Organizations = organizations;

            return View("Details", model);
        }

        public ActionResult Add()
        {
            var model = new UserDetailModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(UserDetailModel model)
        {
            try
            {
                var user = model.ToEntity();
                _client.Post(user);
                return RedirectToAction("Index", "Users");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var user = _client.Get(new UserDto { Id = id }).User;
            var model = user.ToDetailModel();
            var allStateProvinces = _client.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserDetailModel model)
        {
            try
            {
                var user = model.ToEntity();
                _client.Put(user);
                return RedirectToAction("Index", "Users");
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
                _client.Delete(new UserDto { Id = id });
                return RedirectToAction("Index", "Users");
            }
            catch (WebServiceException)
            {
                throw;
            }
        }

        public ActionResult Organization(int id)
        {
            var users = _client.Get(new UsersDto { OrganizationId = id }).Users;
            var organization = _client.Get(new OrganizationDto {Id = id}).Organization;

            var model = new OrganizationAlliesModel();
            model.OrganizationId = id;
            model.OrganizationName = organization.Name;
            model.Users = users.Select(o => o.ToDetailModel()).ToList();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult OrganizationAction(int userId, int organizationId, string actionText)
        {
            var userOrganizationAction = new UserOrganizationAction
                                             {
                                                 UserId = userId,
                                                 OrganizationId = organizationId,
                                                 Action = actionText
                                             };

            _client.Post(userOrganizationAction);

            return RedirectToAction("Details", "Users", new {id = userId});
        }

    }
}
