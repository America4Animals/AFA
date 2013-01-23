using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace AFA.ServiceModel
{
    public class OrganizationComment
    {
        [AutoIncrement]
        public int Id { get; set; }
        [References(typeof(Organization))]
        public int OrganizationId { get; set; }
        [References(typeof(User))]
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
