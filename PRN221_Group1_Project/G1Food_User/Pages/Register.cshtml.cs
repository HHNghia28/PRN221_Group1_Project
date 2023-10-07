using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private readonly string _loginApiUrl;

        public AccountResponse Account { get; private set; }

        public RegisterModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _loginApiUrl = configuration.GetValue<string>("APIEndpoint:Login");
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_loginApiUrl}login");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Account = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);
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
