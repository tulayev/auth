using System.ComponentModel.DataAnnotations;

namespace Web_App_Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }
        
        [Required]
        public string Position { get; set; }
    }
}
