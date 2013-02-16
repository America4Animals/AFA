using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Models
{
    public class OrganizationAlliesModel
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<UserDetailModel> Users { get; set; }
    }
}