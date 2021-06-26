using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class EventModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventURL { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Panelists { get; set; }
        public string Attendee { get; set; }
    }
}
