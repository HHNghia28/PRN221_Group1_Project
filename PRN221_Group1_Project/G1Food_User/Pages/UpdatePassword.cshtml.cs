using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class UpdatePasswordModel : PageModel
    {
        private readonly ILogger<UpdatePasswordModel> _logger;
        private readonly HttpClient _client;
        private readonly string _accountApiUrl;

        public List<AccountResponse> Accounts { get; set; }

        [BindProperty]
        public string accountID { get; set; }

        [BindProperty]
        public string newPassword { get; set; }

        public UpdatePasswordModel(ILogger<UpdatePasswordModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }

        public async Task OnGetAsync()
        {
            string emailForUpdatePassword = Request.Cookies["emailForUpdatePassword"];
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_accountApiUrl}getAllAccounts");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Accounts = JsonSerializer.Deserialize<List<AccountResponse>>(apiResponse.Data.ToString(), options);
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
            
            if(Accounts != null)
            {
                foreach (var account in Accounts)
                {
                    if(account.Email == emailForUpdatePassword)
                    {
                        accountID = account.Id.ToString();
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Get the value of 'accountID' from the form
                string aID = Request.Form["accountID"];

                // Get the value of 'password-confirm' from the form
                string newPass = Request.Form["password-confirm"];
                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_accountApiUrl}updatePassword?id={aID}", newPass);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    return RedirectToPage("./Login");
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
