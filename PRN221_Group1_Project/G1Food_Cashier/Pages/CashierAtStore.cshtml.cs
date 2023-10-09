using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace G1Food_Cashier.Pages.Shared
{
    [Authorize]
    public class CashierAtSroreModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
