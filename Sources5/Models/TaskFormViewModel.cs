using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sources5.Models
{
    public class TaskFormViewModel
    {
        public Project project { get; set; }
        public ProjectTask task { get; set; }
        public IEnumerable<Priority> priorities { get; set; }
    }
}
