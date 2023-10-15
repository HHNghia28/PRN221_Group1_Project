using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G1FOODLibrary.Entities;
using G1FOODLibrary.DTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_Admin.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly HttpClient _client;
        private readonly string _accountApiUrl;

        public AccountResponse AccountResponse { get; set; }
        [BindProperty]
        public AccountRequest Account { get; set; }
        public AccountUpdateRequest UpdateRequest { get; set; }
        public List<StatusResponse> Status { get; set; }
        public List<StatusResponse> Role { get; set; }

        public EditModel(ILogger<EditModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }

        public async Task OnGetAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_accountApiUrl}getAccount?id={id}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    AccountResponse = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);

                    Account = new AccountRequest
                    {
                        AddressDetail = AccountResponse.AddressDetail,
                        Email = AccountResponse.Email,
                        Name = AccountResponse.Name,
                        Phone = AccountResponse.Phone,
                        RoleId = AccountResponse.RoleId,
                        StatusId = AccountResponse.StatusId
                    };
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                }

                response = await _client.GetAsync($"{_accountApiUrl}getRoles");
                response.EnsureSuccessStatusCode();

                stringData = await response.Content.ReadAsStringAsync();
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

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

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                UpdateRequest = new AccountUpdateRequest
                {
                    AddressDetail = Account.AddressDetail,
                    Name = Account.Name,
                    Phone = Account.Phone,
                    RoleId = Account.RoleId,
                    StatusId = Account.StatusId
                };

                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_accountApiUrl}updateAccount?id={id}", UpdateRequest);
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
