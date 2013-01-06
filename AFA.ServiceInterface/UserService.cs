using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFA.ServiceModel;
using ServiceStack.OrmLite;

namespace AFA.ServiceInterface
{
    public class UserService : ServiceStack.ServiceInterface.Service
    {
        /// <summary>
        /// GET /users/{Id}
        /// </summary>
        public object Get(User user)
        {
            return new UserResponse
            {
                User = Db.Id<User>(user.Id)
            };
        }

        public object Post(User user)
        {
            Db.Insert(user);
            return new UserResponse { User = new User() };
        }

        public object Put(User user)
        {
            Db.Update(user);
            return new UserResponse { User = new User() };
        }

        public object Delete(User user)
        {
            Db.DeleteById<User>(user.Id);
            return new UserResponse { User = new User() };
        }
    }
}
