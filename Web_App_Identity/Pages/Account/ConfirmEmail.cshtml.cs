using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App_Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }

        private readonly UserManager<IdentityUser> _userManager; 

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    Message = "Email address is successfully confirmed, now you can try to login";
                    return Page();
                }
            }

            Message = "Failed to validate email!";
            return Page();
        }
    }
}
