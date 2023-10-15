using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using G1FOODLibrary.Entities;
using G1FOODLibrary.DTO;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace G1Food_Admin.Pages.Account
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly HttpClient _client;
        private readonly string _accountApiUrl;
        [BindProperty]
        public AccountRequest Account { get; set; } = new AccountRequest();
        public List<AccountResponse> Accounts { get; set; }
        public List<StatusResponse> Status {  get; set; }
        public List<StatusResponse> Role { get; set; }

        public CreateModel(ILogger<CreateModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }

        public async Task OnGetAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_accountApiUrl}getRoles");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Role = JsonSerializer.Deserialize<List<StatusResponse>>(apiResponse.Data.ToString(), options);
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                }

                response = await _client.GetAsync($"{_accountApiUrl}getAccountStatus");
                response.EnsureSuccessStatusCode();

                stringData = await response.Content.ReadAsStringAsync();
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Status = JsonSerializer.Deserialize<List<StatusResponse>>(apiResponse.Data.ToString(), options);
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync($"{_accountApiUrl}addAccount", Account);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    return NotFound();
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

            return RedirectToPage("./Index");
        }
    }
}
