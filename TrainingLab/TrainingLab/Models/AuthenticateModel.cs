using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class AuthenticateModel
    {
        public string emailId { get; set; }
        public string token { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
    }
}
