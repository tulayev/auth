using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_App_Identity.Data.Account;
using Web_App_Identity.Services;
using Web_App_Identity.ViewModels;

namespace Web_App_Identity.Pages.Account
{
    public class LoginTwoFactorModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<User> _signInManager;

        [BindProperty]
        public EmailMFA EmailMFA { get; set; }

        public LoginTwoFactorModel(UserManager<User> userManager, IEmailService emailService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            EmailMFA = new EmailMFA();
        }

        public async Task OnGetAsync(string email, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);

            EmailMFA.SecurityCode = String.Empty;
            EmailMFA.RememberMe = rememberMe;

            string securityCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            await _emailService.SendAsync(email, "Web App Identity", $"Please use this code as the OTP: {securityCode}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.TwoFactorSignInAsync("Email", EmailMFA.SecurityCode, EmailMFA.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login2FA", "You are locked out.");
                }
                else
                {
                    ModelState.AddModelError("Login2FA", "Failed to login!");
                }

                return Page();
            }
        }
    }
}
