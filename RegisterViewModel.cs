
using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address, such as name@example.com.")]
        [Display(Name = "Email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please create a password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please re-enter your password to confirm it.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The confirmation password does not match the password you entered.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
