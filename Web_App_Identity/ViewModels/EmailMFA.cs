using System.ComponentModel.DataAnnotations;

namespace Web_App_Identity.ViewModels
{
    public class EmailMFA
    {
        [Required]
        [Display(Name = "Security Code")]
        public string SecurityCode { get; set; }

        public bool RememberMe { get; set; }
    }
}
