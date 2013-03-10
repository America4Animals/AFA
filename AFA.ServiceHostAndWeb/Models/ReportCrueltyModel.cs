using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AFA.ServiceHostAndWeb.Models
{
    public class ReportCrueltyModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please specify a valid Category")]
        public int CrueltySpotCategoryId { get; set; }
        [Required]
        public AddCrueltySpotPlaceModel CrueltySpotPlace { get; set; }

        public IList<KeyValuePair<int, string>> AllCrueltySpotCategories { get; set; }

        public string ExtractAddress()
        {
            var address = new StringBuilder();

            if (CrueltySpotPlace != null)
            {
                var addressComponents = CrueltySpotPlace.address_components;

                if (addressComponents != null)
                {
                    var streetNumberComponent =
                        addressComponents.FirstOrDefault(ac => ac.types.Contains("street_number"));
                    if (streetNumberComponent != null)
                    {
                        address.Append(streetNumberComponent.long_name);
                    }

                    var routeComponent = addressComponents.FirstOrDefault(ac => ac.types.Contains("route"));
                    if (routeComponent != null)
                    {
                        if (address.Length > 0)
                        {
                            address.Append(" ");
                        }

                        address.Append(routeComponent.long_name);
                    }
                }
            }

            return address.ToString();
        }

        public string ExtractCity()
        {
            string city = null;

            if (CrueltySpotPlace != null)
            {
                var addressComponents = CrueltySpotPlace.address_components;

                if (addressComponents != null)
                {
                    var cityComponent = addressComponents.FirstOrDefault(ac => ac.types.Contains("locality"));
                    if (cityComponent != null)
                    {
                        city = cityComponent.long_name;
                    }
                }
            }

            return city;
        }

        public string ExtractStateAbbreviation()
        {
            string state = null;

            if (CrueltySpotPlace != null)
            {
                var addressComponents = CrueltySpotPlace.address_components;

                if (addressComponents != null)
                {
                    var stateComponent = addressComponents.FirstOrDefault(ac => ac.types.Contains("administrative_area_level_1"));
                    if (stateComponent != null)
                    {
                        state = stateComponent.long_name;
                    }
                }
            }

            return state;
        }

        public string ExtractZipcode()
        {
            string zipcode = null;

            if (CrueltySpotPlace != null)
            {
                var addressComponents = CrueltySpotPlace.address_components;

                if (addressComponents != null)
                {
                    var zipcodeComponent = addressComponents.FirstOrDefault(ac => ac.types.Contains("postal_code"));
                    if (zipcodeComponent != null)
                    {
                        zipcode = zipcodeComponent.long_name;
                    }
                }
            }

            return zipcode;
        }
    }
}