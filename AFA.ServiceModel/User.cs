using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string City { get; set; }
        [References(typeof(StateProvince))]
        public int StateProvinceId { get; set; }
        [Required]
        [Index(Unique = true)]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // ToDo: Add Photo
    }

    
}
