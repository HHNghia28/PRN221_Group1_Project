using Azure.Core;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
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

                        // Serialize the 'account' object to JSON
                        string accountJson = JsonSerializer.Serialize(account);

                        // Create a cookie with the 'account' JSON data
                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(1), // Set the expiration time for the cookie
                            HttpOnly = true, // Ensures the cookie is accessible only via HTTP
                            SameSite = SameSiteMode.None, // Adjust this as needed for your security requirements
                            Secure = true // Set to true if using HTTPS
                        };

                        Response.Cookies.Append("AccountCookie", accountJson, cookieOptions);

                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    }
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
