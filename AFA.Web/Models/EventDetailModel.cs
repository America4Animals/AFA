using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AFA.Web.Models
{
    public class EventDetailModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; }
        public string TimeDescription { get; set; }
        public string DayDescription { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
    }
}