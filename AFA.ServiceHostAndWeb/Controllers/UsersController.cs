using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Mappers;
using AFA.ServiceHostAndWeb.Models;
using AFA.ServiceInterface;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class UsersController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public IUsersService UsersService { get; set; }
        public IOrganizationsService OrganizationsService { get; set; }
        public IStateProvincesService StateProvincesService { get; set; }

        public UsersController(
            IUserService userService,
            IUsersService usersService,
            IOrganizationsService organizationsService,
            IStateProvincesService stateProvincesService)
        {
            var currentRequestContext = System.Web.HttpContext.Current.ToRequestContext();

            userService.RequestContext = currentRequestContext;
            UserService = userService;

            usersService.RequestContext = currentRequestContext;
            UsersService = usersService;

            organizationsService.RequestContext = currentRequestContext;
            OrganizationsService = organizationsService;

            stateProvincesService.RequestContext = currentRequestContext;
            StateProvincesService = stateProvincesService;
        }

        public ActionResult Index()
        {
            var users = UsersService.Get(new UsersDto()).Users;
            var model = users.Select(o => o.ToModel()).ToList();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var user = UserService.Get(new UserDto { Id = id }).User;
            var model = user.ToDetailModel();

            // have this here for testing
            var allOrgs = OrganizationsService.Get(new OrganizationsDto()).Organizations;
            var myOrgs = UserService.Get(new UserOrganizations { UserId = id }).Organizations;
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
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(UserDetailModel model)
        {
            try
            {
                var user = model.ToEntity();
                UserService.Post(user);
                return RedirectToAction("Index", "Users");
            }
            catch (WebServiceException exception)
            {
                throw;
            }
        }

        public ActionResult Edit(int id)
        {
            var user = UserService.Get(new UserDto { Id = id }).User;
            var model = user.ToDetailModel();
            var allStateProvinces = StateProvincesService.Get(new StateProvinces()).StateProvinces;
            model.AllStateProvinces = new SelectList(allStateProvinces, "Id", "Name", model.StateProvinceId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserDetailModel model)
        {
            try
            {
                var user = model.ToEntity();
                UserService.Put(user);
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
                UserService.Delete(new UserDto { Id = id });
                return RedirectToAction("Index", "Users");
            }
            catch (WebServiceException)
            {
                throw;
            }
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

            UserService.Post(userOrganizationAction);

            return RedirectToAction("Details", "Users", new { id = userId });
        }

    }
}
