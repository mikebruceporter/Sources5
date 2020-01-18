using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sources5.Models
{
    public class TasksViewModel
    {
        // I need one project and a list of its tasks and Priorities
        // Used by Welcome.cshtml in Views/ProjectTask
        public Project project { get; set; }
        public IEnumerable<ProjectTask> tasks { get; set; }
        public IEnumerable<Priority> priorities { get; set; }
    }
}
