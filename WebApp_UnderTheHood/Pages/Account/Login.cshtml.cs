using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verify the credential
            if  (Credential.UserName == "admin" && Credential.Password == "password")
            {
                // Creating the security context
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mail.com"),
                    new Claim("Department", "HR"), // required claim for HR policy from Program.cs
                    new Claim("HRManager", "HR"), // required claim for HR policy from Program.cs
                    new Claim("Admin", "true"), // required claim for Admin policy from Program.cs
                    new Claim("EmploymentDate", "2022-06-01") // implement custom claim requirement for hr manager 
                };

                var identity = new ClaimsIdentity(claims, "custom_cookie_auth");

                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("custom_cookie_auth", principal, authProperties);

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
