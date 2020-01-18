using System;
using System.ComponentModel.DataAnnotations;
namespace Sources5.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a project name - max 100 characters")]
        [MaxLength(100)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [UIHint("Date")]  // date without time component
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set;}

        [UIHint("Date")]  // date without time component
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public UserInfo UserInfo { get; set; }

        public int UserInfoId { get; set; }
    }
}
