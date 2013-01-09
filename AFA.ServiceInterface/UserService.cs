using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceInterface.Mappers;
using AFA.ServiceModel;
using AFA.ServiceModel.DTOs;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class UserService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /users/{Id}
        /// </summary>
        public object Get(UserDto request)
        {
            var query = string.Format("select * from Users where Id = {0}", request.Id);
            var user = Db.Select<UserDto>(query).FirstOrDefault();

            return new UserResponse
            {
                User = user
            };
        }

        public object Post(UserDto request)
        {
            var user = request.ToEntity();
            user.CreatedAt = DateTime.Now;
            Db.Insert(user);

            return new UserResponse { User = new UserDto() };
        }

        public object Put(UserDto request)
        {
            var user = request.ToEntity();
            Db.Update(user);

            return new UserResponse { User = new UserDto() };
        }

        public object Delete(UserDto request)
        {
            Db.DeleteById<User>(request.Id);
            return new UserResponse { User = new UserDto() };
        }

        public object Post(UserOrganizationAction request)
        {
            const string FollowActionValue = "follow";
            var action = request.Action ?? FollowActionValue;
            var userId = request.UserId;
            var orgId = request.OrganizationId;

            Db.Delete<OrganizationAlly>(oa => oa.UserId == userId && oa.OrganizationId == orgId);

            if (action.ToLower() == FollowActionValue)
            {
                var organizationAlly = new OrganizationAlly
                                           {
                                               UserId = userId,
                                               OrganizationId = orgId
                                           };

                Db.Insert(organizationAlly);
            }

            return new UserOrganizationActionResponse();
        }
    }

    /// <summary>
    /// GET /users
    /// GET /users/organization/{OrganizationId}
    /// Returns a list of organizations
    /// </summary>
    public class UsersService : ServiceStack.ServiceInterface.Service
    {
        public object Get(UsersDto request)
        {
            if (request.OrganizationId.HasValue)
            {
                throw new NotImplementedException();
            }

            var users = Db.Select<UserDto>("select * from User");

            return new UsersResponse
            {
                Users = users
            };
        }
    }
}
