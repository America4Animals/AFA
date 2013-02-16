using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace AFA.ServiceModel
{
    /// <summary>
    /// Create your own strong-typed Custom AuthUserSession where you can add additional AuthUserSession 
    /// fields required for your application. The base class is automatically populated with 
    /// User Data as and when they authenticate with your application. 
    /// </summary>
    public class CustomUserSession : AuthUserSession
    {
        public string CustomFoo { get; set; }

        public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            base.OnAuthenticated(authService, session, tokens, authInfo);
            CustomFoo = "SOMETHING CUSTOM";

            //if (session.Email == null)
            //{
            //    session.Email = session.Email ?? tokens.Email;
            //    session.UserAuthName = session.UserAuthName ?? session.Email;
            //    session.UserName = session.UserName ?? session.Email;

            //    using (var db = authService.TryResolve<IDbConnectionFactory>().Open())
            //    {
            //        int userAuthId = Convert.ToInt32(session.UserAuthId);

            //        var userAuth = db.Select<UserAuth>(ua => ua.Id == userAuthId).FirstOrDefault();
            //        if (userAuth != null && (userAuth.Email == null || userAuth.UserName == null))
            //        {
            //            userAuth.Email = userAuth.Email ?? session.Email;
            //            userAuth.UserName = userAuth.UserName ?? session.Email;
            //            db.Update<UserAuth>(userAuth);
            //        }
            //    }
            //}
            //else if (session.UserAuthName == null)
            //{
            //    session.UserAuthName = session.Email;
            //}

            //session.Email = session.Email ?? tokens.Email;
            //session.UserAuthName = session.UserAuthName ?? session.Email;
            //session.UserName = session.UserName ?? session.Email;

            //Populate all matching fields from this session to your own custom User table
            var user = session.TranslateTo<User>();
            //user.Id = int.Parse(session.UserAuthId);

            foreach (var authToken in session.ProviderOAuthAccess)
            {
                if (authToken.Provider == FacebookAuthProvider.Name)
                {
                    user.FacebookName = authToken.DisplayName;
                    user.FacebookFirstName = authToken.FirstName;
                    user.FacebookLastName = authToken.LastName;
                    user.FacebookEmail = authToken.Email;

                    user.Email = authToken.Email;
                    user.FirstName = authToken.FirstName;
                    user.LastName = authToken.LastName;
                }
                //else if (authToken.Provider == TwitterAuthProvider.Name)
                //{
                //    user.TwitterName = authToken.DisplayName;
                //}
                //else if (authToken.Provider == GoogleOpenIdOAuthProvider.Name)
                //{
                //    user.GoogleUserId = authToken.UserId;
                //    user.GoogleFullName = authToken.FullName;
                //    user.GoogleEmail = authToken.Email;
                //}
                //else if (authToken.Provider == YahooOpenIdOAuthProvider.Name)
                //{
                //    user.YahooUserId = authToken.UserId;
                //    user.YahooFullName = authToken.FullName;
                //    user.YahooEmail = authToken.Email;
                //}
            }



            //if (AppHost.AppConfig.AdminUserNames.Contains(session.Email)
            //    && !session.HasRole(RoleNames.Admin))
            //{
            //    using (var assignRoles = authService.ResolveService<AssignRolesService>())
            //    {
            //        assignRoles.Post(new AssignRoles
            //        {
            //            UserName = session.UserAuthName,
            //            Roles = { RoleNames.Admin }
            //        });
            //    }
            //}

            //Resolve the DbFactory from the IOC and persist the user info
            //authService.TryResolve<IDbConnectionFactory>().Run(db => db.Save(user));
            using (var db = authService.TryResolve<IDbConnectionFactory>().Open())
            {
                //Update (if exists) or insert populated data into 'User'
                var existingUser = db.Select<User>(u => u.Email == user.Email).FirstOrDefault();
                if (existingUser != null)
                {
                    user.Id = existingUser.Id;
                }
                else
                {
                    user.CreatedAt = DateTime.Now;
                }

                user.LastLoginDate = DateTime.Now;
                db.Save(user);
            }

        }
    }
}