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
   
    public class User
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Index(Unique = true)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    
}
