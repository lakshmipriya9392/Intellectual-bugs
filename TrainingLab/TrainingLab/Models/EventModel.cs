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
        public int Attendee { get; set; }
        public string imageURL { get; set; }
        public EventAttendeeModel attendeeModel { get; set; }
    }
}
