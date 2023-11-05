using Azure.Core;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using G1FOODLibrary.Entities;
using Azure;

namespace G1Food_User.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _client;
        private readonly string _authApiUrl;

        public IEnumerable<CartResponse> Carts { get; private set; }
        public int cartQuantity;

        [BindProperty]
        public string ResponseMessage { get; set; }

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
                AccountResponse account = null;
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
                            account = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);

                    

                        var claims = new List<Claim>
                            {
                                new Claim("ID", account.Id.ToString()),
                                new Claim("Name", account.Name),
                                new Claim("Email", account.Email),
                                new Claim("Phone", account.Phone),
                                new Claim("Address", account.AddressDetail),
                                new Claim("Role", account.Role),
                                new Claim("Token", account.Token)
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

                        return RedirectToPage("/Index");
                    } else {
                            ResponseMessage = "Email hoặc mật khẩu không đúng!";
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
            return Page();
        }
    }
}
