using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingLab.Models
{
    public class UserModel
    {
        public string emailId { get; set; }
        public string name { get; set; }
        public string password { get; set; }
       
    }
}
