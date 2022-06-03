using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "hr_manager_policy")]
    public class HRManagerModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
