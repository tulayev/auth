using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "admin_policy")]
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
