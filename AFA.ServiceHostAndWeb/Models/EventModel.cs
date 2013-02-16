using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFA.ServiceHostAndWeb.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string TimeDescription { get; set; }
        public string DayDescription { get; set; }
    }
}