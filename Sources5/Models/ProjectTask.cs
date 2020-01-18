using System.ComponentModel.DataAnnotations;

namespace Sources5.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Task Name")]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(1000)]
        public string Methodology { get; set; }

        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public Priority Priority { get; set; }
        
        [Required(ErrorMessage = "Please choose a priority")]
        [Range(1, 4, ErrorMessage = ("Please choose a priority from the list"))]
        public byte PriorityId { get; set; }
        // EF recognizes this one above as a foreign key ("Id")
        // the Range is hard-coded here - not the best.
    }
}
