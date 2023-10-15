using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_Admin.Pages
{
    [Authorize]
    public class InventoryModel : PageModel
    {
        private readonly ILogger<InventoryModel> _logger;
        private readonly HttpClient _client;
        private readonly string _inventoryApiUrl;

        public IEnumerable<WarehouseResponse> Warehouses { get; set; }

        public InventoryModel(ILogger<InventoryModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _inventoryApiUrl = configuration.GetValue<string>("APIEndpoint:Warehouse");
        }
        public async Task OnGetAsync()
        {
            try
            {
                var tokenClaim = HttpContext.User.FindFirst("Token").Value;

                if (!string.IsNullOrEmpty(tokenClaim))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenClaim);
                }

                HttpResponseMessage response = await _client.GetAsync($"{_inventoryApiUrl}getWarehouses");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Warehouses = JsonSerializer.Deserialize<List<WarehouseResponse>>(apiResponse.Data.ToString(), options);
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
    }
}
