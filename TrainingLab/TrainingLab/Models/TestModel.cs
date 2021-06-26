using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class TestModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public float MinimumScore { get; set; }
        public int LevelName{ get; set; }
        public string Description { get; set; }
    }
}
