using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class TopicModel
    {
        public int topicId { get; set; }
        public string topicName { get; set; }
        public string videoURL { get; set; }
        public string notesURL { get; set; }
        public int chapterId { get; set; }
    }
}
