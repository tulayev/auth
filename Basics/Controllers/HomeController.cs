using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
        
        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecurePolicy()
        {
            ViewBag.Text = "Policy";
            return View("Secure");
        }

        public IActionResult Auth()
        {
            var defaultClaims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, "John"),
                new Claim(ClaimTypes.Email, "john@anonymous.com"),
                new Claim(ClaimTypes.DateOfBirth, "12/12/2012")
            };

            var licenseClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim("DrivingLicense", "BC")
            };

            var defaultIdentity = new ClaimsIdentity(defaultClaims, "Default Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "License Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { defaultIdentity, licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
