using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class EventAttendeeModel
    {
        public List<string> Panelists { get; set; }
        public int eventId { get; set; }
        public string emailId { get; set; }
        public bool Attendee { get; set; }
    }
}
