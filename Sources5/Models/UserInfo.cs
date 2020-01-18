using System.ComponentModel.DataAnnotations;

namespace Sources5.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Please enter a first name - max 100 characters")]
        [MaxLength(100)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name - max 100 characters")]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(12)]
        public string SIN { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
