using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace AFA.ServiceHostAndWeb.Helpers
{
    //Provide extra validation for the registration process
    public class CustomRegistrationValidator : RegistrationValidator
    {
        public CustomRegistrationValidator()
        {
            RuleSet(ApplyTo.Post, () => RuleFor(x => x.DisplayName).NotEmpty());
        }
    }
}