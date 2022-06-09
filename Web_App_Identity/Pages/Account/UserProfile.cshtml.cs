using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web_App_Identity.Data.Account;
using Web_App_Identity.ViewModels;

namespace Web_App_Identity.Pages.Account
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; }
        
        [BindProperty]
        public string SuccessMessage { get; set; }

        public UserProfileModel(UserManager<User> userManager)
        {
            _userManager = userManager;
            UserProfile = new UserProfileViewModel();
            SuccessMessage = String.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SuccessMessage = String.Empty;

            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

            UserProfile.Email = User.Identity.Name;
            UserProfile.Department = departmentClaim?.Value;
            UserProfile.Position = positionClaim?.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

                await _userManager.ReplaceClaimAsync(user, departmentClaim, new Claim(departmentClaim.Type, UserProfile.Department));
                await _userManager.ReplaceClaimAsync(user, positionClaim, new Claim(positionClaim.Type, UserProfile.Position));
            }
            catch
            {
                ModelState.AddModelError("UserProfile", "Error occured when saving user profile.");
            }

            SuccessMessage = "The user profile is saved successfully!";

            return Page();
        }

        private async Task<(User, Claim, Claim)> GetUserInfoAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var claims = await _userManager.GetClaimsAsync(user);
            var departmentClaim = claims.FirstOrDefault(c => c.Type == "Department");
            var positionClaim = claims.FirstOrDefault(c => c.Type == "Position");

            return (user, departmentClaim, positionClaim);
        }
    }
}
