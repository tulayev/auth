using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web_App_Identity.Data.Account;
using Web_App_Identity.Services;
using Web_App_Identity.ViewModels;

namespace Web_App_Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create the user
            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var claimDepartment = new Claim("Department", RegisterViewModel.Department);
            var claimPosition = new Claim("Position", RegisterViewModel.Position);

            var result = await _userManager.CreateAsync(user, RegisterViewModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, claimDepartment);
                await _userManager.AddClaimAsync(user, claimPosition);

                string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string confirmationLink = Url.PageLink(pageName: "/account/confirmemail", values: new { userId = user.Id, token = confirmationToken });

                await _emailService.SendAsync(user.Email, "Please confirm your email", $"Please click on this link to confirm your email address: {confirmationLink}");

                return RedirectToPage("/account/login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
        }
    }
}
