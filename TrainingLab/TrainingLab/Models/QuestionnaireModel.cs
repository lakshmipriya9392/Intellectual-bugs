using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class QuestionnaireModel
    {
        public int QuestionId { get; set; }       
        public string Question { get; set; }
        public string OptionList { get; set; }
        public string Answer { get; set; }
    }
}
