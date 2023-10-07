using Azure.Core;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private readonly string _authApiUrl;
        
        [BindProperty]
        public LoginRequest LoginRequest { get; set; }


        public LoginModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _authApiUrl = configuration.GetValue<string>("APIEndpoint:Auth");
        }

        public async Task<IActionResult> OnPostAsync()
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
                    return RedirectToPage("./Index");
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
            return Page();
        }
    }
}
