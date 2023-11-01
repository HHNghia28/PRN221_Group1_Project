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
    public class VerifyEmailUpdatePasswordModel : PageModel
    {
        private readonly ILogger<VerifyEmailUpdatePasswordModel> _logger;
        private readonly HttpClient _client;
        private readonly string _authApiUrl;

        public AccountRequest accountRequest { get; set; }

        [BindProperty]
        public string enteredTooken { get; set; }

        [BindProperty]
        public bool isWrongOTP { get; set; } = false;

        public string token { get; set; }
        public AccountResponse Account { get; private set; }

        [BindProperty]
        public string ResponseMessage { get; set; }

        public VerifyEmailUpdatePasswordModel(ILogger<VerifyEmailUpdatePasswordModel> logger, IConfiguration configuration)
        {
            token = "";
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _authApiUrl = configuration.GetValue<string>("APIEndpoint:Auth");
        }

        public async Task OnGet()
        {
            string email = Request.Query["email"];
            Response.Cookies.Append("emailForUpdatePassword", email);

            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync($"{_authApiUrl}sendMailAuthentication", email);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Extract the substring from the end to the specified index.
                int startIndex = stringData.LastIndexOf(':') + 2;
                token = stringData.Substring(startIndex, 6);
                Response.Cookies.Append("Token", token.ToString());
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

        public async Task<IActionResult> OnPost()
        {
            token = Request.Cookies["Token"];

            if (token == enteredTooken)
            {
                return RedirectToPage("/UpdatePassword");
            }
            else
            {
                isWrongOTP = true;
            }
            return Page();
        }
    }
}
