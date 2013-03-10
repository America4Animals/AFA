using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Models
{
    public class AddCrueltySpotPlaceModel
    {
        [Required]
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public string Website { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; } 
    }
}