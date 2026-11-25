using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email address or instructor ID.")]
        [StringLength(100, ErrorMessage = "Email address or instructor ID cannot be longer than 100 characters.")]
        [Display(Name = "Email address or instructor ID")]
        public string EmailOrId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
