using System.Collections.Generic;

namespace Sources5.Models
{
    public class WelcomeTasksViewModel
    {
        public Project project { get; set; }
        public IEnumerable<ProjectTask> tasks { get; set; }
        public IEnumerable<Priority> priorities { get; set; }
        public string nametask { get; set; }
    }
}
