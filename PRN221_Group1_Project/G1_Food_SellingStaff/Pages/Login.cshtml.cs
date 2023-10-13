using G1_Food_SellingStaff.Pages;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1_Food_SellingStaff.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _client;
        private readonly string _authApiUrl;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }
        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration)
        {

            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _authApiUrl = configuration.GetValue<string>("APIEndpoint:Auth");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                try
                {
                    HttpResponseMessage response = await _client.PostAsJsonAsync($"{_authApiUrl}login", LoginRequest);
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        AccountResponse account = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);

                        if (account.RoleId == new Guid("D1DDB501-E7FA-4D50-9D1B-E2713C0A3B2D")
                            || account.RoleId == new Guid("B9E781F7-E1DD-416B-B4A6-7185D199B4AA"))
                        {

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, account.Email),
                                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                                new Claim(ClaimTypes.Role, account.Role),
                                new Claim("Token", account.Token),
                                new Claim("Name", account.Name)
                            };

                            var claimsIdentity = new ClaimsIdentity(
                                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = true
                            };

                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);

                            return RedirectToPage("/index");

                        } else
                        {
                            return RedirectToPage("/401");
                        }
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"HTTP request failed with error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                }
            }
            return Page();
        }
    }
}
