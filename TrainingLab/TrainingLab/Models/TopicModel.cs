using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class TopicModel
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string VideoURL { get; set; }
        public string NotesURL { get; set; }
    }
}
