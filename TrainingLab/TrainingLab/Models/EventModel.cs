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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public List<string> Panelists { get; set; }
        public int Attendee { get; set; }
    }
}
