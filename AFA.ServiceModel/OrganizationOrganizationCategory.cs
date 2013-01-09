using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace AFA.ServiceModel
{
    public class OrganizationOrganizationCategory
    {
        [AutoIncrement]
        public int Id { get; set; }
        [References(typeof(OrganizationCategory))]
        public int OrganizationCategoryId { get; set; }
        [References(typeof(Organization))]
        public int OrganizationId { get; set; }
    }
}
