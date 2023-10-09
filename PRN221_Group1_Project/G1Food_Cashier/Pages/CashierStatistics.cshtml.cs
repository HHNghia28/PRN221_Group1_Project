using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace G1Food_Cashier.Pages
{
    [Authorize]
    public class CashierStatisticsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
