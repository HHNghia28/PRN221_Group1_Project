using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using G1FOODLibrary.Entities;
using G1FOODLibrary.DTO;
using System.Net.Http.Headers;
using System.Text.Json;
using DataAccess.Context;
using Microsoft.AspNetCore.Authorization;

namespace G1Food_Admin.Pages.Account
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly HttpClient _client;
        private readonly string _accountApiUrl;

        public AccountResponse Account { get; set; }

        public DeleteModel(ILogger<DeleteModel> logger, IConfiguration configuration)
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
                    Account = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);
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
                HttpResponseMessage response = await _client.DeleteAsync($"{_accountApiUrl}deleteAccount?id={id}");
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
