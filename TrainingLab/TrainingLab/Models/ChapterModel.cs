using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class ChapterModel
    {
        public int chapterId { get; set; }

        public string chapterName { get; set; }
        public List<TopicModel> topics { get; set; }
        public int courseId { get; set; }
    }
}
