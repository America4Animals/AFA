using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFA.ServiceHostAndWeb.Models
{
    public class EventDetailModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int EventCategoryId { get; set; }
        public string EventCategoryName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; }
        public string TimeDescription { get; set; }
        public string DateGroupDescription { get; set; }
        public string DateDescription { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }

        public SelectList AllEventCategories { get; set; }
    }
}