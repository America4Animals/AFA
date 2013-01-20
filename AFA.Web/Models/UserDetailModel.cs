using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFA.Web.Models
{
    public class UserDetailModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string City { get; set; }
        public int StateProvinceId { get; set; }
        public string StateProvinceAbbreviation { get; set; }

        public string DisplayName { get; set; }
        public string CityAndState { get; set; }

        public SelectList AllStateProvinces { get; set; }

        // orgId, orgName, following, actionText
        public List<Tuple<int, string, bool, string>> Organizations { get; set; }

    }
}