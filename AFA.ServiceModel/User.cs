using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AFA.ServiceModel
{
    [Route("/users/", "POST,PUT,DELETE")]
    [Route("/users/{Id}", "GET")]
    public class User : IReturn<UserResponse>
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Index(Unique = true)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class UserResponse : IHasResponseStatus
    {
        public User User { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
