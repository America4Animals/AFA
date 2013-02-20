using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.ServiceHostAndWeb.Models;
using ServiceStack;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

namespace AFA.ServiceHostAndWeb.Controllers
{
    public class AccountController : ControllerBase
    {
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var authService = AppHostBase.Resolve<AuthService>();
                authService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
                var response = authService.Authenticate(new Auth
                {
                    UserName = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                });

                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var registerService = AppHostBase.Resolve<RegistrationService>();
                registerService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();           

                try
                {
                    // Attempt to register the user
                    registerService.Post(new Registration
                                             {
                                                 AutoLogin = true,
                                                 DisplayName = model.DisplayName,
                                                 Email = model.Email,
                                                 FirstName = model.FirstName,
                                                 LastName = model.LastName,
                                                 Password = model.Password,
                                                 UserName = model.Email
                                             });

                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    var errorMsg = ex.Message;
                    var error = ex.Errors.FirstOrDefault();
                    if (error != null && error.ErrorMessage != null)
                    {
                        errorMsg = error.ErrorMessage;
                    }

                    ModelState.AddModelError("", errorMsg);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
