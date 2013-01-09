using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFA.Web.Helpers;

namespace AFA.Web.Models
{
    public class OrganizationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public SelectList AllStateProvinces { get; set; }
        public IEnumerable<CheckboxItem> AllCategories { get; set; }
        public int OrganizationAlliesCount { get; set; }
    }
}