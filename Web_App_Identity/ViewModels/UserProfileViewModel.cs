using System.ComponentModel.DataAnnotations;

namespace Web_App_Identity.ViewModels
{
    public class UserProfileViewModel
    {
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
