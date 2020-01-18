using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sources5.Models
{
    public class WelcomeProjectViewModel
    {
        public IEnumerable<Project> projects { get; set; }
        public string nameproject { get; set; }
    }
}
