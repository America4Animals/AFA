using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace AFA.ServiceModel
{
    public class Event
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        [Index(Unique = true)]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }

        public string OrganizerType { get; set; }
        [References(typeof(User))]
        public int UserId { get; set; }
        [References(typeof(Organization))]
        public int OrganizationId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
