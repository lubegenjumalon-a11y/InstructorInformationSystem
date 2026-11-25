using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "Please enter the full name.")]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please enter the address.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the designation.")]
        [StringLength(100, ErrorMessage = "Designation cannot be longer than 100 characters.")]
        [Display(Name = "Designation")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a gender.")]
        [StringLength(20)]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the contact number.")]
        [RegularExpression(@"^[0-9+\-\s()]{7,20}$",
            ErrorMessage = "Enter a valid contact number, such as 09123456789.")]
        [StringLength(20)]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the majorship.")]
        [StringLength(100, ErrorMessage = "Majorship cannot be longer than 100 characters.")]
        [Display(Name = "Majorship")]
        public string Majorship { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the bachelor degree.")]
        [StringLength(150, ErrorMessage = "Bachelor degree cannot be longer than 150 characters.")]
        [Display(Name = "Bachelor degree")]
        public string BachelorDegree { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the e-mail address.")]
        [EmailAddress(ErrorMessage = "Enter a valid e-mail address, such as name@example.com.")]
        [Display(Name = "E-mail address")]
        public string Email { get; set; } = string.Empty;
    }
}
