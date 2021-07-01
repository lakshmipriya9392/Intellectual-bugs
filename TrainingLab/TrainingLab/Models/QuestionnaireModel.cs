using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class QuestionnaireModel
    {
        public int questionId { get; set; }       
        public string question { get; set; }
        public OptionModel optionList { get; set; }
        public string answer { get; set; }
        public string typeOfQuestion { get; set; }
        public int testId { get; set; }
    }
}
