using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Numerics;

namespace G1Food_User.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("phone");
            Response.Cookies.Delete("name");
            Response.Cookies.Delete("address");
            Response.Cookies.Delete("password");
            Response.Cookies.Delete("isRegistered");
            return RedirectToPage("/Login");
        }
    }
}
